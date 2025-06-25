namespace Unite.Indices.Search.Services.Filters.Criteria;

public abstract class Criteria<T>
{
    public virtual T Value { get; set; }
    public virtual bool? Not { get; set; }

    public virtual int Length => Value is null ? 0 : 1;

    public Criteria()
    {
        Value = default;
        Not = null;
    }

    public Criteria(T value, bool? not = null)
    {
        Value = value;
        Not = not;
    }

    public virtual bool IsNotEmpty()
    {
        return Value != null;
    }
}
