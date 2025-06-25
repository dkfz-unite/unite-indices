using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record XenograftCriteria : SpecimenCriteria
{
    public ValuesCriteria<string> MouseStrain { get; set; }
    public RangeCriteria<double?> SurvivalDays { get; set; }
    public BoolCriteria Tumorigenicity { get; set; }
    public ValuesCriteria<string> TumorGrowthForm { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || MouseStrain?.IsNotEmpty() == true
            || SurvivalDays?.IsNotEmpty() == true
            || Tumorigenicity?.IsNotEmpty() == true
            || TumorGrowthForm?.IsNotEmpty() == true;
    }
}
