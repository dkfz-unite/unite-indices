﻿namespace Unite.Indices.Entities.Variants;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }

    public ImageIndex[] Images { get; set; }
    public SampleIndex[] Samples { get; set; }
}
