using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMRIs;
    private int? _numberOfCTs;
    private int? _numberOfTissues;
    private int? _numberOfCells;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfGenes;


    /// <summary>
    /// Number of donors with with the variant in any sample.
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of genes with the variant in any sample.
    /// </summary>
    public int NumberOfGenes { get => _numberOfGenes ?? GetNumberOfGenes(); set => _numberOfGenes = value; }


    public SampleIndex[] Samples { get; set; }


    private int GetNumberOfDonors()
    {
        return Samples?
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfMRIs()
    {
        return Samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCTs()
    {
        return Samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.CT)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfTissues()
    {
        return Samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Tissue)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCells()
    {
        return Samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Cell)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfOrganoids()
    {
        return Samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Organoid)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfXenografts()
    {
        return Samples?
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
