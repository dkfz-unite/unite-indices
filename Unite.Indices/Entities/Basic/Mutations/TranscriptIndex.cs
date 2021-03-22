namespace Unite.Indices.Entities.Basic.Mutations
{
    public class TranscriptIndex
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Biotype { get; set; }
        public bool? Strand { get; set; }

        public string EnsemblId { get; set; }
    }
}
