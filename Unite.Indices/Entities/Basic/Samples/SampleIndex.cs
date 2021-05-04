using Unite.Indices.Entities.Basic.Clinical;
using Unite.Indices.Entities.Basic.Molecular;

namespace Unite.Indices.Entities.Basic.Samples
{
    public class SampleIndex
    {
        public int Id { get; set; }

        public TissueIndex Tissue { get; set; }
        public CellLineIndex CellLine { get; set; }

        public ClinicalDataIndex ClinicalData { get; set; }
        public MolecularDataIndex MolecularData { get; set; }
        public TreatmentIndex[] Treatments { get; set; }
    }
}
