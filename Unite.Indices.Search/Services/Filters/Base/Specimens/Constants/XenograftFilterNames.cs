namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class XenograftFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Xenograft";

    public string MouseStrain => $"{Prefix}.MouseStrain";
    public string Tumorigenicity => $"{Prefix}.Tumorigenicity";
    public string TumorGrowthForm => $"{Prefix}.TumorGrowthForm";
    public string SurvivalDays => $"{Prefix}.SurvivalDays";
    public string Intervention => $"{Prefix}.Intervention";
}
