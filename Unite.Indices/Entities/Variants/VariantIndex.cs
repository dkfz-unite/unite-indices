using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMris;
    private int? _numberOfCts;
    private int? _numberOfTissues;
    private int? _numberOfCells;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfGenes;

    private DataIndex _data;


    /// <summary>
    /// Number of donors with the variant in any sample.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Samples); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRIs with the variant in any sample.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfImages(Samples, ImageTypes.MRI); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CTs with the variant in any sample.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfImages(Samples, ImageTypes.CT); set => _numberOfCts = value; }

    /// <summary>
    /// Number of tissues with the variant in any sample.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Tissue); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with the variant in any sample.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Cell); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with the variant in any sample.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Organoid); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with the variant in any sample.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfSpecimens(Samples, SpecimenTypes.Xenograft); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of genes affected by the variant in any sample.
    /// </summary>
    public int NumberOfGenes { get => _numberOfGenes ?? GetNumberOfGenes(); set => _numberOfGenes = value; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get => _data ?? GetData(); set => _data = value; }


    public SampleIndex[] Samples { get; set; }


    public static int GetNumberOfDonors(SampleIndex[] samples)
    {
        return samples?
            .DistinctBy(sample => sample.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfImages(SampleIndex[] samples, string type)
    {
        return samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == type)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSpecimens(SampleIndex[] samples, string type)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == type)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private DataIndex GetData()
    {
        return new DataIndex
        {
            Donors = true,
            Clinical = Samples.Any(sample => sample.Donor.ClinicalData != null),
            Treatments = Samples.Any(sample => sample.Donor.Treatments?.Any() == true),
            Mris = Samples.Any(sample => sample.Images.Any(image => image.Mri != null)),
            // Cts = Samples.Any(sample => sample.Images.Any(image => image.Ct != null));
            Tissues = Samples.Any(sample => sample.Specimen.Tissue != null),
            TissuesMolecular = Samples.Any(sample => sample.Specimen.Tissue?.MolecularData != null),
            Cells = Samples.Any(sample => sample.Specimen.Cell != null),
            CellsMolecular = Samples.Any(sample => sample.Specimen.Cell?.MolecularData != null),
            CellsDrugs = Samples.Any(sample => sample.Specimen.Cell?.DrugScreenings?.Any() == true),
            Organoids = Samples.Any(sample => sample.Specimen.Organoid != null),
            OrganoidsMolecular = Samples.Any(sample => sample.Specimen.Organoid?.MolecularData != null),
            OrganoidsDrugs = Samples.Any(sample => sample.Specimen.Organoid?.DrugScreenings?.Any() == true),
            OrganoidsInterventions = Samples.Any(sample => sample.Specimen.Organoid?.Interventions?.Any() == true),
            Xenografts = Samples.Any(sample => sample.Specimen.Xenograft != null),
            XenograftsMolecular = Samples.Any(sample => sample.Specimen.Xenograft?.MolecularData != null),
            XenograftsDrugs = Samples.Any(sample => sample.Specimen.Xenograft?.DrugScreenings?.Any() == true),
            XenograftsInterventions = Samples.Any(sample => sample.Specimen.Xenograft?.Interventions?.Any() == true),
            Ssms = Ssm != null, // Intersections can not be calculated here.
            Cnvs = Cnv != null, // Intersections can not be calculated here.
            Svs = Sv != null, // Intersections can not be calculated here.
            GeneExp = null, // Can not be calculated here.
            GeneExpSc = null // Can not be calculated here.
        };
    }

    private int GetNumberOfGenes()
    {
        return GetAffectedFeatures()?
               .Where(feature => feature.Gene != null)
               .DistinctBy(feature => feature.Gene.Id)
               .Count() ?? 0;
    }
}
