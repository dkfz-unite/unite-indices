namespace Unite.Indices.Entities.Mutations
{
    public class CellLineIndex : CellLineBaseIndex
    {
		public CellLineBaseIndex Parent { get; set; }
		public CellLineBaseIndex[] Children { get; set; }
	}
}
