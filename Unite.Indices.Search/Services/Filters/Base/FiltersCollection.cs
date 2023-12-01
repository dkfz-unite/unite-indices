using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria.Models;

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

    public virtual IEnumerable<IFilter<T>> All()
    {
        return _filters;
    }

    public virtual IEnumerable<IFilter<T>> Except(params string[] filterNames)
    {
        return _filters.Where(filter => !filterNames.Contains(filter.Name));
    }


    protected virtual bool IsNotEmpty(IEnumerable<string> values)
    {
        return values?.Any(value => !string.IsNullOrWhiteSpace(value)) == true;
    }

    protected virtual bool IsNotEmpty(IEnumerable<int> values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(IEnumerable<int?> values)
    {
        return values?.Any(value => value.HasValue) == true;
    }

    protected virtual bool IsNotEmpty(IEnumerable<double> values)
    {
        return values?.Any() == true;
    }

    protected virtual bool IsNotEmpty(IEnumerable<double?> values)
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

    protected virtual bool IsNotEmpty(bool? value)
    {
        return value.HasValue;
    }
}
