using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Services.Filters;

public class VariantFiltersCollection : FiltersCollection<VariantIndex>
{
    public VariantFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<VariantIndex>(criteria.Donor, variant => variant.Specimens.First().Donor);
        var mriImageFilters = new MriImageFilters<VariantIndex>(criteria.Mri, variant => variant.Specimens.First().Images.First());
        var tissueFilters = new TissueFilters<VariantIndex>(criteria.Tissue, variant => variant.Specimens.First());
        var cellLineFilters = new CellLineFilters<VariantIndex>(criteria.Cell, variant => variant.Specimens.First());
        var organoidFilters = new OrganoidFilters<VariantIndex>(criteria.Organoid, variant => variant.Specimens.First());
        var xenograftFilters = new XenograftFilters<VariantIndex>(criteria.Xenograft, variant => variant.Specimens.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());

        if (IsNotEmpty(criteria.Image?.Id))
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
              ImageFilterNames.Id,
              variant => variant.Specimens.First().Images.First().Id,
              criteria.Image.Id)
            );
        }

        if (IsNotEmpty(criteria.Specimen?.Id))
        {
            _filters.Add(new EqualityFilter<VariantIndex, int>(
                SpecimenFilterNames.Id,
                variant => variant.Specimens.First().Id,
                criteria.Specimen.Id)
            );
        }
    }
}
