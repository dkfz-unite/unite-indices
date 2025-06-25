using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorsCriteria
{
    public ValuesCriteria<int> Id { get; set; }
    public ValuesCriteria<string> ReferenceId { get; set; }

    public BoolCriteria HasExp { get; set; }
    public BoolCriteria HasExpSc { get; set; }
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
    public BoolCriteria HasMeth { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.IsNotEmpty() == true
            || ReferenceId?.IsNotEmpty() == true
            || HasExp?.IsNotEmpty() == true
            || HasExpSc?.IsNotEmpty() == true
            || HasSms?.IsNotEmpty() == true
            || HasCnvs?.IsNotEmpty() == true
            || HasSvs?.IsNotEmpty() == true
            || HasMeth?.IsNotEmpty() == true;
    }
}
