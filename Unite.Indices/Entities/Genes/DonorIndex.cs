namespace Unite.Indices.Entities.Genes
{
    public class DonorIndex : Basic.Donors.DonorIndex
    {
        public SpecimenIndex[] Specimens { get; set; }
    }
}
