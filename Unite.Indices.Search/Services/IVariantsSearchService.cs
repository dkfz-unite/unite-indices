using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Context;
using Unite.Indices.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Variants.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IVariantsSearchService : ISearchService<VariantIndex, VariantSearchContext>
{
    IDictionary<long, DataIndex> Stats(SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null);
    SearchResult<DonorIndex> SearchDonors(string variantId, SearchCriteria searchCriteria = null, VariantSearchContext searchContext = null);
}
