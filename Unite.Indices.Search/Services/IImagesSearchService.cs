using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Images.DataIndex;

namespace Unite.Indices.Search.Services;

public interface IImagesSearchService : ISearchService<ImageIndex>
{
    SearchResult<GeneIndex> SearchGenes(SearchCriteria searchCriteria);
    SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria);
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria);
}
