using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public abstract record GeneBaseCriteria
{
    public int[] Id { get; set; }

    public string[] Symbol { get; set; }
    public string[] Biotype { get; set; }
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }

    public virtual bool IsNotEmpty()
    {
        return Id?.Any() == true
            || Symbol?.Any() == true
            || Biotype?.Any() == true
            || Chromosome?.Any() == true
            || Position?.From != null
            || Position?.To != null;
    }
}
