namespace Unite.Indices.Entities.Basic.Images;

public class ImageIndex : ImageNavIndex
{   
    public MrImageIndex Mr { get; set; }


    public Radiomics.FeatureEntryIndex[] GetRadiomicsFeatures()
    {
        return Mr?.RadiomicsFeatures;
    }
}
