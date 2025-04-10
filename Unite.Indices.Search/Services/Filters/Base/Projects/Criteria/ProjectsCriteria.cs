namespace Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;

public class ProjectsCriteria
{
    public int[] Id { get; set; }
    public string[] Name { get; set; }

    public bool? HasExp { get; set; }
    public bool? HasExpSc { get; set; }
    public bool? HasSms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasMeth { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Length > 0
            || Name?.Length > 0
            || HasExp != null
            || HasExpSc != null
            || HasSms != null
            || HasCnvs != null
            || HasSvs != null
            || HasMeth != null;
    }
}
