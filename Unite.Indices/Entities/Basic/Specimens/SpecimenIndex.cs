namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex
{
    /// <summary>
    /// Specific specimen Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Specific specimen reference Id. Depends on specimen type. Should be set during indexing.
    /// </summary>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Type of the specimen. Should be set during indexing.
    /// </summary>
    public string Type { get; set; }
    
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
}
