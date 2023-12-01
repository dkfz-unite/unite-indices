using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Services.Filters;

public class SvIndexFiltersCollection : VariantFiltersCollection
{
    public SvIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new StructuralVariantFilters<VariantIndex>(criteria.Sv, variant => variant);
        var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Sv.AffectedFeatures.First().Gene);

        _filters.AddRange(filters.All());
        _filters.AddRange(geneFilters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.SvIndex>(
            VariantFilterNames.Type,
            variant => variant.Sv
        ));
    }
}
