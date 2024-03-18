namespace Unite.Indices.Entities.Projects;

public class VariantStatsIndex
{
    public int NumberOfSsms { get; set; }
    public Stat[] GenesAffectedBySsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public Stat[] GenesAffectedByCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public Stat[] GenesAffectedBySvs { get; set; }
}
