using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Criteria;

public record SearchCriteria
{
    public int From { get; set; }
    public int Size { get; set; }
    public string Term { get; set; }

    public ProjectCriteria Project { get; set; }
    public DonorCriteria Donor { get; set; }
    public ImagesCriteria Image { get; set; }
    public MriImageCriteria Mri { get; set; }
    public SpecimensCriteria Specimen { get; set; }
    public MaterialCriteria Material { get; set; }
    public LineCriteria Line { get; set; }
    public OrganoidCriteria Organoid { get; set; }
    public XenograftCriteria Xenograft { get; set; }
    public GeneCriteria Gene { get; set; }
    public SsmCriteria Ssm { get; set; }
    public CnvCriteria Cnv { get; set; }
    public SvCriteria Sv { get; set; }


    public bool HasDonorFilters =>
        Donor?.IsNotEmpty() == true;

    public bool HasImageFilters =>
        Image?.IsNotEmpty() == true ||
        Mri?.IsNotEmpty() == true;

    public bool HasSpecimenFilters =>
        Specimen?.IsNotEmpty() == true ||
        Material?.IsNotEmpty() == true ||
        Line?.IsNotEmpty() == true ||
        Organoid?.IsNotEmpty() == true ||
        Xenograft?.IsNotEmpty() == true;

    public bool HasGeneFilters =>
        Gene?.IsNotEmpty() == true;

    public bool HasVariantFilters =>
        Ssm?.IsNotEmpty() == true ||
        Cnv?.IsNotEmpty() == true ||
        Sv?.IsNotEmpty() == true;

    public bool HasSsmFilters =>
        Ssm?.IsNotEmpty() == true;
    
    public bool HasCnvFilters =>
        Cnv?.IsNotEmpty() == true;

    public bool HasSvFilters =>
        Sv?.IsNotEmpty() == true;

    public SearchCriteria()
    {
        From = 0;
        Size = 20;
    }
}
