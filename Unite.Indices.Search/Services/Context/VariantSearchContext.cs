
using Unite.Indices.Search.Engine.Enums;

namespace Unite.Indices.Search.Services.Context;

public class VariantSearchContext
{
    public VariantType? VariantType { get; set; }


    public VariantSearchContext()
    {
    }

    public VariantSearchContext(VariantType variantType) : this()
    {
        VariantType = variantType;
    }
}
