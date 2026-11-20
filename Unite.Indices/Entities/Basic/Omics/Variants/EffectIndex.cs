namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class EffectIndex
{
    [Searchable]
    public string Type { get; set; }
    [Searchable]
    public string Impact { get; set; }
    public int Severity { get; set; }
}
