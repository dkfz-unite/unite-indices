namespace Unite.Indices.Entities.Mutations
{
    public class MutationIndex : Basic.Mutations.MutationIndex
    {
        public DonorIndex[] Donors { get; set; }

        /// <summary>
        /// Number of affected donors
        /// </summary>
        public int NumberOfDonors { get; set; }

        /// <summary>
        /// Number of affected specimens
        /// </summary>
        public int NumberOfSpecimens { get; set; }
    }
}
