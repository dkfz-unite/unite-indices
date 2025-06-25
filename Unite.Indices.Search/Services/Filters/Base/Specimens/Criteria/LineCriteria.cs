using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record LineCriteria : SpecimenCriteria
{
    public ValuesCriteria<string> CellsSpecies { get; set; }
    public ValuesCriteria<string> CellsType { get; set; }
    public ValuesCriteria<string> CultureType { get; set; }

    public ValuesCriteria<string> Name { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || CellsSpecies?.IsNotEmpty() == true
            || CellsType?.IsNotEmpty() == true
            || CultureType?.IsNotEmpty() == true
            || Name?.IsNotEmpty() == true;
    }
}
