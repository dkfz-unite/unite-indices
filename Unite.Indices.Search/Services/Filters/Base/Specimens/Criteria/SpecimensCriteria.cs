namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimensCriteria
{
    public int[] Id { get; init; }
    public string[] ReferenceId { get; init; }
    public string[] SpecimenType { get; set; }

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
            || SpecimenType?.Length > 0
            || HasExp != null
            || HasExpSc != null
            || HasSms != null
            || HasCnvs != null
            || HasSvs != null
            || HasMeth != null;
    }
}
