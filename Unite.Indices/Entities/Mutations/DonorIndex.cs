namespace Unite.Indices.Entities.Mutations
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public SpecimenIndex[] Specimens { get; set; }
    }
}
