namespace Unite.Indices.Entities.Mutations
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public Basic.Mutations.AnalysedSampleIndex[] Samples { get; set; }
    }
}
