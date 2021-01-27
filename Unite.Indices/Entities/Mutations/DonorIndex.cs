namespace Unite.Indices.Entities.Mutations
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public AnalysedSampleIndex[] Samples { get; set; }
    }
}
