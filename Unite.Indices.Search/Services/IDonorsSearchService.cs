using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Donors.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IDonorsSearchService : ISearchService<DonorIndex>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null);
    SearchResult<ImageIndex> SearchImages(int donorId, ImageType type, SearchCriteria searchCriteria = null);
    SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null);
    SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null);
    SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null);
}
