using Unite.Indices.Entities.Basic.Specimens.Drugs;

namespace Unite.Indices.Entities.Basic.Specimens;

public abstract class SpecimenBaseIndex
{
    [Searchable]
    public int Id { get; set; }
    [Searchable]
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
    [Searchable]
    public string Category { get; set; }
    [Searchable]
    public string TumorType { get; set; }
    [Searchable]
    public double? TumorGrade { get; set; }

    [Searchable]
    public TumorClassificationIndex TumorClassification { get; set; }
    [Searchable]
    public MolecularDataIndex MolecularData { get; set; }
    [Searchable]
    public InterventionIndex[] Interventions { get; set; }
    [Searchable]
    public DrugScreeningIndex[] DrugScreenings { get; set; }
}
