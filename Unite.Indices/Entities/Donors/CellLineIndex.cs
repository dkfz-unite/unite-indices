namespace Unite.Indices.Entities.Donors
{
    public class CellLineIndex : CellLineBaseIndex
    {
		public CellLineBaseIndex Parent { get; set; }
		public CellLineBaseIndex[] Children { get; set; }
		public SampleIndex[] Samples { get; set; }
	}
}
