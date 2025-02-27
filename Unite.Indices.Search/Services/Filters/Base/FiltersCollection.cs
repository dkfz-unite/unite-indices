using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Criteria.Models;

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


    protected virtual bool IsNotEmpty(params string[] values)
    {
        return values?.Any(value => !string.IsNullOrWhiteSpace(value)) == true;
    }

    protected virtual bool IsNotEmpty(params bool[] values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(params bool?[] values)
    {
        return values?.Any(value => value.HasValue) == true;
    }

    protected virtual bool IsNotEmpty(params byte[] values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(params byte?[] values)
    {
        return values?.Any(value => value.HasValue) == true;
    }

    protected virtual bool IsNotEmpty(params int[] values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(params int?[] values)
    {
        return values?.Any(value => value.HasValue) == true;
    }

    protected virtual bool IsNotEmpty(params double[] values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(params double?[] values)
    {
        return values?.Any(value => value.HasValue) == true;
    }

    protected virtual bool IsNotEmpty(Range<int?> range)
    {
        return range?.From.HasValue == true || range?.To.HasValue == true;
    }

    protected virtual bool IsNotEmpty(Range<double?> range)
    {
        return range?.From.HasValue == true || range?.To.HasValue == true;
    }
}
