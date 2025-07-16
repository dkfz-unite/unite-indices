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
    public MrImageCriteria Mr { get; set; }
    public SpecimensCriteria Specimen { get; set; }
    public MaterialCriteria Material { get; set; }
    public LineCriteria Line { get; set; }
    public OrganoidCriteria Organoid { get; set; }
    public XenograftCriteria Xenograft { get; set; }
    public GeneCriteria Gene { get; set; }
    public SmCriteria Sm { get; set; }
    public CnvCriteria Cnv { get; set; }
    public SvCriteria Sv { get; set; }


    public bool HasDonorFilters =>
        Donor?.IsNotEmpty() == true;

    public bool HasImageFilters =>
        Image?.IsNotEmpty() == true ||
        Mr?.IsNotEmpty() == true;

    public bool HasSpecimenFilters =>
        Specimen?.IsNotEmpty() == true ||
        Material?.IsNotEmpty() == true ||
        Line?.IsNotEmpty() == true ||
        Organoid?.IsNotEmpty() == true ||
        Xenograft?.IsNotEmpty() == true;

    public bool HasGeneFilters =>
        Gene?.IsNotEmpty() == true;

    public bool HasVariantFilters =>
        Sm?.IsNotEmpty() == true ||
        Cnv?.IsNotEmpty() == true ||
        Sv?.IsNotEmpty() == true;

    public bool HasSmFilters =>
        Sm?.IsNotEmpty() == true;
    
    public bool HasCnvFilters =>
        Cnv?.IsNotEmpty() == true;

    public bool HasSvFilters =>
        Sv?.IsNotEmpty() == true;

    public bool AreDonorFiltersNegative =>
        Donor?.IsNegative() == true;

    public bool AreImageFiltersNegative =>
        Mr?.IsNegative() == true;

    public bool AreSpecimenFiltersNegative =>
        Material?.IsNegative() == true ||
        Line?.IsNegative() == true ||
        Organoid?.IsNegative() == true ||
        Xenograft?.IsNegative() == true;

    public bool AreGeneFiltersNegative =>
        Gene?.IsNegative() == true;

    public bool AreSmFiltersNegative =>
        Sm?.IsNegative() == true;

    public bool AreCnvFiltersNegative =>
        Cnv?.IsNegative() == true;

    public bool AreSvFiltersNegative =>
        Sv?.IsNegative() == true;
        

    public SearchCriteria()
    {
        From = 0;
        Size = 20;
    }
}
