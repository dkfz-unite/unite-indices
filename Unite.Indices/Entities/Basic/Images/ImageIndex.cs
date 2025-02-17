namespace Unite.Indices.Entities.Basic.Images;

public class ImageIndex : ImageNavIndex
{   
    public MriImageIndex Mri { get; set; }


    public Radiomics.FeatureEntryIndex[] GetRadiomicsFeatures()
    {
        return Mri?.RadiomicsFeatures;
    }
}
