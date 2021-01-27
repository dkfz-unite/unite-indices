namespace Unite.Indices.Entities.Donors
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public MutationIndex[] Mutations { get; set; }
    }
}