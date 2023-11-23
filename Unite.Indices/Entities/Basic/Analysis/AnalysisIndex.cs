namespace Unite.Indices.Entities.Basic.Analysis;

public class AnalysisIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    
    public string Type { get; set; }
    public int? Day { get; set; }

    public double? Purity { get; set; }
    public double? Ploidy { get; set; }
}
