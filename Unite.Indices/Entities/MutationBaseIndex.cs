namespace Unite.Indices.Entities
{
    public class MutationBaseIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Chromosome { get; set; }
        public string Contig { get; set; }
        public string SequenceType { get; set; }
        public int Position { get; set; }
        public string Type { get; set; }

        public GeneIndex Gene { get; set; }
    }
}
