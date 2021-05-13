namespace Unite.Indices.Entities.Donors
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public SpecimenIndex[] Specimens { get; set; }
    }
}
