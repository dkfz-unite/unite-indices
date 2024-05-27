using Unite.Indices.Entities.Basic.Genome.Dna.Constants;
using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMris;
    private int? _numberOfCts;
    private int? _numberOfMaterials;
    private int? _numberOfLines;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfSsms;
    private int? _numberOfCnvs;
    private int? _numberOfSvs;

    private ExpressionStatsIndex _reads;
    private ExpressionStatsIndex _tpm;
    private ExpressionStatsIndex _fpkm;

    private DataIndex _data;


    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Specimens); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRI images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfImages(Specimens, ImageType.MRI); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CT images with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfImages(Specimens, ImageType.CT); set => _numberOfCts = value; }

    /// <summary>
    /// Number of donor derived materials with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfMaterials { get => _numberOfMaterials ?? GetNumberOfSpecimens(Specimens, SpecimenType.Material); set => _numberOfMaterials = value; }

    /// <summary>
    /// Number of cell lines with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfLines { get => _numberOfLines ?? GetNumberOfSpecimens(Specimens, SpecimenType.Line); set => _numberOfLines = value; }

    /// <summary>
    /// Number of organoids with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfSpecimens(Specimens, SpecimenType.Organoid); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfSpecimens(Specimens, SpecimenType.Xenograft); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int NumberOfSsms { get => _numberOfSsms ?? GetNumberOfVariants(Specimens, VariantType.SSM); set => _numberOfSsms = value; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int NumberOfCnvs { get => _numberOfCnvs ?? GetNumberOfVariants(Specimens, VariantType.CNV); set => _numberOfCnvs = value; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int NumberOfSvs { get => _numberOfSvs ?? GetNumberOfVariants(Specimens, VariantType.SV); set => _numberOfSvs = value; }


    /// <summary>
    /// Raw expression stats for this gene.
    /// </summary>
    public ExpressionStatsIndex Reads { get => _reads ?? GetExpression(Specimens, index => index.Reads); set => _reads = value; }

    /// <summary>
    /// TPM expression stats for this gene.
    /// </summary>
    public ExpressionStatsIndex Tpm { get => _tpm ?? GetExpression(Specimens, index => index.Tpm); set => _tpm = value; }

    /// <summary>
    /// FPKM expression stats for this gene.
    /// </summary>
    public ExpressionStatsIndex Fpkm { get => _fpkm ?? GetExpression(Specimens, index => index.Fpkm); set => _fpkm = value; }


    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get => _data ?? GetData(); set => _data = value; }


    public SpecimenIndex[] Specimens { get; set; }


    public static int GetNumberOfDonors(SpecimenIndex[] specimens)
    {
        return specimens?
            .Where(specimen => specimen.Variants?.Any() == true)
            .DistinctBy(specimens => specimens.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfImages(SpecimenIndex[] specimens, string type)
    {
        return specimens?
            .Where(specimen => specimen.Variants?.Any() == true)
            .Where(specimen => specimen.Images?.Any() == true)
            .SelectMany(specimen => specimen.Images)
            .Where(image => image.Type == type)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSpecimens(SpecimenIndex[] specimens, string type)
    {
        return specimens?
            .Where(specimen => specimen.Variants?.Any() == true)
            .Where(specimen => specimen.Type == type)
            .DistinctBy(specimen => specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfVariants(SpecimenIndex[] specimens, string type)
    {
        return specimens?
            .Where(specimen => specimen.Variants?.Any() == true)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Type == type)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    public static ExpressionStatsIndex GetExpression(SpecimenIndex[] specimens, Func<ExpressionIndex, double> getter)
    {
        var expressions = specimens?
            .Where(specimen => specimen.Expression != null)
            .Select(specimen => getter(specimen.Expression))
            .OrderBy(expression => expression)
            .ToArray();

        if (expressions?.Any() == true)
        {
            return new ExpressionStatsIndex
            {
                Min = Math.Round(expressions.First()),
                Max = Math.Round(expressions.Last()),
                Mdn = Math.Round(expressions[expressions.Length / 2])
            };
        }
        
        return null;
    }

    private DataIndex GetData()
    {
        return new DataIndex
        {
            Donors = true,
            Clinical = Specimens?.Any(specimen => specimen.Donor.ClinicalData != null),
            Treatments = Specimens?.Any(specimen => specimen.Donor.Treatments?.Any() == true),
            Mris = Specimens?.Any(specimen => specimen.Images?.Any(image => image.Mri != null) == true),
            // Cts = Specimens?.Any(specimen => specimen.Images?.Any(image => image.Ct != null) == true),
            Materials = Specimens?.Any(specimen => specimen.Material != null),
            MaterialsMolecular = Specimens?.Any(specimen => specimen.Material?.MolecularData != null),
            Lines = Specimens?.Any(specimen => specimen.Line != null),
            LinesMolecular = Specimens.Any(specimen => specimen.Line?.MolecularData != null),
            LinesDrugs = Specimens?.Any(specimen => specimen.Line?.DrugScreenings?.Any() == true),
            LinesInterventions = Specimens?.Any(specimen => specimen.Line?.Interventions?.Any() == true),
            Organoids = Specimens?.Any(specimen => specimen.Organoid != null),
            OrganoidsMolecular = Specimens?.Any(specimen => specimen.Organoid?.MolecularData != null),
            OrganoidsDrugs = Specimens?.Any(specimen => specimen.Organoid?.DrugScreenings?.Any() == true),
            OrganoidsInterventions = Specimens?.Any(specimen => specimen.Organoid?.Interventions?.Any() == true),
            Xenografts = Specimens?.Any(specimen => specimen.Xenograft != null),
            XenograftsMolecular = Specimens?.Any(specimen => specimen.Xenograft?.MolecularData != null),
            XenograftsDrugs = Specimens?.Any(specimen => specimen.Xenograft?.DrugScreenings?.Any() == true),
            XenograftsInterventions = Specimens?.Any(specimen => specimen.Xenograft?.Interventions?.Any() == true),
            Ssms = Specimens?.Any(specimen => specimen.Variants?.Any(variant => variant.Ssm != null) == true),
            Cnvs = Specimens?.Any(specimen => specimen.Variants?.Any(variant => variant.Cnv != null) == true),
            Svs = Specimens?.Any(specimen => specimen.Variants?.Any(variant => variant.Sv != null) == true),
            GeneExp = Specimens?.Any(specimen => specimen.Expression != null),
            GeneExpSc = false
        };
    }
}
