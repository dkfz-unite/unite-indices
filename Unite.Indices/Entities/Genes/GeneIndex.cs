using Unite.Indices.Entities.Basic.Genome.Variants.Constants;
using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Genes;

public record ExpressionStats(double Min, double Max, double Avg, double Median);

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

    private ExpressionStats _reads;
    private ExpressionStats _tpm;
    private ExpressionStats _fpkm;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRI images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMRIs { get => _numberOfMRIs ?? GetNumberOfMRIs(); set => _numberOfMRIs = value; }

    /// <summary>
    /// Number of CT images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCTs { get => _numberOfCTs ?? GetNumberOfCTs(); set => _numberOfCTs = value; }

    /// <summary>
    /// Number of tissues with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfTissues(); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfCells(); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfOrganoids(); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfXenografts(); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int NumberOfSSMs { get => _numberOfSSMs ?? GetNumberOfSSMs(); set => _numberOfSSMs = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCNVs { get => _numberOfCNVs ?? GetNumberOfCNVs(); set => _numberOfCNVs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSVs { get => _numberOfSVs ?? GetNumberOfSVs(); set => _numberOfSVs = value; }


    /// <summary>
    /// Raw expression stats for this gene.
    /// </summary>
    public ExpressionStats Reads { get => _reads ?? GetExpression(index => index.Reads); set => _reads = value; }

    /// <summary>
    /// TPM expression stats for this gene.
    /// </summary>
    public ExpressionStats TPM { get => _reads ?? GetExpression(index => index.TPM); set => _reads = value; }

    /// <summary>
    /// FPKM expression stats for this gene.
    /// </summary>
    public ExpressionStats FPKM { get => _reads ?? GetExpression(index => index.FPKM); set => _reads = value; }


    public SampleIndex[] Samples { get; set; }


    private int GetNumberOfDonors()
    {
        return Samples?
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfMRIs()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCTs()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.CT)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfTissues()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Tissue)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCells()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Cell)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfOrganoids()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Organoid)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfXenografts()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Xenograft)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfSSMs()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.SSM)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCNVs()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.CNV)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfSVs()
    {
        return Samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.SV)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private ExpressionStats GetExpression(Func<GeneExpressionIndex, double> getter)
    {
        var expressions = Samples?
            .Where(sample => sample.Expression != null)
            .Select(sample => getter(sample.Expression))
            .OrderBy(expression => expression)
            .ToArray();

        if (expressions?.Any() == true)
        {
            var min = Math.Round(expressions.First());
            var max = Math.Round(expressions.Last());
            var avg = Math.Round(expressions.Average());
            var med = Math.Round(expressions[expressions.Length / 2]);

            return new(min, max, avg, med);
        }
        else
        {
            return null;
        }
    }
}
