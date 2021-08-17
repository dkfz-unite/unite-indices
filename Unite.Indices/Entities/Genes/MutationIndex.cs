namespace Unite.Indices.Entities.Genes
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public DonorIndex[] Donors { get; set; }
    }
}
