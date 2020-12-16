namespace Unite.Indices.Entities.Mutations
{
    public class SampleIndex : SampleBaseIndex
    {
        public DonorIndex Donor { get; set; }
        public CellLineIndex CellLine { get; set; }
    }
}
