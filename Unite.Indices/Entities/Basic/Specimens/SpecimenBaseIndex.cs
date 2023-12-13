namespace Unite.Indices.Entities.Basic.Specimens;

public abstract class SpecimenBaseIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }

    public MolecularDataIndex MolecularData { get; set; }
    public DrugScreeningIndex[] DrugScreenings { get; set; }
}
