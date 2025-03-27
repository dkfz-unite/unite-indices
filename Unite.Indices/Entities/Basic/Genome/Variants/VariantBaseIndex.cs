namespace Unite.Indices.Entities.Basic.Genome.Variants;

public abstract class VariantBaseIndex : VariantNavIndex
{
    public string Chromosome { get; set; }
    public int ChromosomeI => GetIndex(Chromosome);
    public int Start { get; set; }
    public int End { get; set; }
    public int? Length { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }


    protected int GetIndex(string chromosome)
    {
        return chromosome switch
        {
            "X" => 23,
            "Y" => 24,
            "MT" => 25,
            _ => int.Parse(chromosome)
        };
    }
}
