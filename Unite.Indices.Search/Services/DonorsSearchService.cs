﻿using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class DonorsSearchService : SearchService<DonorIndex>
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    

    public DonorsSearchService(IElasticOptions options) : base(options)
    {
        _donorsIndexService = new DonorsIndexService(options);
    }


    public override DonorIndex Get(string key)
    {
        var query = new GetQuery<DonorIndex>(key)
            .AddExclusion(donor => donor.Specimens.First().Tissue)
            .AddExclusion(donor => donor.Specimens.First().Cell)
            .AddExclusion(donor => donor.Specimens.First().Organoid)
            .AddExclusion(donor => donor.Specimens.First().Xenograft);

        return _donorsIndexService.Get(query).Result;
    }

    public override SearchResult<DonorIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if (criteria.HasGeneFilters)
            ids = AggregateFromGenes(index => index.Specimens.First().Donor.Id, criteria);
        else if (criteria.HasVariantFilters)
            ids = AggregateFromVariants(index => index.Specimens.First().Donor.Id, criteria);

        if (ids != null)
        {
            if (ids.Length > 0)
                criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };
            else
                return new SearchResult<DonorIndex>();
        }

        var filters = new DonorFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes)
            .AddExclusion(donor => donor.Specimens)
            .AddExclusion(donor => donor.Images);

        return _donorsIndexService.Search(query).Result;
    }

    
    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, DonorIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
