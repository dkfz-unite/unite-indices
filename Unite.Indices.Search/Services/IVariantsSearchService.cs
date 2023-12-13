using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Variants.DataIndex;


namespace Unite.Indices.Search.Services;

public interface IVariantsSearchService : ISearchService<VariantIndex>
{
    SearchResult<DonorIndex> SearchDonors(SearchCriteria searchCriteria);
    IDictionary<string, DataIndex> Stats(SearchCriteria searchCriteria);
}
