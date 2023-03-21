namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMutations;
    private int? _numberOfCopyNumberVariants;
    private int? _numberOfStructuralVariants;

    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(); set => _numberOfDonors = value; }
    public int NumberOfMutations { get => _numberOfMutations ?? GetNumberOfMutations(); set => _numberOfMutations = value; }
    public int NumberOfCopyNumberVariants { get => _numberOfCopyNumberVariants ?? GetNumberOfCopyNumberVariants(); set => _numberOfCopyNumberVariants = value; }
    public int NumberOfStructuralVariants { get => _numberOfStructuralVariants ?? GetNumberOfStructuralVariants(); set => _numberOfStructuralVariants = value; }

    public SampleIndex[] Samples { get; set; }


    private int GetNumberOfDonors()
    {
        return Samples?
            .Select(sample => sample.Donor)
            .DistinctBy(donor => donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfMutations()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCopyNumberVariants()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfStructuralVariants()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
