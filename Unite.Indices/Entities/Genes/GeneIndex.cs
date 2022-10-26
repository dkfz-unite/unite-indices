namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfImages;
    private int? _numberOfSpecimens;
    private int? _numberOfMutations;
    private int? _numberOfCopyNumberVariants;
    private int? _numberOfStructuralVariants;


    public SpecimenIndex[] Specimens { get; set; }


    /// <summary>
    /// Total number of donors having this gene affected
    /// </summary>
    public int NumberOfDonors
    {
        get => _numberOfDonors ?? GetNumberOfDonors();
        set => _numberOfDonors = value;
    }

    /// <summary>
    /// Total number of images of donors having this gene affected
    /// </summary>
    public int NumberofImages
    {
        get => _numberOfImages ?? GetNumberOfImages();
        set => _numberOfImages = value;
    }

    /// <summary>
    /// Total number of specimens having this gene affected
    /// </summary>
    public int NumberOfSpecimens
    {
        get => _numberOfSpecimens ?? GetNumberOfSpecimens();
        set => _numberOfSpecimens = value;
    }

    /// <summary>
    /// Total number of mutations affecting this gene across all donors
    /// </summary>
    public int NumberOfMutations
    {
        get => _numberOfMutations ?? GetNumberOfMutations();
        set => _numberOfMutations = value;
    }

    /// <summary>
    /// Total number of copy number variants affecting this gene across all donors
    /// </summary>
    public int NumberOfCopyNumberVariants
    {
        get => _numberOfCopyNumberVariants ?? GetNumberOfCopyNumberVariants();
        set => _numberOfCopyNumberVariants = value;
    }

    /// <summary>
    /// Total number of structural variants affecting this gene across all donors
    /// </summary>
    public int NumberOfStructuralVariants
    {
        get => _numberOfStructuralVariants ?? GetNumberOfStructuralVariants();
        set => _numberOfStructuralVariants = value;
    }


    private int GetNumberOfDonors()
    {
        return Specimens?
            .Select(specimen => specimen.Donor)
            .DistinctBy(donor => donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfImages()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Images)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfSpecimens()
    {
        return Specimens?.Length ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.AffectedFeatures != null)
            .SelectMany(variant => variant.AffectedFeatures)
            .Where(feature => feature.Gene != null)
            .Select(feature => feature.Gene)
            .DistinctBy(gene => gene.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfMutations()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCopyNumberVariants()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfStructuralVariants()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
