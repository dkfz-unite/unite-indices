using Unite.Indices.Entities.Basic.Molecular;

namespace Unite.Indices.Entities.Basic.Specimens
{
    public class SpecimenIndex
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        
        public TissueIndex Tissue { get; set; }
        public CellLineIndex CellLine { get; set; }

        public MolecularDataIndex MolecularData { get; set; }
    }
}
