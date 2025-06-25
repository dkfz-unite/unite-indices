using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GenesCriteria
{
    public ValuesCriteria<int> Id { get; set; }

    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.IsNotEmpty() == true
            || HasSms?.IsNotEmpty() == true
            || HasCnvs?.IsNotEmpty() == true
            || HasSvs?.IsNotEmpty() == true;
    }
}
