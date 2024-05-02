namespace Unite.Indices.Entities;

public record Stat<T>
{
    public string Key { get; init; }
    public T Value { get; init; }
}
