using Unite.Indices.Entities.Basic.Samples;

namespace Unite.Indices.Entities.Basic.Mutations
{
    public class AnalysedSampleIndex
    {
        public int Id { get; set; }

        public SampleIndex Sample { get; set; }
        public AnalysisIndex Analysis { get; set; }
        public FileIndex File { get; set; }
    }
}
