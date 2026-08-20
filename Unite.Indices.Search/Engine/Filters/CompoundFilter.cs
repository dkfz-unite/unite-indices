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
    public bool IsEmpty => FilterA == null || FilterB == null;
    private IFilter<T> FilterA { get; set; }
    private IFilter<T> FilterB { get; set; }
    private LogicalOperator LogicalOperator { get; set; }
    
    public CompoundFilter(string name, bool? not, IFilter<T> filterA, IFilter<T> filterB, LogicalOperator logicalOperator)
    {
        Name = name;
        Not = not ?? false;
        FilterA = filterA;
        FilterB = filterB;
        LogicalOperator = logicalOperator;
    }
    
    public QueryContainer CreateQuery()
    {
        var logicalOperator = LogicalOperator == LogicalOperator.And ? Operator.And : Operator.Or;
        
        return !IsEmpty ? QueryExtensions.CreateCompoundQuery(FilterA.CreateQuery(), FilterB.CreateQuery(), logicalOperator) : null;
    }
}