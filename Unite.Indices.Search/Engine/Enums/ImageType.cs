using System.Runtime.Serialization;

namespace Unite.Indices.Search.Engine.Enums;

public enum ImageType
{
    [EnumMember(Value = "MRI")]
    MRI = 1,

    [EnumMember(Value = "CT")]
    CT = 2
}
