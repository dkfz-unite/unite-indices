using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    private int? _numberOfDonors;
    private int? _numberOfMris;
    private int? _numberOfCts;
    private int? _numberOfMaterials;
    private int? _numberOfLines;
    private int? _numberOfOrganoids;
    private int? _numberOfXenografts;
    private int? _numberOfGenes;

    private DataIndex _data;


    /// <summary>
    /// Number of donors with the variant in any sample.
    /// </summary>
    public int NumberOfDonors { get => _numberOfDonors ?? GetNumberOfDonors(Specimens); set => _numberOfDonors = value; }

    /// <summary>
    /// Number of MRIs with the variant in any sample.
    /// </summary>
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfImages(Specimens, ImageType.MRI); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CTs with the variant in any sample.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfImages(Specimens, ImageType.CT); set => _numberOfCts = value; }

    /// <summary>
    /// Number of donor derived materials with the variant in any sample.
    /// </summary>
    public int NumberOfMaterials { get => _numberOfMaterials ?? GetNumberOfSpecimens(Specimens, SpecimenType.Material); set => _numberOfMaterials = value; }

    /// <summary>
    /// Number of cell lines with the variant in any sample.
    /// </summary>
    public int NumberOfLines { get => _numberOfLines ?? GetNumberOfSpecimens(Specimens, SpecimenType.Line); set => _numberOfLines = value; }

    /// <summary>
    /// Number of organoids with the variant in any sample.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfSpecimens(Specimens, SpecimenType.Organoid); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with the variant in any sample.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfSpecimens(Specimens, SpecimenType.Xenograft); set => _numberOfXenografts = value; }

    /// <summary>
    /// Number of genes affected by the variant in any sample.
    /// </summary>
    public int NumberOfGenes { get => _numberOfGenes ?? GetNumberOfGenes(); set => _numberOfGenes = value; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get => _data ?? GetData(); set => _data = value; }


    public SpecimenIndex[] Specimens { get; set; }


    public static int GetNumberOfDonors(SpecimenIndex[] specimens)
    {
        return specimens?
            .DistinctBy(specimen => specimen.Donor.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfImages(SpecimenIndex[] specimens, string type)
    {
        return specimens?
            .Where(specimen => specimen.Images?.Any() == true)
            .SelectMany(specimen => specimen.Images)
            .Where(specimen => specimen.Type == type)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfSpecimens(SpecimenIndex[] specimens, string type)
    {
        return specimens?
            .Where(specimen => specimen.Type == type)
            .DistinctBy(specimen => specimen.Id)
            .Count() ?? 0;
    }

    private DataIndex GetData()
    {
        return new DataIndex
        {
            Donors = true,
            Clinical = Specimens.Any(specimen => specimen.Donor.ClinicalData != null),
            Treatments = Specimens.Any(specimen => specimen.Donor.Treatments?.Any() == true),
            Mris = Specimens.Any(specimen => specimen.Images?.Any(image => image.Mri != null) == true),
            // Cts = Specimens.Any(specimen => specimen.Images?.Any(image => image.Ct != null) == true);
            Materials = Specimens.Any(specimen => specimen.Material != null),
            MaterialsMolecular = Specimens.Any(specimen => specimen.Material?.MolecularData != null),
            Lines = Specimens.Any(specimen => specimen.Line != null),
            LinesMolecular = Specimens.Any(specimen => specimen.Line?.MolecularData != null),
            LinesDrugs = Specimens.Any(specimen => specimen.Line?.DrugScreenings?.Any() == true),
            Organoids = Specimens.Any(specimen => specimen.Organoid != null),
            OrganoidsMolecular = Specimens.Any(specimen => specimen.Organoid?.MolecularData != null),
            OrganoidsDrugs = Specimens.Any(specimen => specimen.Organoid?.DrugScreenings?.Any() == true),
            OrganoidsInterventions = Specimens.Any(specimen => specimen.Organoid?.Interventions?.Any() == true),
            Xenografts = Specimens.Any(specimen => specimen.Xenograft != null),
            XenograftsMolecular = Specimens.Any(specimen => specimen.Xenograft?.MolecularData != null),
            XenograftsDrugs = Specimens.Any(specimen => specimen.Xenograft?.DrugScreenings?.Any() == true),
            XenograftsInterventions = Specimens.Any(specimen => specimen.Xenograft?.Interventions?.Any() == true),
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
