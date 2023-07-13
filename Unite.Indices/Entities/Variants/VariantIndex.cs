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
    public int NumberOfMris { get => _numberOfMris ?? GetNumberOfMris(Samples); set => _numberOfMris = value; }

    /// <summary>
    /// Number of CTs with the variant in any sample.
    /// </summary>
    public int NumberOfCts { get => _numberOfCts ?? GetNumberOfCts(Samples); set => _numberOfCts = value; }

    /// <summary>
    /// Number of tissues with the variant in any sample.
    /// </summary>
    public int NumberOfTissues { get => _numberOfTissues ?? GetNumberOfTissues(Samples); set => _numberOfTissues = value; }

    /// <summary>
    /// Number of cell lines with the variant in any sample.
    /// </summary>
    public int NumberOfCells { get => _numberOfCells ?? GetNumberOfCells(Samples); set => _numberOfCells = value; }

    /// <summary>
    /// Number of organoids with the variant in any sample.
    /// </summary>
    public int NumberOfOrganoids { get => _numberOfOrganoids ?? GetNumberOfOrganoids(Samples); set => _numberOfOrganoids = value; }

    /// <summary>
    /// Number of xenografts with the variant in any sample.
    /// </summary>
    public int NumberOfXenografts { get => _numberOfXenografts ?? GetNumberOfXenografts(Samples); set => _numberOfXenografts = value; }

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

    public static int GetNumberOfMris(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.MRI)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCts(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Images?.Any() == true)
            .SelectMany(sample => sample.Images)
            .Where(image => image.Type == ImageTypes.CT)
            .DistinctBy(image => image.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfTissues(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Tissue)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfCells(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Cell)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfOrganoids(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Organoid)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    public static int GetNumberOfXenografts(SampleIndex[] samples)
    {
        return samples?
            .Where(sample => sample.Specimen.Type == SpecimenTypes.Xenograft)
            .DistinctBy(sample => sample.Specimen.Id)
            .Count() ?? 0;
    }

    private DataIndex GetData()
    {
         var index = new DataIndex();

        index.Donors = true;
        index.Clinical = Samples.Any(sample => sample.Donor.ClinicalData != null);
        index.Treatments = Samples.Any(sample => sample.Donor.Treatments?.Any() == true);
        index.Mris = Samples.Any(sample => sample.Images.Any(image => image.Mri != null));
        // index.Cts = Samples.Any(sample => sample.Images.Any(image => image.Ct != null));
        index.Tissues = Samples.Any(sample => sample.Specimen.Tissue != null);
        index.TissuesMolecular = Samples.Any(sample => sample.Specimen.Tissue?.MolecularData != null);
        index.Cells = Samples.Any(sample => sample.Specimen.Cell != null);
        index.CellsMolecular = Samples.Any(sample => sample.Specimen.Cell?.MolecularData != null);
        index.CellsDrugs = Samples.Any(sample => sample.Specimen.Cell?.DrugScreenings?.Any() == true);
        index.Organoids = Samples.Any(sample => sample.Specimen.Organoid != null);
        index.OrganoidsMolecular = Samples.Any(sample => sample.Specimen.Organoid?.MolecularData != null);
        index.OrganoidsDrugs = Samples.Any(sample => sample.Specimen.Organoid?.DrugScreenings?.Any() == true);
        index.OrganoidsInterventions = Samples.Any(sample => sample.Specimen.Organoid?.Interventions?.Any() == true);
        index.Xenografts = Samples.Any(sample => sample.Specimen.Xenograft != null);
        index.XenograftsMolecular = Samples.Any(sample => sample.Specimen.Xenograft?.MolecularData != null);
        index.XenograftsDrugs = Samples.Any(sample => sample.Specimen.Xenograft?.DrugScreenings?.Any() == true);
        index.XenograftsInterventions = Samples.Any(sample => sample.Specimen.Xenograft?.Interventions?.Any() == true);
        index.Ssms = Ssm != null; // Intersections can not be calculated here.
        index.Cnvs = Cnv != null; // Intersections can not be calculated here.
        index.Svs = Sv != null; // Intersections can not be calculated here.
        index.GeneExp = null; // Can not be calculated here.
        index.GeneExpSc = null; // Can not be calculated here.

        return index;
    }

    private int GetNumberOfGenes()
    {
        return GetAffectedFeatures()?
               .Where(feature => feature.Gene != null)
               .DistinctBy(feature => feature.Gene.Id)
               .Count() ?? 0;
    }
}
