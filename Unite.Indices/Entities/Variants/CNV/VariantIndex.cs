namespace Unite.Indices.Entities.Variants.CNV;

public class VariantIndex : Basic.Genome.Variants.CNV.VariantIndex
{
    public DonorIndex Donor { get; set; }
    public SpecimenIndex Specimen { get; set; }
    public ImageIndex[] Images { get; set; }

    /// <summary>
    /// Number of genes affected by the variant in the specimen
    /// </summary>
    public int NumberOfGenes { get; set; }
}
