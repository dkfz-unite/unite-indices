using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex
{
    private string _referenceId;
    private string _type;

    public int Id { get; set; }
    public string ReferenceId { get => _referenceId ?? GetSpecimenReferenceId(); set => _referenceId = value; }
    public string Type { get => _type ?? GetSpecimenTypeType(); set => _type = value; }
    public int? ParentId { get; set; }
    public string ParentReferenceId { get; set; }
    public string ParentType { get; set; }
    public int DonorId { get; set; }
    public string DonorReferenceId { get; set; }
    public int? CreationDay { get; set; }
    


    public TissueIndex Tissue { get; set; }
    public CellLineIndex Cell { get; set; }
    public OrganoidIndex Organoid { get; set; }
    public XenograftIndex Xenograft { get; set; }


    public DrugScreeningIndex[] GetDrugScreenings()
    {
        return Tissue != null ? null : 
               Cell != null ? Cell.DrugScreenings : 
               Organoid != null ? Organoid.DrugScreenings : 
               Xenograft != null ? Xenograft.DrugScreenings : 
               throw new NullReferenceException("Specific specimen type is not set.");
    }


    private string GetSpecimenReferenceId()
    {
        return Tissue != null ? $"{Tissue.ReferenceId}" : 
               Cell != null ? $"{Cell.ReferenceId}" :
               Organoid != null ? $"{Organoid.ReferenceId}" :
               Xenograft != null ? $"{Xenograft.ReferenceId}" :
               throw new NullReferenceException("Specific specimen type is not set.");
    }

    private string GetSpecimenTypeType()
    {
        return Tissue != null ? $"{SpecimenTypes.Tissue}" :
               Cell != null ? $"{SpecimenTypes.Cell}" :
               Organoid != null ? $"{SpecimenTypes.Organoid}" :
               Xenograft != null ? $"{SpecimenTypes.Xenograft}" :
               throw new NullReferenceException("Specific specimen type is not set.");
    }
}
