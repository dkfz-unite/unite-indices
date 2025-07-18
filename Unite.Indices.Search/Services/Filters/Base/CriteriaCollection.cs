using Unite.Essentials.Extensions;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base;

public abstract record CriteriaCollection
{
    public virtual bool IsNotEmpty()
    {
        var criterias = GetCriteria().ToArray();
        
        return criterias.Any(criteria => criteria.IsNotEmpty());
    }

    public virtual bool IsNegative()
    {
        var criterias = GetCriteria().ToArray();

        return criterias.Where(criteria => criteria.IsNotEmpty())
                        .All(criteria => criteria.Not == true);
    }

    public virtual void MakePositive()
    {
        var criterias = GetCriteria().ToArray();

        criterias.ForEach(criteria => criteria.Not = false);
    }


    protected virtual IEnumerable<CriteriaBase> GetCriteria()
    {
        var properties = GetType().GetProperties().Where(prop => typeof(CriteriaBase).IsAssignableFrom(prop.PropertyType));

        return properties
            .Select(prop => (CriteriaBase)prop.GetValue(this))
            .Where(criteria => criteria != null);
    }
}
