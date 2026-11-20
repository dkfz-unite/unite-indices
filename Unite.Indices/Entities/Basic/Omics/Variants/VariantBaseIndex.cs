namespace Unite.Indices.Entities.Basic.Omics.Variants;

public abstract class VariantBaseIndex : VariantNavIndex
{
    [Searchable]
    public string Chromosome { get; set; }
    public string ChromosomeArm { get; set; }
    public int ChromosomeI => GetIndex(Chromosome);
    [Searchable]
    public int Start { get; set; }
    [Searchable]
    public int End { get; set; }
    [Searchable]
    public int? Length { get; set; }

    [Searchable]
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
