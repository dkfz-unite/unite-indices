namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record OrganoidCriteria : SpecimenCriteria
{
    public string[] Medium { get; set; }
    public bool? Tumorigenicity { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Medium?.Length > 0
            || Tumorigenicity != null;
    }
}
