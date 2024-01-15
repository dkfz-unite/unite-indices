using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record XenograftCriteria : SpecimenBaseCriteria
{
    public string[] MouseStrain { get; set; }
    public Range<double?> SurvivalDays { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string[] TumorGrowthForm { get; set; }
}
