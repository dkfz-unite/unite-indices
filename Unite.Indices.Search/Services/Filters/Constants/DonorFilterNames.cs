﻿namespace Unite.Indices.Search.Services.Filters.Constants;

public static class DonorFilterNames
{
    private const string _prefix = "Donor";

    public static readonly string Id = $"{_prefix}.Id";
    public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
    public static readonly string Diagnosis = $"{_prefix}.Diagnosis";
    public static readonly string PrimarySite = $"{_prefix}.PrimarySite";
    public static readonly string Localization = $"{_prefix}.Localization";
    public static readonly string Gender = $"{_prefix}.Gender";
    public static readonly string Age = $"{_prefix}.Age";
    public static readonly string VitalStatus = $"{_prefix}.VitalStatus";
    public static readonly string VitalStatusChangeDay = $"{_prefix}.VitalStatusChangeDay";
    public static readonly string ProgressionStatus = $"{_prefix}.ProgressionStatus";
    public static readonly string ProgressionStatusChangeDay = $"{_prefix}.ProgressionStatusChangeDay";
    public static readonly string Therapy = $"{_prefix}.Therapy";

    public static readonly string MtaProtected = $"{_prefix}.MtaProtected";
    public static readonly string Project = $"{_prefix}.Project";
    public static readonly string Study = $"{_prefix}.Study";

    public static readonly string HasSsms = $"{_prefix}.HasSsms";
    public static readonly string HasCnvs = $"{_prefix}.HasCnvs";
    public static readonly string HasSvs = $"{_prefix}.HasSvs";
    public static readonly string HasGeneExp = $"{_prefix}.HasGeneExp";
}
