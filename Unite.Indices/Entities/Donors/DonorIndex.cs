namespace Unite.Indices.Entities.Donors
{
    public class DonorIndex: DonorBaseIndex
    {
        public SampleIndex[] Samples { get; set; }
        public CellLineIndex[] CellLines { get; set; }
    }
}