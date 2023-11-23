namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    /// <summary>
    /// Specific variant Id. Depends on variant type. Should be set during indexing.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Type of the variant. Should be set during indexing.
    /// </summary>
    public string Type { get ; set; }

    public SsmIndex Ssm { get; set; }
    public CnvIndex Cnv { get; set; }
    public SvIndex Sv { get; set; }


    public AffectedFeatureIndex[] GetAffectedFeatures()
    {
        return Ssm != null ? Ssm.AffectedFeatures :
               Cnv != null ? Cnv.AffectedFeatures :
               Sv != null ? Sv.AffectedFeatures :
               throw new NullReferenceException("Specific variant is not set.");
    }
}
