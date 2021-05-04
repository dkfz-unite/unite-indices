namespace Unite.Indices.Entities.Donors
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public MutationIndex[] Mutations { get; set; }

        public SampleIndex[] Samples { get; set; }

        /// <summary>
        /// Number of donor mutations
        /// </summary>
        public int NumberOfMutations { get; set; }

        /// <summary>
        /// Number of expressed genes
        /// </summary>
        public int NumberOfGenes { get; set; }

        /// <summary>
        /// Number of donor derived tissue samples
        /// </summary>
        public int NumberOfTissues { get; set; }

        /// <summary>
        /// Number of all donor derived cell lines
        /// </summary>
        public int NumberOfCells { get; set; }
    }
}