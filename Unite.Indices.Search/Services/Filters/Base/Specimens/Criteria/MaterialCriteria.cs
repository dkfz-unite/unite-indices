using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record MaterialCriteria : SpecimenCriteria
{
    public string[] Type { get; set; }
    public string[] FixationType { get; set; }
    public string[] TumorType { get; set; }
    public Range<double?> TumorGrade { get; set; }
    public string[] Source { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Length > 0
            || FixationType?.Length > 0
            || TumorType?.Length > 0
            || TumorGrade?.From != null
            || TumorGrade?.To != null
            || Source?.Length > 0;
    }
}
