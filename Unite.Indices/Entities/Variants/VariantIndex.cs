namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfGenes;

    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(); set => _numberOfDonors = value; }
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
