using Unite.Essentials.Extensions;
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

        var donorsToExclude = new HashSet<string>();

        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Id, criteria, exclusive);

            if (exclusive)
            {
                donorsToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Donor = Set(criteria.Donor, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<ProjectIndex>();

                if (criteria.Donor.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (donorsToExclude.Count > 0)
        {
            criteria.Donor = Set(criteria.Donor, [.. donorsToExclude.Select(int.Parse)], true);
        }


        var imagesToExclude = new HashSet<string>();

        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Id, criteria, exclusive);

            if (exclusive)
            {
                imagesToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<ProjectIndex>();

                if (criteria.Image.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (imagesToExclude.Count > 0)
        {
            criteria.Image = Set(criteria.Image, [.. imagesToExclude.Select(int.Parse)], true);
        }


        var specimensToExclude = new HashSet<string>();

        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else
                    return new SearchResult<ProjectIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else
                    return new SearchResult<ProjectIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else
                    return new SearchResult<ProjectIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else
                    return new SearchResult<ProjectIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else
                    return new SearchResult<ProjectIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<ProjectIndex>();
            }
        }

        if (specimensToExclude.Count > 0)
        {
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
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
