namespace Unite.Indices.Entities.Basic.Donors;

public class ClinicalDataIndex
{
    [Searchable]
    public string Sex { get; set; }
    [Searchable]
    public int? Age { get; set; }
    [Searchable]
    public string Diagnosis { get; set; }
    [Searchable]
    public string PrimarySite { get; set; }
    [Searchable]
    public string Localization { get; set; }
    [Searchable]
    public bool? VitalStatus { get; set; }
    [Searchable]
    public int? VitalStatusChangeDay { get; set; }
    [Searchable]
    public bool? ProgressionStatus { get; set; }
    [Searchable]
    public int? ProgressionStatusChangeDay { get; set; }
    public bool? SteroidsReactive { get; set; }
    public int? Kps { get; set; }
}
