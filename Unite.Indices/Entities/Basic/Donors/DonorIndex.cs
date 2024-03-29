﻿namespace Unite.Indices.Entities.Basic.Donors;

public class DonorIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public bool? MtaProtected { get; set; }

    public ClinicalDataIndex ClinicalData { get; set; }
    public TreatmentIndex[] Treatments { get; set; }
    public ProjectIndex[] Projects { get; set; }
    public StudyIndex[] Studies { get; set; }
}
