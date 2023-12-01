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
    /// Deletes an element from the index.
    /// </summary>
    /// <param name="element">Element.</param>
    /// <returns>Task.</returns>
    // Task Delete(T element);

    /// <summary>
    /// Deletes a range of elements from the index.
    /// </summary>
    /// <param name="elements">Elements.</param>
    /// <returns>Task.</returns>
    // Task DeleteRange(IEnumerable<T> elements);


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
