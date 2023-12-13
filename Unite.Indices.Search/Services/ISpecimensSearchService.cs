using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Specimens.DataIndex;

namespace Unite.Indices.Search.Services;

public interface ISpecimensSearchService : ISearchService<SpecimenIndex>
{
    SearchResult<GeneIndex> SearchGenes(SearchCriteria searchCriteria);
    SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria);
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria);
}
