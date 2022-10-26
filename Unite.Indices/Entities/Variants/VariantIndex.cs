namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfSpecimens;
    private int? _numberOfGenes;


    public SpecimenIndex[] Specimens { get; set; }


    /// <summary>
    /// Total number of donors affected by the variant
    /// </summary>
    public int NumberOfDonors
    {
        get => _numberOfDonors ?? GetNumberOfDonors();
        set => _numberOfDonors = value;
    }

    /// <summary>
    /// Total number of specimens affected by the variant across all donors
    /// </summary>
    public int NumberOfSpecimens
    {
        get => _numberOfSpecimens ?? GetNumberofSpecimens();
        set => _numberOfSpecimens = value;
    }

    /// <summary>
    /// Total number of genes affected by the variant across all donors
    /// </summary>
    public int NumberOfGenes
    {
        get => _numberOfGenes ?? GetNumberOfGenes();
        set => _numberOfGenes = value;
    }


    private int GetNumberOfDonors()
    {
        return Specimens?
            .Select(specimen => specimen.Donor)
            .DistinctBy(donor => donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberofSpecimens()
    {
        return Specimens?.Length ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return AffectedFeatures?
            .Where(feature => feature.Gene != null)
            .Select(feature => feature.Gene)
            .DistinctBy(gene => gene.Id)
            .Count() ?? 0;
    }
}
