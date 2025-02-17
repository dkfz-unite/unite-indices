using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record XenograftCriteria : SpecimenCriteria
{
    public string[] MouseStrain { get; set; }
    public Range<double?> SurvivalDays { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string[] TumorGrowthForm { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || MouseStrain?.Length > 0
            || SurvivalDays.From != null
            || SurvivalDays.To != null
            || Tumorigenicity != null
            || TumorGrowthForm?.Length > 0;
    }
}
