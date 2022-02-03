namespace Unite.Indices.Entities.Images
{
    public class ImageIndex : Basic.Images.ImageIndex
    {
        public DonorIndex Donor { get; set; }

        public SpecimenIndex[] Specimens { get; set; }
    }
}
