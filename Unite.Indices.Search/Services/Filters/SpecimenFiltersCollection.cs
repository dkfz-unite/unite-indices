using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class SpecimenFiltersCollection : FiltersCollection<SpecimenIndex>
{
    public SpecimenFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<SpecimenIndex>(criteria.Donor, specimen => specimen.Donor);
        var imageFilters = new ImageFilters<SpecimenIndex>(criteria.Image, specimen => specimen.Images.First());
        var mriImageFilters = new MriImageFilters<SpecimenIndex>(criteria.Mri, specimen => specimen.Images.First().Mri);
        var specimenFilters = new SpecimenFilters<SpecimenIndex>(criteria.Specimen, specimen => specimen);
        var specimenDataFilters = new SpecimenDataFilters<SpecimenIndex>(criteria.Specimen, specimen => specimen.Data);
        var tissueFilters = new TissueFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen.Tissue);
        var cellLineFilters = new CellLineFilters<SpecimenIndex>(criteria.Cell, specimen => specimen.Cell);
        var organoidFilters = new OrganoidFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid);
        var xenograftFilters = new XenograftFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen.Xenograft);
        
        Add(donorFilters.All());
        Add(imageFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(specimenDataFilters.All());
        Add(tissueFilters.All());
        Add(cellLineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
    }
}
