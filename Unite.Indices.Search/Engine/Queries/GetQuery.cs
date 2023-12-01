using System.Linq.Expressions;
using Nest;

namespace Unite.Indices.Search.Engine.Queries;

public class GetQuery<T> where T : class
{
    private GetDescriptor<T> _request;


    public GetQuery(string key)
    {
        _request = new GetDescriptor<T>(key);
    }


    public GetQuery<T> AddExclusion<TProp>(Expression<Func<T, TProp>> property)
    {
        _request.SourceExcludes(property);

        return this;
    }


    public IGetRequest<T> GetRequest()
    {
        return _request;
    }
}
