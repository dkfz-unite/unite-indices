namespace Unite.Indices.Search.Services.Filters.Criteria.Models;

public record Range<T>
{
    public T From { get; set; }
    public T To { get; set; }
}
