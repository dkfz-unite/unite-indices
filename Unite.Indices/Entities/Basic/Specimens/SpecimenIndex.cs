namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex : SpecimenNavIndex
{
    [Searchable]
    public MaterialIndex Material { get; set; }
    [Searchable]
    public LineIndex Line { get; set; }
    [Searchable]
    public OrganoidIndex Organoid { get; set; }
    [Searchable]
    public XenograftIndex Xenograft { get; set; }


    public TumorClassificationIndex GetTumorClassification()
    {
        return Material?.TumorClassification ?? 
               Line?.TumorClassification ?? 
               Organoid?.TumorClassification ??
               Xenograft?.TumorClassification;
    }

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

    public Drugs.DrugScreeningIndex[] GetDrugScreenings()
    {
        return Line?.DrugScreenings ??
               Organoid?.DrugScreenings ??
               Xenograft?.DrugScreenings;
    }
}
