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
        CrossReferences.Add(new BucketCrossReference<ProjectIndex, ImageIndex>
        {
            DocumentKey = project => project.Donors.First().Id,
            ReferenceKey = image => image.Donor.Id
        });

        CrossReferences.Add(new BucketCrossReference<DonorIndex, ImageIndex>
        {
            DocumentKey = donor => donor.Id,
            ReferenceKey = image => image.Donor.Id
        });

        // Specimen level.
        CrossReferences.Add(new BucketCrossReference<SpecimenIndex, ImageIndex>
        {
            DocumentKey = specimen => specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Transcriptomics.
        CrossReferences.Add(new BucketCrossReference<GeneIndex, ImageIndex>
        {
            DocumentKey = gene => gene.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<GeneExpressionIndex, ImageIndex>
        {
            DocumentKey = expression => expression.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Proteomics.
        CrossReferences.Add(new BucketCrossReference<ProteinIndex, ImageIndex>
        {
            DocumentKey = protein => protein.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<ProteinExpressionIndex, ImageIndex>
        {
            DocumentKey = expression => expression.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        // Genome variants.
        CrossReferences.Add(new BucketCrossReference<SmIndex, ImageIndex>
        {
            DocumentKey = sm => sm.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<CnvIndex, ImageIndex>
        {
            DocumentKey = cnv => cnv.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<SvIndex, ImageIndex>
        {
            DocumentKey = sv => sv.Specimens.First().Id,
            ReferenceKey = image => image.Specimens.First().Id
        });

        CrossReferences.Add(new BucketCrossReference<CnvProfileIndex, ImageIndex>
        {
            DocumentKey = profile => profile.Specimen.Id,
            ReferenceKey = image => image.Specimens.First().Id
        });
    }
}
