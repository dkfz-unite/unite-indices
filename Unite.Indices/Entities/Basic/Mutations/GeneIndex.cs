namespace Unite.Indices.Entities.Basic.Mutations
{
    public class GeneIndex
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public bool? Strand { get; set; }

        public string EnsemblId { get; set; }
    }
}
