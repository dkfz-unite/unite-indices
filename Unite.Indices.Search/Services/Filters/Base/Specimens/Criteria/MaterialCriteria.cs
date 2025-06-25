using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record MaterialCriteria : SpecimenCriteria
{
    public ValuesCriteria<string> Type { get; set; }
    public ValuesCriteria<string> FixationType { get; set; }
    public ValuesCriteria<string> TumorType { get; set; }
    public RangeCriteria<double?> TumorGrade { get; set; }
    public ValuesCriteria<string> Source { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.IsNotEmpty() == true
            || FixationType?.IsNotEmpty() == true
            || TumorType?.IsNotEmpty() == true
            || TumorGrade?.IsNotEmpty() == true
            || Source?.IsNotEmpty() == true;
    }
}
