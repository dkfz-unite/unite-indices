using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Context;
using Unite.Indices.Search.Services.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Specimens.DataIndex;

namespace Unite.Indices.Search.Services;

public interface ISpecimensSearchService : ISearchService<SpecimenIndex, SpecimenSearchContext>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
    SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
    SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
}
