namespace Unite.Indices.Entities.Images
{
    public class ImageIndex : Basic.Images.ImageIndex
    {
        /// <summary>
        /// Donor of the image
        /// </summary>
        public DonorIndex Donor { get; set; }

        /// <summary>
        /// Image donor tissues
        /// </summary>
        public SpecimenIndex[] Specimens { get; set; }


        /// <summary>
        /// Total number of genes affected by any mutation across all tissues of the image donor
        /// </summary>
        public int NumberOfGenes { get; set; }

        /// <summary>
        /// Total number of mutations across all tissues of the image donor
        /// </summary>
        public int NumberOfMutations { get; set; }
    }
}
