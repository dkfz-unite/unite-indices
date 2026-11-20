namespace Unite.Indices.Entities.CnvProfiles;

[SearchableDocument("cnv-profiles")]
public class CnvProfileIndex: CnvProfileNavIndex
{
    [Searchable]
    public string Chromosome { get; set; }
    [Searchable]
    public string ChromosomeArm { get; set; }
    [Searchable]
    public float Gain { get; set; }
    [Searchable]
    public float Loss { get; set; }
    [Searchable]
    public float Neutral { get; set; }

    [References(typeof(Specimens.SpecimenIndex))]
    public Basic.Specimens.SpecimenNavIndex Specimen { get; set; }
}