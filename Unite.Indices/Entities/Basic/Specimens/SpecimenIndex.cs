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
     

    public MaterialIndex Material { get; set; }
    public LineIndex Line { get; set; }
    public OrganoidIndex Organoid { get; set; }
    public XenograftIndex Xenograft { get; set; }


    public MolecularDataIndex GetMolecularData()
    {
        return Material?.MolecularData ?? 
               Line?.MolecularData ?? 
               Organoid?.MolecularData ??
               Xenograft?.MolecularData;
    }

    public InterventionIndex[] GetInterventions()
    {
        return Line?.Interventions ??
               Organoid?.Interventions ??
               Xenograft?.Interventions;
    }

    public DrugScreeningIndex[] GetDrugScreenings()
    {
        return Line?.DrugScreenings ??
               Organoid?.DrugScreenings ??
               Xenograft?.DrugScreenings;
    }
}
