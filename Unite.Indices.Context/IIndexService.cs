namespace Unite.Indices.Context;

/// <summary>
/// Index write service. Can add, update and delete indices.
/// </summary>
public interface IIndexService<T> 
    where T : class
{
    /// <summary>
    /// Adds an element to the index.
    /// </summary>
    /// <param name="element">Element.</param>
    /// <returns>Task.</returns>
    Task Add(T element);

    /// <summary>
    /// Adds a range of elements to the index.
    /// </summary>
    /// <param name="elements">Elements.</param>
    /// <returns>Task.</returns>
    Task AddRange(IEnumerable<T> elements);

    /// <summary>
    /// Deletes document with given id from the index.
    /// </summary>
    /// <param name="key">Document id.</param>
    /// <returns>Task.</returns>
    Task Delete(string key);

    /// <summary>
    /// Deletes a range of documents with given ids from the index.
    /// </summary>
    /// <param name="elements">Document ids.</param>
    /// <returns>Task.</returns>
    Task DeleteRange(IEnumerable<string> keys);

    /// <summary>
    /// Update index collection mapping.
    /// </summary>
    /// <returns>Task.</returns> 
    Task UpdateIndex();

    /// <summary>
    /// Delete index collection.
    /// </summary>
    /// <returns>Task.</returns>
    Task DeleteIndex();
}
