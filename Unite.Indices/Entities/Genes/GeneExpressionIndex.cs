using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Indices.Entities.Genes;

[SearchableDocument("gene_expressions")]
public class GeneExpressionIndex
{
    public string Id { get; set; }
    [References(typeof(Genes.GeneIndex))]
    public GeneNavIndex Gene { get; set; }
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenNavIndex Specimen { get; set; }
    
    [Searchable]
    public double Expression { get; set; }
}
