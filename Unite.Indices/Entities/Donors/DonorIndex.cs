namespace Unite.Indices.Entities.Donors
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public MutationIndex[] Mutations { get; set; }

        public SampleIndex[] Samples { get; set; }
        // public CellLineIndex[] CellLines { get; set; };
        // public XenograftIndex[] Xenografts { get; set; };
    }
}