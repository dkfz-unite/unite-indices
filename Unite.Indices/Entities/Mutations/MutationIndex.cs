namespace Unite.Indices.Entities.Mutations
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public DonorIndex[] Donors { get; set; }

        public SampleIndex[] Samples { get; set; }
        // public CellLineIndex[] CellLines { get; set; };
        // public XenograftIndex[] Xenografts { get; set; };
    }
}
