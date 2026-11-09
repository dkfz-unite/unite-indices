using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.InputFilters;

namespace Unite.Indices.Search.Engine2;

public interface IBucketCrossReference
{
    Type DocumentType { get; }
    Type TargetType { get; }
    LambdaExpression DocumentKey { get; }
    LambdaExpression ReferenceKey { get; }

    /// <summary>
    /// Searches the referenced bucket with its query from the container and returns a filter
    /// on the target's reference key with the found document keys.
    /// Returns null if the container has no query for the referenced bucket (nothing to constrain).
    /// </summary>
    Task<ValueFilter> FindReferences(QueryContainer query, IServiceProvider serviceProvider);
}

public class BucketCrossReference<TDocument, TTarget> : IBucketCrossReference
    where TDocument : class
    where TTarget : class
{
    public Type DocumentType => typeof(TDocument);
    public Type TargetType => typeof(TTarget);

    public Expression<Func<TDocument, object>> DocumentKey { get; init; }
    public Expression<Func<TTarget, object>> ReferenceKey { get; init; }

    LambdaExpression IBucketCrossReference.DocumentKey => DocumentKey;
    LambdaExpression IBucketCrossReference.ReferenceKey => ReferenceKey;


    public async Task<ValueFilter> FindReferences(QueryContainer query, IServiceProvider serviceProvider)
    {
        var filter = DocumentQueryConverter.Convert(query.Get<TDocument>());

        if (filter.IsEmpty)
            return null;

        var aggregationName = $"{typeof(TDocument).Name}.{PropertyExpressionBuilder.GetPath(DocumentKey)}";

        var searchQuery = new SearchQuery<TDocument>()
            .AddPagination(0, 0)
            .AddFilter(filter)
            .AddAggregation(aggregationName, DocumentKey);

        var indexService = serviceProvider.GetRequiredService<IIndexService<TDocument>>();
        var result = await indexService.Search(searchQuery);

        var keys = result.Aggregations.TryGetValue(aggregationName, out var buckets) && buckets != null
            ? buckets.Keys.ToList()
            : [];

        return new ValueFilter
        {
            FilterType = DocumentQueryConverter.GetFilterType(PropertyExpressionBuilder.GetPropertyType(ReferenceKey)),
            Property = PropertyExpressionBuilder.GetPath(ReferenceKey),
            Values = keys
        };
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

    public async Task<SearchResult<T>> Search(QueryContainer query, int from = 0, int size = 20)
    {
        var referenceFilters = new List<FilterNode>();

        foreach (var crossReference in CrossReferences)
        {
            if (crossReference.TargetType != typeof(T))
                throw new InvalidOperationException($"Cross reference to '{crossReference.TargetType.Name}' can't be used to search '{typeof(T).Name}'.");

            var referenceFilter = await crossReference.FindReferences(query, _serviceProvider);

            if (referenceFilter == null)
                continue;

            // Referenced bucket was filtered, but nothing was found: no target document can match.
            if (referenceFilter.Values.Count == 0)
                return new SearchResult<T>();

            referenceFilters.Add(referenceFilter);
        }

        var targetQuery = CreateTargetQuery(query, referenceFilters);
        var filter = DocumentQueryConverter.Convert(targetQuery.Get<T>());

        var searchQuery = new SearchQuery<T>()
            .AddPagination(from, size)
            .AddFilter(filter);

        var indexService = _serviceProvider.GetRequiredService<IIndexService<T>>();

        return await indexService.Search(searchQuery);
    }

    /// <summary>
    /// Copies the input query and combines the target's own filters with the reference filters (AND).
    /// The input query is not modified.
    /// </summary>
    private static QueryContainer CreateTargetQuery(QueryContainer query, List<FilterNode> referenceFilters)
    {
        var filterNodes = new List<FilterNode>();

        var ownFilterGroup = query.Get<T>()?.FilterGroup;

        if (ownFilterGroup != null)
            filterNodes.Add(ownFilterGroup);

        filterNodes.AddRange(referenceFilters);

        var targetQuery = query.Copy();

        targetQuery.Set(new DocumentQuery<T>
        {
            FilterGroup = new FilterGroup { Operator = LogicalOperator.And, FilterNodes = filterNodes }
        });

        return targetQuery;
    }

    private static FilterNode FindFilter(IDocumentQuery documentQuery, IBucketCrossReference crossReference)
    {
        var path = PropertyExpressionBuilder.GetPath(crossReference.ReferenceKey);

        return FindFilter(documentQuery?.FilterGroup, path);
    }

    private static FilterNode FindFilter(FilterNode node, string path)
    {
        return node switch
        {
            null => null,
            FilterGroup group => group.FilterNodes?
                .Select(child => FindFilter(child, path))
                .FirstOrDefault(found => found != null),
            _ => string.Equals(node.Property, path, StringComparison.OrdinalIgnoreCase) ? node : null
        };
    }
}
