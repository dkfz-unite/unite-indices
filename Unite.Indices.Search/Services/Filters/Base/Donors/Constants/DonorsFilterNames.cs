namespace Unite.Indices.Search.Services.Filters.Base.Donors.Constants;

public class DonorsFilterNames : DataFilterNames
{
    protected override string Prefix => "Donors";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
}
