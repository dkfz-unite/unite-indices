namespace Unite.Indices.Entities.Genes
{
    public class MutationIndex : Basic.Genome.Mutations.MutationIndex
    {
        public DonorIndex[] Donors { get; set; }
    }
}
