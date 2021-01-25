namespace Unite.Indices.Entities.Mutations
{
    public class SampleIndex : Basic.Mutations.SampleIndex
    {
        // public CellLineIndex CellLine { get; set; };
        // public XenograftIndex Xenograft { get; set; };

        public Basic.FileIndex File { get; set; }
        public Basic.Mutations.AnalysisIndex Analysis { get; set; }
        public Basic.Mutations.SampleIndex[] MatchedSamples { get; set; }
    }
}
