namespace Unite.Indices.Entities.Basic.Analysis;

public class SampleIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    
    public AnalysisIndex[] Analyses { get; set; }
}
