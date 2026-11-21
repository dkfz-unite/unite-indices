namespace Unite.Indices.Entities.Basic.Specimens;

public class XenograftIndex : SpecimenBaseIndex
{
    [Searchable]
    public string MouseStrain { get; set; }
    public int? GroupSize { get; set; }
    public string ImplantType { get; set; }
    public string ImplantLocation { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    [Searchable]
    public bool? Tumorigenicity { get; set; }
    [Searchable]
    public string TumorGrowthForm { get; set; }
    [Searchable]
    public int? SurvivalDaysFrom { get; set; }
    [Searchable]
    public int? SurvivalDaysTo { get; set; }
}
