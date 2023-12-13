using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Genes.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IGenesSearchService : ISearchService<GeneIndex>
{
    SearchResult<DonorIndex> SearchDonors(SearchCriteria searchCriteria);
    SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria);
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null);
}
