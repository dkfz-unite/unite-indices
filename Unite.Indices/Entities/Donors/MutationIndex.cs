namespace Unite.Indices.Entities.Donors
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public SampleIndex[] Samples { get; set; }
    }
}
