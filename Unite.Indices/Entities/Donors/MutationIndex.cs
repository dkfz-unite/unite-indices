namespace Unite.Indices.Entities.Donors
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public Basic.Mutations.AnalysedSampleIndex[] Samples { get; set; }
    }
}
