namespace Unite.Indices.Entities.Basic.Analysis;

public class SampleIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    
    public double? Purity { get; set; }
    public double? Ploidy { get; set; }
    
    public AnalysisIndex[] Analyses { get; set; }
}
