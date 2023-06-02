using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMris;
    private int? _numberOfCts;
    private int? _numberOfTissues;
    private int? _numberOfCells;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfGenes;


    /// <summary>
    /// Number of donors with the variant in any sample.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Samples); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRIs with the variant in any sample.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfMris(Samples); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CTs with the variant in any sample.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfCts(Samples); set => _numberOfCts = value; }

    /// <summary>
    /// Number of tissues with the variant in any sample.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfTissues(Samples); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with the variant in any sample.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfCells(Samples); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with the variant in any sample.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfOrganoids(Samples); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with the variant in any sample.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfXenografts(Samples); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of genes affected by the variant in any sample.
    /// </summary>
    public int NumberOfGenes { get => _numberOfGenes ?? GetNumberOfGenes(); set => _numberOfGenes = value; }


    public SampleIndex[] Samples { get; set; }


    public static int GetNumberOfDonors(SampleIndex[] samples)
    {
        return samples?
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfMris(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCts(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.CT)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfTissues(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Tissue)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCells(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Cell)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfOrganoids(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Organoid)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfXenografts(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Xenograft)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return GetAffectedFeatures()?
               .Where(feature => feature.Gene != null)
               .DistinctBy(feature => feature.Gene.Id)
               .Count() ?? 0;
    }
}
