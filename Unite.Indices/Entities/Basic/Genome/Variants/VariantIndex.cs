using Unite.Indices.Entities.Basic.Genome.Variants.Constants;

namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    private string _id;
    private string _type;


    public string Id { get => _id ?? GetVariantId(); set => _id = value; }
    public string Type { get => _type ?? GetVariantType(); set => _type = value; }

    public MutationIndex Ssm { get; set; }
    public CopyNumberVariantIndex Cnv { get; set; }
    public StructuralVariantIndex Sv { get; set; }


    public AffectedFeatureIndex[] GetAffectedFeatures()
    {
        return Ssm != null ? Ssm.AffectedFeatures :
               Cnv != null ? Cnv.AffectedFeatures :
               Sv != null ? Sv.AffectedFeatures :
               throw new NullReferenceException("Specific variant is not set.");
    }

    private string GetVariantId()
    {
        return Ssm != null ? $"{VariantTypes.SSM}{Ssm.Id}" :
               Cnv != null ? $"{VariantTypes.CNV}{Cnv.Id}" :
               Sv != null ? $"{VariantTypes.SV}{Sv.Id}" :
               throw new NullReferenceException("Specific variant is not set.");
    }

    private string GetVariantType()
    {
        return Ssm != null ? VariantTypes.SSM :
               Cnv != null ? VariantTypes.CNV :
               Sv != null ? VariantTypes.SV :
               throw new NullReferenceException("Specific variant is not set.");
    }
}
