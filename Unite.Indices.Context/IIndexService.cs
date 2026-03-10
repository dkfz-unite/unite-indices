using System.Linq.Expressions;

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
    /// Deletes documents where given property equals any of the given values.
    /// </summary>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="property">Property selector.</param>
    /// <param name="values">Property values.</param>
    /// <returns>Task.</returns>
    Task DeleteWhereEquals<TProp>(Expression<Func<T, TProp>> property, params TProp[] values);

    /// <summary>
    /// Create index collection if it does not exist.
    /// </summary>
    /// <returns>Task.</returns>
    Task CreateIndex();

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
