using System.Linq.Expressions;
using Nest;

namespace Unite.Indices.Search.Engine.Extensions;

public static class AggregationExtensions
{
    /// <summary>
    /// Adds 'terms' aggregation for given property to given request
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="name">Aggregation name</param>
    /// <param name="property">Aggregation property</param>
    /// <param name="size">Number of top met terms to return</param>
    public static void AddTermsAggregation<T, TProp>(this ISearchRequest<T> request,
        string name,
        Expression<Func<T, TProp>> property,
        int? size)
    {
        if (request.Aggregations == null)
        {
            request.Aggregations = new AggregationDictionary();
        }

        var termsAggregation = new TermsAggregation(name);
        termsAggregation.Field = new Field(property);
        termsAggregation.Size = size;

        var filterAggregation = new FilterAggregation(name);
        filterAggregation.Filter = request.Query;
        filterAggregation.Aggregations = termsAggregation;

        var aggregation = request.Query != null
            ? (AggregationContainer)filterAggregation
            : (AggregationContainer)termsAggregation;

        request.Aggregations.Add(name, aggregation);
    }

    /// <summary>
    /// Collects 'terms' aggregation results for given aggregation name from response
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="response">Response</param>
    /// <param name="name">Aggregation name</param>
    /// <returns></returns>
    public static IDictionary<string, long> GetTermsAggregationData<T>(this ISearchResponse<T> response,
        string name)
        where T : class
    {
        if (response.Aggregations.Filter(name) != null)
        {
            return response.Aggregations
                .Filter(name)?
                .Terms(name)?.Buckets
                .Where(bucket => bucket.DocCount != null)
                .ToDictionary(bucket => bucket.Key, bucket => bucket.DocCount.Value);
        }
        else
        {
            return response.Aggregations
                .Terms(name)?.Buckets
                .Where(bucket => bucket.DocCount != null)
                .ToDictionary(bucket => bucket.Key, bucket => bucket.DocCount.Value);
        }
    }
}
