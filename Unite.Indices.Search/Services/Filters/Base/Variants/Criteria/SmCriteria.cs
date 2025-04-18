﻿namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record SmCriteria : VariantCriteria
{
    public string[] Type { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Length > 0;
    }
}
