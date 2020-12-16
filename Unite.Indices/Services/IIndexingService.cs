using System.Collections.Generic;

namespace Unite.Indices.Services
{
    public interface IIndexingService<T>
    {
        void IndexOne(T index);
        void IndexMany(IEnumerable<T> indices);
    }
}
