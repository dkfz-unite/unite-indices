using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Indices.Entities.Genes;

public class GeneExpressionIndex
{
    public string Id { get; set; }
    public GeneNavIndex Gene { get; set; }
    public SpecimenNavIndex Specimen { get; set; }
    
    public double Expression { get; set; }
}
