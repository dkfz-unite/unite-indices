using System.Runtime.Serialization;

namespace Unite.Indices.Search.Engine.Enums;

public enum VariantType
{
    [EnumMember(Value = "SSM")]
    SSM = 1,

    [EnumMember(Value = "CNV")]
    CNV = 2,

    [EnumMember(Value = "SV")]
    SV = 3
}
