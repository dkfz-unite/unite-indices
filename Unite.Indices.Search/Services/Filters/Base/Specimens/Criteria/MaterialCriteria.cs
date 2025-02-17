namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record MaterialCriteria : SpecimenCriteria
{
    public string[] Type { get; set; }
    public string[] TumorType { get; set; }
    public string[] Source { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Length > 0
            || TumorType?.Length > 0
            || Source?.Length > 0;
    }
}
