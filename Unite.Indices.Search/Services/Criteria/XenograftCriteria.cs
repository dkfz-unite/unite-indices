﻿using Unite.Indices.Search.Services.Criteria.Models;

namespace Unite.Indices.Search.Services.Criteria;

public record XenograftCriteria : SpecimenCriteriaBase
{
    public string[] MouseStrain { get; set; }
    public string[] Intervention { get; set; }
    public Range<double?> SurvivalDays { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string[] TumorGrowthForm { get; set; }
}
