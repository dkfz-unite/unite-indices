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

    private GeneExpressionStatsIndex _reads;
    private GeneExpressionStatsIndex _tpm;
    private GeneExpressionStatsIndex _fpkm;

    private DataIndex _data;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Samples); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRI images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfMris(Samples); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CT images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfCts(Samples); set => _numberOfCts = value; }

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
    public int NumberOfSsms { get => _numberOfSsms ?? GetNumberOfSsms(Samples); set => _numberOfSsms = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCnvs { get => _numberOfCnvs ?? GetNumberOfCnvs(Samples); set => _numberOfCnvs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSvs { get => _numberOfSvs ?? GetNumberOfSvs(Samples); set => _numberOfSvs = value; }


    /// <summary>
    /// Raw expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex Reads { get => _reads ?? GetExpression(Samples, index => index.Reads); set => _reads = value; }

    /// <summary>
    /// TPM expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex Tpm { get => _tpm ?? GetExpression(Samples, index => index.Tpm); set => _tpm = value; }

    /// <summary>
    /// FPKM expression stats for this gene.
    /// </summary>
    public GeneExpressionStatsIndex Fpkm { get => _fpkm ?? GetExpression(Samples, index => index.Fpkm); set => _fpkm = value; }

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

    public static int GetNumberOfMris(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCts(SampleIndex[] samples)
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

    public static int GetNumberOfSsms(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.SSM)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCnvs(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Variants?.Any() == true)
            .SelectMany(sample => sample.Variants)
            .Where(variant => variant.Type == VariantTypes.CNV)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSvs(SampleIndex[] samples)
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

    private DataIndex GetData()
    {
        var index = new DataIndex();

        index.Donors = true;
        index.Clinical = Samples?.Any(sample => sample.Donor.ClinicalData != null);
        index.Treatments = Samples?.Any(sample => sample.Donor.Treatments?.Any() == true);
        index.Mris = Samples?.Any(sample => sample.Images?.Any(image => image.Mri != null) == true);
        // index.Cts = Samples?.Any(sample => sample.Images?.Any(image => image.Ct != null));
        index.Tissues = Samples?.Any(sample => sample.Specimen.Tissue != null);
        index.TissuesMolecular = Samples?.Any(sample => sample.Specimen.Tissue?.MolecularData != null);
        index.Cells = Samples?.Any(sample => sample.Specimen.Cell != null);
        index.CellsMolecular = Samples.Any(sample => sample.Specimen.Cell?.MolecularData != null);
        index.CellsDrugs = Samples?.Any(sample => sample.Specimen.Cell?.DrugScreenings?.Any() == true);
        index.Organoids = Samples?.Any(sample => sample.Specimen.Organoid != null);
        index.OrganoidsMolecular = Samples?.Any(sample => sample.Specimen.Organoid?.MolecularData != null);
        index.OrganoidsDrugs = Samples?.Any(sample => sample.Specimen.Organoid?.DrugScreenings?.Any() == true);
        index.OrganoidsInterventions = Samples?.Any(sample => sample.Specimen.Organoid?.Interventions?.Any() == true);
        index.Xenografts = Samples?.Any(sample => sample.Specimen.Xenograft != null);
        index.XenograftsMolecular = Samples?.Any(sample => sample.Specimen.Xenograft?.MolecularData != null);
        index.XenograftsDrugs = Samples?.Any(sample => sample.Specimen.Xenograft?.DrugScreenings?.Any() == true);
        index.XenograftsInterventions = Samples?.Any(sample => sample.Specimen.Xenograft?.Interventions?.Any() == true);
        index.Ssms = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Ssm != null) == true);
        index.Cnvs = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Cnv != null) == true);
        index.Svs = Samples?.Any(sample => sample.Variants?.Any(variant => variant.Sv != null) == true);
        index.GeneExp = Samples?.Any(sample => sample.Expression != null);
        index.GeneExpSc = false;
        
        return index;
    }
}
