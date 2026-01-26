using Unite.Indices.Entities.Basic.Specimens.Drugs;

namespace Unite.Indices.Entities.Basic.Specimens;

public abstract class SpecimenBaseIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    public string Category { get; set; }
    public string TumorType { get; set; }
    public double? TumorGrade { get; set; }

    public TumorClassificationIndex TumorClassification { get; set; }
    public MolecularDataIndex MolecularData { get; set; }
    public InterventionIndex[] Interventions { get; set; }
    public DrugScreeningIndex[] DrugScreenings { get; set; }
}
