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
        var materialFilters = new MaterialFilters<SpecimenIndex>(criteria.Material, specimen => specimen.Material);
        var lineFilters = new LineFilters<SpecimenIndex>(criteria.Line, specimen => specimen.Line);
        var organoidFilters = new OrganoidFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid);
        var xenograftFilters = new XenograftFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen.Xenograft);

        var donorsNavFilters = new DonorsNavFilters<SpecimenIndex>(criteria.Donor, specimen => specimen.Donor);
        var imagesNavFilters = new ImagesNavFilters<SpecimenIndex>(criteria.Image, specimen => specimen.Images.First());
        var specimensNavFilters = new SpecimensNavFilters<SpecimenIndex>(criteria.Specimen, specimen => specimen);
        var specimensDataFilters = new DataFilters<SpecimenIndex>(criteria.Specimen, specimen => specimen.Data);
        
        
        Add(materialFilters.All());
        Add(lineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());

        Add(donorsNavFilters.All());
        Add(imagesNavFilters.All());
        Add(specimensNavFilters.All());
        Add(specimensDataFilters.All());
    }
}
