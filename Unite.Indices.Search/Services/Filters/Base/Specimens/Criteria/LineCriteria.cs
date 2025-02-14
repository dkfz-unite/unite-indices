namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record LineCriteria : SpecimenCriteria
{
    public string[] CellsSpecies { get; set; }
    public string[] CellsType { get; set; }
    public string[] CultureType { get; set; }

    public string[] Name { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || CellsSpecies?.Length > 0
            || CellsType?.Length > 0
            || CultureType?.Length > 0
            || Name?.Length > 0;
    }
}
