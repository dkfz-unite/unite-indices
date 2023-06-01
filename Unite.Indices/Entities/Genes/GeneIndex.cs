using Unite.Indices.Entities.Basic.Genome.Variants.Constants;
using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMRIs;
    private int? _numberOfCTs;
    private int? _numberOfTissues;
    private int? _numberOfCells;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfSSMs;
    private int? _numberOfCNVs;
    private int? _numberOfSVs;

    private GeneExpressionStatsIndex _reads;
    private GeneExpressionStatsIndex _tpm;
    private GeneExpressionStatsIndex _fpkm;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Samples); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRI images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMRIs { get => _numberOfMRIs ?? GetNumberOfMRIs(Samples); set => _numberOfMRIs = value; }

    /// <summary>
    /// Number of CT images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCTs { get => _numberOfCTs ?? GetNumberOfCTs(Samples); set => _numberOfCTs = value; }

    /// <summary>
    /// Number of tissues with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfTissues(Samples); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfCells(Samples); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfOrganoids(Samples); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfXenografts(Samples); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int NumberOfSSMs { get => _numberOfSSMs ?? GetNumberOfSSMs(Samples); set => _numberOfSSMs = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCNVs { get => _numberOfCNVs ?? GetNumberOfCNVs(Samples); set => _numberOfCNVs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSVs { get => _numberOfSVs ?? GetNumberOfSVs(Samples); set => _numberOfSVs = value; }


    /// <summary>
    /// Raw expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex Reads { get => _reads ?? GetExpression(Samples, index => index.Reads); set => _reads = value; }

    /// <summary>
    /// TPM expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex TPM { get => _reads ?? GetExpression(Samples, index => index.TPM); set => _reads = value; }

    /// <summary>
    /// FPKM expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex FPKM { get => _reads ?? GetExpression(Samples, index => index.FPKM); set => _reads = value; }


    public SampleIndex[] Samples { get; set; }


    public static int GetNumberOfDonors(SampleIndex[] samples)
    {
        return samples?
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfMRIs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCTs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.CT)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfTissues(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Tissue)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCells(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Cell)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfOrganoids(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Organoid)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfXenografts(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Xenograft)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSSMs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.SSM)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCNVs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.CNV)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSVs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.SV)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static GeneExpressionStatsIndex GetExpression(SampleIndex[] samples, Func<GeneExpressionIndex, double> getter)
    {
        var expressions = samples?
            .Where(sample => sample.Expression != null)
            .Select(sample => getter(sample.Expression))
            .OrderBy(expression => expression)
            .ToArray();

        if (expressions?.Any() == true)
        {
            var index = new GeneExpressionStatsIndex();

            index.Min = Math.Round(expressions.First());
            index.Max = Math.Round(expressions.Last());
            index.Mean = Math.Round(expressions.Average());
            index.Median = Math.Round(expressions[expressions.Length / 2]);

            return index;
        }
        else
        {
            return null;
        }
    }
}
