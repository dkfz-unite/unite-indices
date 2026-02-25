namespace Unite.Indices.Entities.CnvProfiles;

public class CnvProfileIndex: CnvProfileNavIndex
{
    public string Chromosome { get; set; }
    public string ChromosomeArm { get; set; }
    public float Gain { get; set; }
    public float Loss { get; set; }
    public float Neutral { get; set; }

    public Basic.Specimens.SpecimenNavIndex Specimen { get; set; }
}