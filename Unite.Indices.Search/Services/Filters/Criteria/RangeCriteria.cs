using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Criteria;

public class RangeCriteria<T> : Criteria<Range<T>>
{
    public override bool IsNotEmpty()
    {
        return Value != null && (Value.From != null || Value.To != null);
    }
}
