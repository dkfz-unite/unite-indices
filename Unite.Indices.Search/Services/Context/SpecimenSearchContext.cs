
using Unite.Indices.Search.Engine.Enums;

namespace Unite.Indices.Search.Services.Context;

public class SpecimenSearchContext
{
    public SpecimenType? SpecimenType { get; set; }


    public SpecimenSearchContext()
    {
    }

    public SpecimenSearchContext(SpecimenType specimenType) : this()
    {
        SpecimenType = specimenType;
    }
}
