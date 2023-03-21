namespace Unite.Indices.Entities.Genes;

public class SampleIndex : Basic.Genome.Analysis.SampleIndex
{
    public DonorIndex Donor { get; set; }
    public SpecimenIndex Specimen { get; set; }
    public GeneExpressionIndex Expression { get; set; }

    public ImageIndex[] Images { get; set; }
    public VariantIndex[] Variants { get; set; }
}
