namespace Unite.Indices.Entities;

/// <summary>
/// Shows which data is available for given entity.
/// TODO: this information and it's structure has to be completely revised.
/// </summary>
public class DataIndex
{
    public virtual bool? Donors { get; set; }
    public virtual bool? Clinical { get; set; }
    public virtual bool? Treatments { get; set; }
    public virtual bool? Mris { get; set; }
    public virtual bool? Cts { get; set; }
    public virtual bool? Materials { get; set; }
    public virtual bool? MaterialsMolecular { get; set; }
    public virtual bool? Lines { get; set; }
    public virtual bool? LinesMolecular { get; set; }
    public virtual bool? LinesDrugs { get; set; }
    public virtual bool? LinesInterventions { get; set; }
    public virtual bool? Organoids { get; set; }
    public virtual bool? OrganoidsMolecular { get; set; }
    public virtual bool? OrganoidsDrugs { get; set; }
    public virtual bool? OrganoidsInterventions { get; set; }
    public virtual bool? Xenografts { get; set; }
    public virtual bool? XenograftsMolecular { get; set; }
    public virtual bool? XenograftsDrugs { get; set; }
    public virtual bool? XenograftsInterventions { get; set; }
    public virtual bool? GeneExp { get; set; }
    public virtual bool? GeneExpSc { get; set; }
    public virtual bool? Ssms { get; set; }
    public virtual bool? Cnvs { get; set; }
    public virtual bool? Svs { get; set; }
}
