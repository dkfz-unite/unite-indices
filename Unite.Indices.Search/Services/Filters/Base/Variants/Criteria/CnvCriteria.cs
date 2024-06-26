﻿namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record CnvCriteria : VariantBaseCriteria
{
    public string[] Type { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Any() == true
            || Loh != null
            || Del != null;
    }
}
