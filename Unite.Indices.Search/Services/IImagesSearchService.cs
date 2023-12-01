using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Context;
using Unite.Indices.Search.Services.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Images.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IImagesSearchService : ISearchService<ImageIndex, ImageSearchContext>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
    SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
    SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
}
