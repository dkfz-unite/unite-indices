namespace Unite.Indices.Entities.Basic.Mutations
{
    public class TranscriptConsequenceIndex
    {
        public string AminoAcidChange { get; set; }
        public string CodonChange { get; set; }

        public GeneIndex Gene { get; set; }
        public TranscriptIndex Transcript { get; set; }
        public ConsequenceIndex Consequence { get; set; }
    }
}
