namespace Unite.Indices.Entities.Basic.Images
{
    public class ImageIndex
    {
        public int Id { get; set; }

        public int? ScanningDay { get; set; }

        public MriImageIndex MriImage { get; set; }
    }
}
