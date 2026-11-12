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

namespace Unite.Indices.Search.Engine2.InputFilters;

public class QueryContainer
{
    public DocumentQuery<ProjectIndex> ProjectQuery { get; set; }
    public DocumentQuery<DonorIndex> DonorQuery { get; set; }
    public DocumentQuery<ImageIndex> ImageQuery { get; set; }
    public DocumentQuery<SpecimenIndex> SpecimenQuery { get; set; }
    public DocumentQuery<GeneIndex> GeneQuery { get; set; }
    public DocumentQuery<SmIndex> SmQuery { get; set; }
    public DocumentQuery<CnvIndex> CnvQuery { get; set; }
    public DocumentQuery<SvIndex> SvQuery { get; set; }
    public DocumentQuery<CnvProfileIndex> CnvProfileQuery { get; set; }
    public DocumentQuery<GeneExpressionIndex> GeneExpressionQuery { get; set; }
    public DocumentQuery<ProteinIndex> ProteinQuery { get; set; }
    public DocumentQuery<ProteinExpressionIndex> ProteinExpressionQuery { get; set; }
}
