namespace Unite.Indices.Search.Services.Filters.Base.Donors.Constants;

public class DonorFilterNames
{
    protected string Prefix => "Donor";
    
    public string ReferenceId => $"{Prefix}.ReferenceId";
    public string Diagnosis => $"{Prefix}.Diagnosis";
    public string PrimarySite => $"{Prefix}.PrimarySite";
    public string Localization => $"{Prefix}.Localization";
    public string Sex => $"{Prefix}.Sex";
    public string Age => $"{Prefix}.Age";
    public string VitalStatus => $"{Prefix}.VitalStatus";
    public string VitalStatusChangeDay => $"{Prefix}.VitalStatusChangeDay";
    public string ProgressionStatus => $"{Prefix}.ProgressionStatus";
    public string ProgressionStatusChangeDay => $"{Prefix}.ProgressionStatusChangeDay";
    public string Therapy => $"{Prefix}.Therapy";

    public string MtaProtected => $"{Prefix}.MtaProtected";
    public string Project => $"{Prefix}.Project";
    public string Study => $"{Prefix}.Study";
}
