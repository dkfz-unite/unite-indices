namespace Unite.Indices.Entities.Images;

public class SampleIndex : Basic.Analysis.SampleIndex
{
    public Basic.Analysis.SampleDataIndex Data { get; set; }
    
    public Basic.Analysis.ResourceIndex[] Resources { get; set; }
}
