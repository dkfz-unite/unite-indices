namespace Unite.Indices.Entities.Basic.Mutations
{
    public class GeneIndex
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string[] Synonyms { get; set; }
        public string Name { get; set; }
        public string Chromosome { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public bool? Strand { get; set; }

        public GeneInfoIndex GeneInfo { get; set; }
    }
}
