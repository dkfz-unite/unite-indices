using Unite.Indices.Entities.Projects;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Projects;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class ProjectFiltersCollection : FiltersCollection<ProjectIndex>
{
    public ProjectFiltersCollection(SearchCriteria criteria) : base()
    {
        var projectFilters = new ProjectFilters<ProjectIndex>(criteria.Project, project => project);

        var projectsNavFilters = new ProjectsNavFilters<ProjectIndex>(criteria.Project, project => project);
        var projectsDataFilters = new DataFilters<ProjectIndex>(criteria.Project, project => project.Data);
        var donorsNavFilters = new DonorsNavFilters<ProjectIndex>(criteria.Donor, project => project.Donors.First());
        var imagesNavFilters = new ImagesNavFilters<ProjectIndex>(criteria.Image, project => project.Donors.First().Images.First());
        var specimensNavFilters = new SpecimensNavFilters<ProjectIndex>(criteria.Specimen, project => project.Donors.First().Specimens.First());


        Add(projectFilters.All());
        
        Add(projectsNavFilters.All());
        Add(projectsDataFilters.All());
        Add(donorsNavFilters.All());
        Add(imagesNavFilters.All());
        Add(specimensNavFilters.All());
    }
}
