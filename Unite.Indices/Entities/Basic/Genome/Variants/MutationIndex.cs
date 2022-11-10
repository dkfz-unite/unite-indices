namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class MutationIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Length { get; set; }

    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }
}
