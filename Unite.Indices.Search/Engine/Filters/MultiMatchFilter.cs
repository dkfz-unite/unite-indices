using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class MultiMatchFilter<T> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public string Value { get; }

    public MultiMatchFilter(string name, string value)
    {
        Name = name;
        Not = false;
        Value = value;
    }

    public MultiMatchFilter(string name, bool not, string value)
    {
        Name = name;
        Not = not;
        Value = value;
    }

    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateMultiMatchQuery<T>(Value) : null;
    }
}
