﻿namespace Unite.Indices.Search.Services.Criteria;

public record OrganoidCriteria : SpecimenCriteriaBase
{
    public string[] Medium { get; set; }
    public string[] Intervention { get; set; }
    public bool? Tumorigenicity { get; set; }
}
