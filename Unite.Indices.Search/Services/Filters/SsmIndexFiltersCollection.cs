using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Services.Filters;

public class SsmIndexFiltersCollection : VariantFiltersCollection
{
    public SsmIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new MutationFilters<VariantIndex>(criteria.Ssm, variant => variant);
        var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Ssm.AffectedFeatures.First().Gene);

        _filters.AddRange(filters.All());
        _filters.AddRange(geneFilters.All());

        Add(new NotNullFilter<VariantIndex, Indices.Entities.Basic.Genome.Variants.SsmIndex>(
            VariantFilterNames.Type,
            variant => variant.Ssm
        ));
    }
}
