namespace Unite.Indices.Entities.Projects.Stats.Base;

public record Stat<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }


    public Stat()
    {
    }

    public Stat(TKey key)
    {
        Key = key;
    }

    public Stat(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
