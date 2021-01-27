namespace Unite.Indices.Entities.Basic.Mutations
{
    public class AnalysedSampleIndex : SampleIndex
    {
        public AnalysisIndex Analysis { get; set; }
        public FileIndex File { get; set; }
        
        public SampleIndex[] MatchedSamples { get; set; }
    }
}
