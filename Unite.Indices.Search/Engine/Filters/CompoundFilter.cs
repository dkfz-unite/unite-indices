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
    public bool IsEmpty
    {
        get
        {
            if (Filters == null)
                return true;
            
            foreach (var filter in Filters)
            {
                if(filter.IsEmpty)
                    return true;
            }
            
            return false;
        }
    }

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
        var logicalOperator = LogicalOperator == LogicalOperator.And ? Operator.And : Operator.Or;
        
        return !IsEmpty ? QueryExtensions.CreateCompoundQuery(Filters.Select(x => x.CreateQuery()), logicalOperator) : null;
    }
}