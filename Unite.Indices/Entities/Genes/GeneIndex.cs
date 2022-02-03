namespace Unite.Indices.Entities.Genes
{
    public class GeneIndex : Basic.Genome.GeneIndex
    {
        public MutationIndex[] Mutations { get; set; }

        /// <summary>
        /// Number of donors having this gene affected
        /// </summary>
        public int NumberOfDonors { get; set; }

        /// <summary>
        /// Number of mutations affecting this gene
        /// </summary>
        public int NumberOfMutations { get; set; }
    }
}
