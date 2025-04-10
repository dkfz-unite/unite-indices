namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record ImagesCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }
    public string[] ImageType { get; set; }

    public bool? HasExp { get; set; }
    public bool? HasExpSc { get; set; }
    public bool? HasSms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasMeth { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Length > 0
            || ReferenceId?.Length > 0
            || ImageType?.Length > 0
            || HasExp != null
            || HasExpSc != null
            || HasSms != null
            || HasCnvs != null
            || HasSvs != null
            || HasMeth != null;
    }
}
