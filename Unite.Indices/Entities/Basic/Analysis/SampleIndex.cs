namespace Unite.Indices.Entities.Basic.Analysis;

public class SampleIndex
{
    public int Id { get; set; }    
    public string AnalysisType { get; set; }
    public int? AnalysisDay { get; set; }

    public double? Purity { get; set; }
    public double? Ploidy { get; set; }
    public int? CellsNumber { get; set; }
    public string GenesModel { get; set; }
}
