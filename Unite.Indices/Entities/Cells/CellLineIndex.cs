namespace Unite.Indices.Entities.Cells
{
    public class CellLineIndex : CellLineBaseIndex
    {
		public DonorIndex Donor { get; set; }
		public CellLineBaseIndex Parent { get; set; }
		public CellLineBaseIndex[] Children { get; set; }
		public SampleIndex[] Samples { get; set; }
	}
}
