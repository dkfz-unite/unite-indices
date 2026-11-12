using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using CnvProfileIndex = Unite.Indices.Entities.CnvProfiles.CnvProfileIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneExpressionIndex = Unite.Indices.Entities.Genes.GeneExpressionIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using ProteinExpressionIndex = Unite.Indices.Entities.Proteins.ProteinExpressionIndex;
using ProteinIndex = Unite.Indices.Entities.Proteins.ProteinIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;

namespace Unite.Indices.Search.Engine2.Criteria;

public class CriteriaContainer
{
    public DocumentCriteria<ProjectIndex> ProjectCriteria { get; set; }
    public DocumentCriteria<DonorIndex> DonorCriteria { get; set; }
    public DocumentCriteria<ImageIndex> ImageCriteria { get; set; }
    public DocumentCriteria<SpecimenIndex> SpecimenCriteria { get; set; }
    public DocumentCriteria<GeneIndex> GeneCriteria { get; set; }
    public DocumentCriteria<SmIndex> SmCriteria { get; set; }
    public DocumentCriteria<CnvIndex> CnvCriteria { get; set; }
    public DocumentCriteria<SvIndex> SvCriteria { get; set; }
    public DocumentCriteria<CnvProfileIndex> CnvProfileCriteria { get; set; }
    public DocumentCriteria<GeneExpressionIndex> GeneExpressionCriteria { get; set; }
    public DocumentCriteria<ProteinIndex> ProteinCriteria { get; set; }
    public DocumentCriteria<ProteinExpressionIndex> ProteinExpressionCriteria { get; set; }
}
