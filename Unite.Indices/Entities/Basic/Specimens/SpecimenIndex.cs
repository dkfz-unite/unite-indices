namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int? CreationDay { get; set; }

    public TissueIndex Tissue { get; set; }
    public CellLineIndex CellLine { get; set; }
    public OrganoidIndex Organoid { get; set; }
    public XenograftIndex Xenograft { get; set; }


    public DrugScreeningIndex[] GetDrugScreenings()
    {
        return Tissue != null ? null
             : CellLine != null ? CellLine.DrugScreenings
             : Organoid != null ? Organoid.DrugScreenings
             : Xenograft != null ? Xenograft.DrugScreenings
             : throw new NullReferenceException("Specific specimen type is not set");
    }
}
