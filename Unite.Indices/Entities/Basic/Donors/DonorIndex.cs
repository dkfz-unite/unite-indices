using Unite.Indices.Entities.Basic.Clinical;
using Unite.Indices.Entities.Basic.Molecular;

namespace Unite.Indices.Entities.Basic.Donors
{
    public class DonorIndex
    {
        public int Id { get; set; }

        public bool? MtaProtected { get; set; }

        public string[] Pseudonyms { get; set; }

        public ClinicalDataIndex ClinicalData { get; set; }
        public MolecularDataIndex MolecularData { get; set; }
        public TreatmentIndex[] Treatments { get; set; }
        public WorkPackageIndex[] WorkPackages { get; set; }
        public StudyIndex[] Studies { get; set; }
    }
}
