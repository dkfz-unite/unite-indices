namespace Unite.Indices.Services;

public interface IIndexingService<T>
{
    Task IndexOne(T index);
    Task IndexMany(IEnumerable<T> indices);
    Task UpdateMapping();
    Task DeleteIndex();
}
