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

namespace Unite.Indices.Search.Engine2.Services;

/// <summary>
/// Searches images. Projects and donors are linked by donor, specimens and all omics data by specimen.
/// </summary>
public class ImagesSearchService : SearchService<ImageIndex>
{
    public ImagesSearchService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        // Donor level.
        CrossReferences.Add(new BucketCrossReference<ProjectIndex, ImageIndex, int>
        {
            DocumentKey = project => project.Donors.First().Id,
            ReferenceKey = image => image.Donor.Id
        });

        CrossReferences.Add(new BucketCrossReference<DonorIndex, ImageIndex, int>
        {
            DocumentKey = donor => donor.Id,
            ReferenceKey = image => image.Donor.Id
        });

        // Specimen level.
        CrossReferences.Add(new BucketCrossReference<SpecimenIndex, ImageIndex, int>
        {
            DocumentKey = specimen => specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Transcriptomics.
        CrossReferences.Add(new BucketCrossReference<GeneIndex, ImageIndex, int>
        {
            DocumentKey = gene => gene.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<GeneExpressionIndex, ImageIndex, int>
        {
            DocumentKey = expression => expression.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Proteomics.
        CrossReferences.Add(new BucketCrossReference<ProteinIndex, ImageIndex, int>
        {
            DocumentKey = protein => protein.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<ProteinExpressionIndex, ImageIndex, int>
        {
            DocumentKey = expression => expression.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Genome variants.
        CrossReferences.Add(new BucketCrossReference<SmIndex, ImageIndex, int>
        {
            DocumentKey = sm => sm.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<CnvIndex, ImageIndex, int>
        {
            DocumentKey = cnv => cnv.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<SvIndex, ImageIndex, int>
        {
            DocumentKey = sv => sv.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<CnvProfileIndex, ImageIndex, int>
        {
            DocumentKey = profile => profile.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });
    }
}
