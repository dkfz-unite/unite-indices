using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Projects;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class ProjectsSearchService : SearchService<ProjectIndex>
{
    public ProjectsSearchService(IElasticOptions options) : base(options)
    {
    }


    public override async Task<ProjectIndex> Get(string key)
    {
        var query = new GetQuery<ProjectIndex>(key);

        return await _projectsIndexService.Get(query);
    }

    public override async Task<SearchResult<ProjectIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        if (criteria.HasDonorFilters)
        {
            var ids = await AggregateFromDonors(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Donor = Set(criteria.Donor, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Donor.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasImageFilters)
        {
            var ids = await AggregateFromImages(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Image.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasSpecimenFilters)
        {
            var ids = await AggregateFromSpecimens(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria);
            
            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasSsmFilters)
        {
            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ProjectIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ProjectIndex>();
        }

        var filters = new ProjectFiltersCollection(criteria).All();

        var query = new SearchQuery<ProjectIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(project => project.Stats.Donors.Number);

        return await _projectsIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, ProjectIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
