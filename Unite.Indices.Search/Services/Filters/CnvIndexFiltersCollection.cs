using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Services.Filters;

public class CnvIndexFiltersCollection : VariantFiltersCollection
{
    public CnvIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new CopyNumberVariantFilters<VariantIndex>(criteria.Cnv, variant => variant);
        var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Cnv.AffectedFeatures.First().Gene);

        _filters.AddRange(filters.All());
        _filters.AddRange(geneFilters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.CnvIndex>(
            VariantFilterNames.Type,
            variant => variant.Cnv
        ));
    }
}
