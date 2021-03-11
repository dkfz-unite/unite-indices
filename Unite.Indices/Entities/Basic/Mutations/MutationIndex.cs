namespace Unite.Indices.Entities.Basic.Mutations
{
    public class MutationIndex
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Chromosome { get; set; }
        public string SequenceType { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string Alt { get; set; }

        public TranscriptConsequenceIndex[] TranscriptConsequences { get; set; }
    }
}
