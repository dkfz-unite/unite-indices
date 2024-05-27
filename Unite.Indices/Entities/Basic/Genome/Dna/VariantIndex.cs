namespace Unite.Indices.Entities.Basic.Genome.Dna;

public class VariantIndex
{
    /// <summary>
    /// Specific variant Id. Depends on variant type. Should be set during indexing.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Type of the variant. Should be set during indexing (<see cref="Constants.VariantType"/>).
    /// </summary>
    public string Type { get ; set; }

    public SsmIndex Ssm { get; set; }
    public CnvIndex Cnv { get; set; }
    public SvIndex Sv { get; set; }


    public AffectedFeatureIndex[] GetAffectedFeatures()
    {
        return Ssm?.AffectedFeatures ??
               Cnv?.AffectedFeatures ??
               Sv?.AffectedFeatures;
    }
}
