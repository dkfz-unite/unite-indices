using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Donors.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IDonorsSearchService : ISearchService<DonorIndex>
{
    SearchResult<ImageIndex> SearchImages(SearchCriteria searchCriteria);
    SearchResult<SpecimenIndex> SearchSpecimens(SearchCriteria searchCriteria);
    SearchResult<GeneIndex> SearchGenes(SearchCriteria searchCriteria);
    SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria);
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria);
}
