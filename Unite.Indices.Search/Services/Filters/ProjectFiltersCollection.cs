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

        var projectsFilters = new ProjectsFilters<ProjectIndex>(criteria.Project, project => project);
        var projectsDataFilters = new ProjectsDataFilters<ProjectIndex>(criteria.Project, project => project.Data);
        var donorsFilters = new DonorsFilters<ProjectIndex>(criteria.Donor, project => project.Donors.First());
        var imagesFilters = new ImagesFilters<ProjectIndex>(criteria.Image, project => project.Donors.First().Images.First());
        var specimensFilters = new SpecimensFilters<ProjectIndex>(criteria.Specimen, project => project.Donors.First().Specimens.First());


        Add(projectFilters.All());
        
        Add(projectsFilters.All());
        Add(projectsDataFilters.All());
        Add(donorsFilters.All());
        Add(imagesFilters.All());
        Add(specimensFilters.All());
    }
}
