using Unite.Indices.Entities.Basic.Genome.Variants.Constants;
using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMris;
    private int? _numberOfCts;
    private int? _numberOfTissues;
    private int? _numberOfCells;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfSsms;
    private int? _numberOfCnvs;
    private int? _numberOfSvs;

    private BulkExpressionStatsIndex _reads;
    private BulkExpressionStatsIndex _tpm;
    private BulkExpressionStatsIndex _fpkm;

    private DataIndex _data;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Samples); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRI images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfImages(Samples, ImageTypes.MRI); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CT images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfImages(Samples, ImageTypes.CT); set => _numberOfCts = value; }

    /// <summary>
    /// Number of tissues with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Tissue); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Cell); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Organoid); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Xenograft); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int NumberOfSsms { get => _numberOfSsms ?? GetNumberOfVariants(Samples, VariantTypes.SSM); set => _numberOfSsms = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCnvs { get => _numberOfCnvs ?? GetNumberOfVariants(Samples, VariantTypes.CNV); set => _numberOfCnvs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSvs { get => _numberOfSvs ?? GetNumberOfVariants(Samples, VariantTypes.SV); set => _numberOfSvs = value; }


    /// <summary>
    /// Raw expression stats for this gene.
    /// </summary>
    public BulkExpressionStatsIndex Reads { get => _reads ?? GetExpression(Samples, index => index.Reads); set => _reads = value; }

    /// <summary>
    /// TPM expression stats for this gene.
    /// </summary>
    public BulkExpressionStatsIndex Tpm { get => _tpm ?? GetExpression(Samples, index => index.Tpm); set => _tpm = value; }

    /// <summary>
    /// FPKM expression stats for this gene.
    /// </summary>
    public BulkExpressionStatsIndex Fpkm { get => _fpkm ?? GetExpression(Samples, index => index.Fpkm); set => _fpkm = value; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get => _data ?? GetData(); set => _data = value; }


    public SampleIndex[] Samples { get; set; }


    public static int GetNumberOfDonors(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfImages(SampleIndex[] samples, string type)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == type)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSpecimens(SampleIndex[] samples, string type)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Specimen.Type == type)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfVariants(SampleIndex[] samples, string type)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == type)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static BulkExpressionStatsIndex GetExpression(SampleIndex[] samples, Func<BulkExpressionIndex, double> getter)
    {
        var expressions = samples?
            .Where(sample => sample.Expression != null)
            .Select(sample => getter(sample.Expression))
            .OrderBy(expression => expression)
            .ToArray();

        if (expressions?.Any() == true)
        {
            return new BulkExpressionStatsIndex
            {
                Min = Math.Round(expressions.First()),
                Max = Math.Round(expressions.Last()),
                Mean = Math.Round(expressions.Average()),
                Median = Math.Round(expressions[expressions.Length / 2])
            };
        }
        else
        {
            return null;
        }
    }

    private DataIndex GetData()
    {
        return new DataIndex
        {
            Donors = true,
            Clinical = Samples?.Any(sample => sample.Donor.ClinicalData != null),
            Treatments = Samples?.Any(sample => sample.Donor.Treatments?.Any() == true),
            Mris = Samples?.Any(sample => sample.Images?.Any(image => image.Mri != null) == true),
            // Cts = Samples?.Any(sample => sample.Images?.Any(image => image.Ct != null));
            Tissues = Samples?.Any(sample => sample.Specimen.Tissue != null),
            TissuesMolecular = Samples?.Any(sample => sample.Specimen.Tissue?.MolecularData != null),
            Cells = Samples?.Any(sample => sample.Specimen.Cell != null),
            CellsMolecular = Samples.Any(sample => sample.Specimen.Cell?.MolecularData != null),
            CellsDrugs = Samples?.Any(sample => sample.Specimen.Cell?.DrugScreenings?.Any() == true),
            Organoids = Samples?.Any(sample => sample.Specimen.Organoid != null),
            OrganoidsMolecular = Samples?.Any(sample => sample.Specimen.Organoid?.MolecularData != null),
            OrganoidsDrugs = Samples?.Any(sample => sample.Specimen.Organoid?.DrugScreenings?.Any() == true),
            OrganoidsInterventions = Samples?.Any(sample => sample.Specimen.Organoid?.Interventions?.Any() == true),
            Xenografts = Samples?.Any(sample => sample.Specimen.Xenograft != null),
            XenograftsMolecular = Samples?.Any(sample => sample.Specimen.Xenograft?.MolecularData != null),
            XenograftsDrugs = Samples?.Any(sample => sample.Specimen.Xenograft?.DrugScreenings?.Any() == true),
            XenograftsInterventions = Samples?.Any(sample => sample.Specimen.Xenograft?.Interventions?.Any() == true),
            Ssms = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Ssm != null) == true),
            Cnvs = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Cnv != null) == true),
            Svs = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Sv != null) == true),
            GeneExp = Samples?.Any(sample => sample.Expression != null),
            GeneExpSc = false
        };
    }
}
