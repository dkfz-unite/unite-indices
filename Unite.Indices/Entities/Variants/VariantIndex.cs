namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
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
            .Select(sample => sample.Donor)
            .DistinctBy(donor => donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return GetAffectedFeatures()?
               .Where(feature => feature.Gene != null)
               .Select(feature => feature.Gene)
               .DistinctBy(gene => gene.Id)
               .Count() ?? 0;
    }
}
