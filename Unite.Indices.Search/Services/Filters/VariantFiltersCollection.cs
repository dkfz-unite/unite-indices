﻿using Unite.Indices.Entities.Variants;
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
        var tissueFilters = new TissueFilters<VariantIndex>(criteria.Tissue, variant => variant.Specimens.First().Tissue);
        var cellLineFilters = new CellLineFilters<VariantIndex>(criteria.Cell, variant => variant.Specimens.First().Cell);
        var organoidFilters = new OrganoidFilters<VariantIndex>(criteria.Organoid, variant => variant.Specimens.First().Organoid);
        var xenograftFilters = new XenograftFilters<VariantIndex>(criteria.Xenograft, variant => variant.Specimens.First().Xenograft);
        var ssmGeneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Ssm.AffectedFeatures.First().Gene);
        var cnvGeneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Cnv.AffectedFeatures.First().Gene);
        var svGeneFilters = new GeneFilters<VariantIndex>(criteria.Gene, variant => variant.Sv.AffectedFeatures.First().Gene);
        var variantFilters = new VariantFilters<VariantIndex>(criteria.Variant, variant => variant);
        var ssmFilters = new SsmFilters<VariantIndex>(criteria.Ssm, variant => variant.Ssm);
        var cnvFilters = new CnvFilters<VariantIndex>(criteria.Cnv, variant => variant.Cnv);
        var svFilters = new SvFilters<VariantIndex>(criteria.Sv, variant => variant.Sv);

        Add(donorFilters.All());
        Add(imageFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(tissueFilters.All());
        Add(cellLineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
        Add(ssmGeneFilters.All());
        Add(cnvGeneFilters.All());
        Add(svGeneFilters.All());
        Add(variantFilters.All());
        Add(ssmFilters.All());
        Add(cnvFilters.All());
        Add(svFilters.All());
    }
}
