using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Indices.Entities.Proteins;

[SearchableDocument("protein_expressions")]
public class ProteinExpressionIndex
{
    public string Id { get; set; }
    [References(typeof(Proteins.ProteinIndex))]
    public ProteinNavIndex Protein { get; set; }
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenNavIndex Specimen { get; set; }

    [Searchable]
    public double Expression { get; set; }
}
