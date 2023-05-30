namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfSSMs;
    private int? _numberOfCNVs;
    private int? _numberOfSVs;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int NumberOfSSMs { get => _numberOfSSMs ?? GetNumberOfSSMs(); set => _numberOfSSMs = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCNVs { get => _numberOfCNVs ?? GetNumberOfCNVs(); set => _numberOfCNVs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSVs { get => _numberOfSVs ?? GetNumberOfSVs(); set => _numberOfSVs = value; }


    public SampleIndex[] Samples { get; set; }


    private int GetNumberOfDonors()
    {
        return Samples?
            .Select(sample => sample.Donor)
            .DistinctBy(donor => donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfSSMs()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.SSM != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCNVs()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.CNV != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfSVs()
    {
        return Samples?
            .Where(sample => sample.Variants != null)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.SV != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
