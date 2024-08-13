namespace Unite.Indices.Entities.Donors;

public class SampleIndex : Basic.Analysis.SampleIndex
{
    public Basic.Analysis.SampleDataIndex Data { get; set; }
    
    public Basic.Analysis.ResourceIndex[] Resources { get; set; }
}
