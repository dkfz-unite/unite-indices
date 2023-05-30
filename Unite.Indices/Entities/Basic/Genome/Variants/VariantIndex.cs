using Unite.Indices.Entities.Basic.Genome.Variants.Constants;

namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    private string _id;
    private string _type;


    public string Id { get => _id ?? GetVariantId(); set => _id = value; }
    public string Type { get => _type ?? GetVariantType(); set => _type = value; }

    public MutationIndex SSM { get; set; }
    public CopyNumberVariantIndex CNV { get; set; }
    public StructuralVariantIndex SV { get; set; }


    public AffectedFeatureIndex[] GetAffectedFeatures()
    {
        return SSM != null ? SSM.AffectedFeatures :
               CNV != null ? CNV.AffectedFeatures :
               SV != null ? SV.AffectedFeatures :
               throw new NullReferenceException("Specific variant is not set.");
    }

    private string GetVariantId()
    {
        return SSM != null ? $"{VariantTypes.SSM}{SSM.Id}" :
               CNV != null ? $"{VariantTypes.CNV}{CNV.Id}" :
               SV != null ? $"{VariantTypes.SV}{SV.Id}" :
               throw new NullReferenceException("Specific variant is not set.");
    }

    private string GetVariantType()
    {
        return SSM != null ? VariantTypes.SSM :
               CNV != null ? VariantTypes.CNV :
               SV != null ? VariantTypes.SV :
               throw new NullReferenceException("Specific variant is not set.");
    }
}
