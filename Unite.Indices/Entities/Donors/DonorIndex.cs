namespace Unite.Indices.Entities.Donors
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public MutationIndex[] Mutations { get; set; }

        /// <summary>
        /// Number of donor specimens
        /// </summary>
        public int NumberOfSpecimens { get; set; }

        /// <summary>
        /// Number of donor mutations
        /// </summary>
        public int NumberOfMutations { get; set; }

        /// <summary>
        /// Number of expressed genes
        /// </summary>
        public int NumberOfGenes { get; set; }
    }
}