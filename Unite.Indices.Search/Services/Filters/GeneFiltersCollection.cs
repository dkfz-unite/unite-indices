using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class GeneFiltersCollection : FiltersCollection<GeneIndex>
{
    public GeneFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<GeneIndex>(criteria.Donor, gene => gene.Specimens.First().Donor);
        var imageFilters = new ImageFilters<GeneIndex>(criteria.Image, gene => gene.Specimens.First().Images.First());
        var mriImageFilters = new MriImageFilters<GeneIndex>(criteria.Mri, gene => gene.Specimens.First().Images.First().Mri);
        var specimenFilters = new SpecimenFilters<GeneIndex>(criteria.Specimen, gene => gene.Specimens.First());
        var materialFilters = new MaterialFilters<GeneIndex>(criteria.Material, gene => gene.Specimens.First().Material);
        var lineFilters = new LineFilters<GeneIndex>(criteria.Line, gene => gene.Specimens.First().Line);
        var organoidFilters = new OrganoidFilters<GeneIndex>(criteria.Organoid, gene => gene.Specimens.First().Organoid);
        var xenograftFilters = new XenograftFilters<GeneIndex>(criteria.Xenograft, gene => gene.Specimens.First().Xenograft);
        var geneFilters = new GeneFilters<GeneIndex>(criteria.Gene, gene => gene);
        var geneDataFilters = new GeneDataFilters<GeneIndex>(criteria.Gene, gene => gene.Data);
        var variantFilters = new VariantFilters<GeneIndex>(criteria.Variant, gene => gene.Specimens.First().Variants.First());
        var ssmFilters = new SsmFilters<GeneIndex>(criteria.Ssm, gene => gene.Specimens.First().Variants.First().Ssm);
        var cnvFilters = new CnvFilters<GeneIndex>(criteria.Cnv, gene => gene.Specimens.First().Variants.First().Cnv);
        var svFilters = new SvFilters<GeneIndex>(criteria.Sv, gene => gene.Specimens.First().Variants.First().Sv);

        Add(donorFilters.All());
        Add(imageFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(materialFilters.All());
        Add(lineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
        Add(geneFilters.All());
        Add(geneDataFilters.All());
        Add(variantFilters.All());
        Add(ssmFilters.All());
        Add(cnvFilters.All());
        Add(svFilters.All());
    }
}
