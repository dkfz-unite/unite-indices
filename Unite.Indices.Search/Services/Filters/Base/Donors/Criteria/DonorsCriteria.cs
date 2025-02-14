namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorsCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public bool? HasExp { get; set; }
    public bool? HasExpSc { get; set; }
    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasMeth { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Length > 0
            || ReferenceId?.Length > 0
            || HasExp != null
            || HasExpSc != null
            || HasSsms != null
            || HasCnvs != null
            || HasSvs != null
            || HasMeth != null;
    }
}
