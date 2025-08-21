namespace Unite.Indices.Search.Services.Filters.Criteria;

public record ValuesCriteria<T> : Criteria<T[]>
{
    public override int Length => Value?.Length ?? 0;

    public override bool IsNotEmpty()
    {
        return Value?.Length > 0;
    }

    public ValuesCriteria() : base()
    {
    }

    public ValuesCriteria(T[] value, bool? not = null) : base(value, not)
    {
    }
}
