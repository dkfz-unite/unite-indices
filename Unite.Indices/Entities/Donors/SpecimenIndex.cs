﻿namespace Unite.Indices.Entities.Donors;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public AnalysisIndex[] Analyses { get; set; }
}
