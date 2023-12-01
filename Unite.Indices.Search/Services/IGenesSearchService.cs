using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Genes.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IGenesSearchService : ISearchService<GeneIndex>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null);
    SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null);
    SearchResult<VariantIndex> SearchVariants(int geneId, VariantType type, SearchCriteria searchCriteria = null);
}
