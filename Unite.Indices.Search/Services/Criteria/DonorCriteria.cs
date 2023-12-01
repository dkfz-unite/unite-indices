﻿using Unite.Indices.Search.Services.Criteria.Models;

namespace Unite.Indices.Search.Services.Criteria;

public record DonorCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public string[] Gender { get; set; }
    public Range<double?> Age { get; set; }
    public string[] Diagnosis { get; set; }
    public string[] PrimarySite { get; set; }
    public string[] Localization { get; set; }
    public bool? VitalStatus { get; set; }
    public Range<double?> VitalStatusChangeDay { get; set; }
    public bool? ProgressionStatus { get; set; }
    public Range<double?> ProgressionStatusChangeDay { get; set; }

    public string[] Therapy { get; set; }

    public bool? MtaProtected { get; set; }
    public string[] Project { get; set; }
    public string[] Study { get; set; }

    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasGeneExp { get; set; }
}
