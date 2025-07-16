using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base;

public abstract class FiltersCollection<T> where T : class
{
    protected const string _keywordSuffix = "keyword";

    protected readonly List<IFilter<T>> _filters = [];


    public virtual void Add(IFilter<T> filter)
    {
        _filters.Add(filter);
    }

    public virtual void Add(IEnumerable<IFilter<T>> filters)
    {
        _filters.AddRange(filters);
    }

    public virtual void Remove(string filterName)
    {
        var filter = _filters.FirstOrDefault(f => f.Name == filterName);

        if (filter != null)
        {
            _filters.Remove(filter);
        }
    }

    public virtual bool MakePositive()
    {
        var reverted = false;

        foreach (var filter in _filters)
        {
            if (filter.Not == true)
            {
                filter.Not = false;
                reverted = true;
            }
        }

        return reverted;
    }

    public virtual bool Any()
    {
        return _filters.Any();
    }

    public virtual IEnumerable<IFilter<T>> All()
    {
        return _filters;
    }

    public virtual IEnumerable<IFilter<T>> Except(params string[] filterNames)
    {
        return _filters.Where(filter => !filterNames.Contains(filter.Name));
    }

    protected virtual bool IsNotEmpty<TProp>(Criteria<TProp> criteria)
    {
        return criteria?.IsNotEmpty() == true;
    }
}
