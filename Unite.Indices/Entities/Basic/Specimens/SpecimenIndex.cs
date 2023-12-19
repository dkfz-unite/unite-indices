namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex
{
    /// <summary>
    /// Specific specimen Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Parent specimen Id.
    /// </summary> 
    public int? ParentId { get; set; }

    /// <summary>
    /// Specific specimen reference Id. Depends on specimen type. Should be set during indexing.
    /// </summary>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Parent specimen reference Id. Depends on specimen type. Should be set during indexing.
    /// </summary> 
    public string ParentReferenceId { get; set; }

    /// <summary>
    /// Type of the specimen. Should be set during indexing.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Type of the parent specimen. Should be set during indexing.
    /// </summary>
    public string ParentType { get; set; }
     

    public TissueIndex Tissue { get; set; }
    public CellLineIndex Cell { get; set; }
    public OrganoidIndex Organoid { get; set; }
    public XenograftIndex Xenograft { get; set; }


    public MolecularDataIndex GetMolecularData()
    {
        return Tissue?.MolecularData ?? 
               Cell?.MolecularData ?? 
               Organoid?.MolecularData ??
               Xenograft?.MolecularData;
    }

    public DrugScreeningIndex[] GetDrugScreenings()
    {
        return Cell?.DrugScreenings ??
               Organoid?.DrugScreenings ??
               Xenograft?.DrugScreenings;
    }

    public OrganoidInterventionIndex[] GetOrganoidInterventions()
    {
        return Organoid?.Interventions;
    }

    public XenograftInterventionIndex[] GetXenograftInterventions()
    {
        return Xenograft?.Interventions;
    }
}
