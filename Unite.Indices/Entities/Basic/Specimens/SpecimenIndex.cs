namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenIndex : SpecimenNavIndex
{
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

    public Drugs.DrugScreeningIndex[] GetDrugScreenings()
    {
        return Line?.DrugScreenings ??
               Organoid?.DrugScreenings ??
               Xenograft?.DrugScreenings;
    }
}
