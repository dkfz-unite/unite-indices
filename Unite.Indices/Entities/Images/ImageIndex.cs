﻿namespace Unite.Indices.Entities.Images;

public class ImageIndex : Basic.Images.ImageIndex
{
    public DonorIndex Donor { get; set; }

    public SpecimenIndex[] Specimens { get; set; }


    /// <summary>
    /// Total number of genes affected by any variant across all tissues of the image donor
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Total number of mutations across all tissues of the image donor
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Total number of copy number variants across all tissues of the image donor
    /// </summary>
    public int NumberOfCopyNumberVariants { get; set; }

    /// <summary>
    /// Total number of structural variants across all tissues of the image donor
    /// </summary>
    public int NumberOfStructuralVariants { get; set; }
}
