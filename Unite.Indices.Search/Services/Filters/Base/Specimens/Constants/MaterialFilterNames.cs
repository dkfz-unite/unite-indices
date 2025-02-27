namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class MaterialFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Material";

    public string Type => $"{Prefix}.Type";
    public string FixationType => $"{Prefix}.FixationType";
    public string TumorType => $"{Prefix}.TumorType";
    public string TumorGrade => $"{Prefix}.TumorGrade";
    public string Source => $"{Prefix}.Source";
}
