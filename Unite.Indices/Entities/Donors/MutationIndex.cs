namespace Unite.Indices.Entities.Donors
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public AnalysedSampleIndex[] Samples { get; set; }
    }
}
