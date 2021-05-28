namespace Unite.Indices.Entities.Specimens
{
    public class SpecimenIndex : Basic.Specimens.SpecimenIndex
    {
        public DonorIndex Donor { get; set; }

        public MutationIndex[] Mutations { get; set; }
    }
}
