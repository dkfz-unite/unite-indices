namespace Unite.Indices.Entities.Basic.Genome;

public class ProteinIndex
{
    public int Id { get; set; }
    public string StableId { get; set; }
    public bool? IsCanonical { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public int? Length { get; set; }
}
