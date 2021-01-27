namespace Unite.Indices.Entities.Mutations
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public DonorIndex[] Donors { get; set; }
    }
}
