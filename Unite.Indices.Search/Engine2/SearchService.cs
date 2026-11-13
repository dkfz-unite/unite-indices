using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2;

public interface IBucketCrossReference
{
    Type DocumentType { get; }
    Type TargetType { get; }
    LambdaExpression DocumentKey { get; }
    LambdaExpression ReferenceKey { get; }

    /// <summary>
    /// Searches the referenced bucket with its criteria from the container and returns criteria
    /// on the target's reference key with the found document keys.
    /// Returns null if the container has no criteria for the referenced bucket (nothing to constrain).
    /// </summary>
    Task<CrossReferenceResult> FindReferences(CriteriaContainer criteria, IServiceProvider serviceProvider);
}

/// <summary>
/// Criteria on the target's reference key and the number of keys found in the referenced bucket.
/// </summary>
public record CrossReferenceResult(PropertyCriteria Criteria, int Count);

public class BucketCrossReference<TDocument, TTarget, TKey> : IBucketCrossReference
    where TDocument : class
    where TTarget : class
{
    private static readonly TypeConverter _keyConverter = TypeDescriptor.GetConverter(typeof(TKey));

    public Type DocumentType => typeof(TDocument);
    public Type TargetType => typeof(TTarget);

    public Expression<Func<TDocument, TKey>> DocumentKey { get; init; }
    public Expression<Func<TTarget, TKey>> ReferenceKey { get; init; }

    LambdaExpression IBucketCrossReference.DocumentKey => DocumentKey;
    LambdaExpression IBucketCrossReference.ReferenceKey => ReferenceKey;


    public async Task<CrossReferenceResult> FindReferences(CriteriaContainer criteria, IServiceProvider serviceProvider)
    {
        var filter = DocumentCriteriaConverter.Convert(criteria.Get<TDocument>());

        if (filter.IsEmpty)
            return null;

        var aggregationName = $"{typeof(TDocument).Name}.references";

        var searchQuery = new SearchQuery<TDocument>()
            .AddPagination(0, 0)
            .AddFilter(filter)
            .AddAggregation(aggregationName, DocumentKey);

        var indexService = serviceProvider.GetRequiredService<IIndexService<TDocument>>();
        var result = await indexService.Search(searchQuery);

        // Aggregation bucket keys are strings, parse them back to the key type.
        var keys = result.Aggregations.TryGetValue(aggregationName, out var buckets) && buckets != null
            ? buckets.Keys.Select(key => (TKey)_keyConverter.ConvertFromInvariantString(key)).ToList()
            : [];

        // Reference keys are internal: the field is created from the key directly, not taken from the searchable fields.
        var referenceCriteria = new ValueCriteria<TKey>
        {
            Property = new SearchField<TTarget, TKey>(ReferenceKey.Body.ToString(), ReferenceKey),
            Values = keys
        };

        return new CrossReferenceResult(referenceCriteria, keys.Count);
    }
}

public class SearchService<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public List<IBucketCrossReference> CrossReferences { get; } = new();


    public SearchService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<SearchResult<T>> Search(CriteriaContainer criteria, int from = 0, int size = 20)
    {
        var referenceCriteria = new List<CriteriaNode>();

        foreach (var crossReference in CrossReferences)
        {
            if (crossReference.TargetType != typeof(T))
                throw new InvalidOperationException($"Cross reference to '{crossReference.TargetType.Name}' can't be used to search '{typeof(T).Name}'.");

            var reference = await crossReference.FindReferences(criteria, _serviceProvider);

            if (reference == null)
                continue;

            // Referenced bucket was filtered, but nothing was found: no target document can match.
            if (reference.Count == 0)
                return new SearchResult<T>();

            referenceCriteria.Add(reference.Criteria);
        }

        var targetCriteria = CreateTargetCriteria(criteria, referenceCriteria);
        var filter = DocumentCriteriaConverter.Convert(targetCriteria.Get<T>());

        var searchQuery = new SearchQuery<T>()
            .AddPagination(from, size)
            .AddFilter(filter);

        var indexService = _serviceProvider.GetRequiredService<IIndexService<T>>();

        return await indexService.Search(searchQuery);
    }

    /// <summary>
    /// Copies the input criteria and combines the target's own criteria with the reference criteria (AND).
    /// The input criteria are not modified.
    /// </summary>
    private static CriteriaContainer CreateTargetCriteria(CriteriaContainer criteria, List<CriteriaNode> referenceCriteria)
    {
        var nodes = new List<CriteriaNode>();

        var ownCriteria = criteria.Get<T>();

        if (ownCriteria != null)
            nodes.Add(ownCriteria);

        nodes.AddRange(referenceCriteria);

        var targetCriteria = criteria.Copy();

        targetCriteria.Set(new DocumentCriteria<T>
        {
            Operator = LogicalOperator.And,
            Nodes = nodes
        });

        return targetCriteria;
    }
}
