using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Criteria;

public record SearchCriteria
{
    public int From { get; set; }
    public int Size { get; set; }
    public string Term { get; set; }

    public DonorCriteria Donor { get; set; }
    public ImageCriteria Image { get; set; }
    public MriImageCriteria Mri { get; set; }
    public SpecimenCriteria Specimen { get; set; }
    public MaterialCriteria Material { get; set; }
    public LineCriteria Line { get; set; }
    public OrganoidCriteria Organoid { get; set; }
    public XenograftCriteria Xenograft { get; set; }
    public GeneCriteria Gene { get; set; }
    public VariantCriteria Variant { get; set; }
    public SsmCriteria Ssm { get; set; }
    public CnvCriteria Cnv { get; set; }
    public SvCriteria Sv { get; set; }

    public bool HasGeneFilters =>
        Gene?.IsNotEmpty() == true;

    public bool HasVariantFilters =>
        Ssm?.IsNotEmpty() == true ||
        Cnv?.IsNotEmpty() == true ||
        Sv?.IsNotEmpty() == true;

    public SearchCriteria()
    {
        From = 0;
        Size = 20;
    }
}
