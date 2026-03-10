using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Indices.Entities.Proteins;

public class ProteinExpressionIndex
{
    public string Id { get; set; }
    public ProteinNavIndex Protein { get; set; }
    public SpecimenNavIndex Specimen { get; set; }

    public double Expression { get; set; }
}
