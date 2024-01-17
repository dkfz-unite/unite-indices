using Unite.Indices.Entities.Basic.Genome.Variants.Constants;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class VariantFiltersCollection : FiltersCollection<VariantIndex>
{
    public VariantFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<VariantIndex>(criteria.Donor, variant => variant.Specimens.First().Donor);
        var imageFilters = new ImageFilters<VariantIndex>(criteria.Image, variant => variant.Specimens.First().Images.First());
        var mriImageFilters = new MriImageFilters<VariantIndex>(criteria.Mri, variant => variant.Specimens.First().Images.First().Mri);
        var specimenFilters = new SpecimenFilters<VariantIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var materialFilters = new MaterialFilters<VariantIndex>(criteria.Material, variant => variant.Specimens.First().Material);
        var lineFilters = new LineFilters<VariantIndex>(criteria.Line, variant => variant.Specimens.First().Line);
        var organoidFilters = new OrganoidFilters<VariantIndex>(criteria.Organoid, variant => variant.Specimens.First().Organoid);
        var xenograftFilters = new XenograftFilters<VariantIndex>(criteria.Xenograft, variant => variant.Specimens.First().Xenograft);
        var variantFilters = new VariantFilters<VariantIndex>(criteria.Variant, variant => variant);
        var ssmFilters = new SsmFilters<VariantIndex>(criteria.Ssm, variant => variant.Ssm);
        var cnvFilters = new CnvFilters<VariantIndex>(criteria.Cnv, variant => variant.Cnv);
        var svFilters = new SvFilters<VariantIndex>(criteria.Sv, variant => variant.Sv);

        Add(donorFilters.All());
        Add(imageFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(materialFilters.All());
        Add(lineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
        Add(variantFilters.All());
        Add(ssmFilters.All());
        Add(cnvFilters.All());
        Add(svFilters.All());


        // Magic. We apply gene filters based on the variant type, 
        // because gene filters live in separate criteria, 
        // but gene index for search is part of the specific variant type index.
        // Only variants of specific type can be filtered by gene criteria.
        if (criteria.Variant?.Type?.First() == VariantType.SSM)
        {
            var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Ssm.AffectedFeatures.First().Gene);
            Add(geneFilters.All());
        }
        else if (criteria.Variant?.Type?.First() == VariantType.CNV)
        {
            var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Cnv.AffectedFeatures.First().Gene);
            Add(geneFilters.All());
        }
        else if (criteria.Variant?.Type?.First() == VariantType.SV)
        {
            var geneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Sv.AffectedFeatures.First().Gene);
            Add(geneFilters.All());
        }
    }
}
