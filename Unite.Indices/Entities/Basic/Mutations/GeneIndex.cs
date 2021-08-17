﻿namespace Unite.Indices.Entities.Basic.Mutations
{
    public class GeneIndex
    {
        public int Id { get; set; }

        public string Symbol { get; set; }
        public string Biotype { get; set; }
        public string Chromosome { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public bool? Strand { get; set; }

        public string EnsemblId { get; set; }
    }
}
