namespace Unite.Indices.Entities.Basic.Specimens;

public class XenograftIndex : SpecimenBaseIndex
{
    public string MouseStrain { get; set; }
    public int? GroupSize { get; set; }
    public string ImplantType { get; set; }
    public string TissueLocation { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string TumorGrowthForm { get; set; }
    public int? SurvivalDaysFrom { get; set; }
    public int? SurvivalDaysTo { get; set; }

    public XenograftInterventionIndex[] Interventions { get; set; }
}
