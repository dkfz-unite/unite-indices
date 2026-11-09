using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public enum LogicalOperator
{
    And,
    Or
}

public class CompoundFilter<T> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => Filters == null || Filters.All(filter => filter.IsEmpty);

    private IList<IFilter<T>> Filters { get; set; }
    private LogicalOperator LogicalOperator { get; set; }

    public CompoundFilter(string name, bool? not, IList<IFilter<T>> filters, LogicalOperator logicalOperator)
    {
        Name = name;
        Not = not ?? false;
        Filters = filters;
        LogicalOperator = logicalOperator;
    }

    public QueryContainer CreateQuery()
    {
        if (IsEmpty)
            return null;

        var queries = Filters
            .Where(filter => !filter.IsEmpty)
            .Select(filter => filter.Not ? !filter.CreateQuery() : filter.CreateQuery()); // !q → bool.must_not [q]

        return LogicalOperator == LogicalOperator.And
            ? queries.Aggregate((left, right) => left && right)
            : queries.Aggregate((left, right) => left || right);
    }
}
