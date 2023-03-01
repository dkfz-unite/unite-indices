namespace Unite.Indices.Entities.Genes;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }
    public Basic.Genome.Transcriptomics.GeneExpressionIndex Expression { get; set; }

    public ImageIndex[] Images { get; set; }
    public VariantIndex[] Variants { get; set; }
}
