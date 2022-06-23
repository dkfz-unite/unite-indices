namespace Unite.Indices.Services;

public interface IIndexCreationService<T> where T : class
{
    T CreateIndex(object key);
}
