using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using GeneExpressionIndex = Unite.Indices.Entities.Genes.GeneExpressionIndex;
using ProteinIndex = Unite.Indices.Entities.Proteins.ProteinIndex;
using ProteinExpressionIndex = Unite.Indices.Entities.Proteins.ProteinExpressionIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;

namespace Unite.Indices.Search.Services;


public abstract class SearchService<T> : ISearchService<T> where T : class
{
    protected readonly ProjectsIndexService _projectsIndexService;
    protected readonly IIndexService<DonorIndex> _donorsIndexService;
    protected readonly IIndexService<ImageIndex> _imagesIndexService;
    protected readonly IIndexService<SpecimenIndex> _specimensIndexService;
    protected readonly IIndexService<GeneIndex> _genesIndexService;
    protected readonly IIndexService<GeneExpressionIndex> _geneExpressionsIndexService;
    protected readonly IIndexService<ProteinIndex> _proteinsIndexService;
    protected readonly IIndexService<ProteinExpressionIndex> _proteinExpressionsIndexService;
    protected readonly IIndexService<SmIndex> _smsIndexService;
    protected readonly IIndexService<CnvIndex> _cnvsIndexService;
    protected readonly IIndexService<SvIndex> _svsIndexService;
    protected readonly IIndexService<Entities.CnvProfiles.CnvProfileIndex> _cnvProfileIndexService;


    protected SearchService(IElasticOptions options)
    {
        _projectsIndexService = new ProjectsIndexService(options);
        _donorsIndexService = new DonorsIndexService(options);
        _imagesIndexService = new ImagesIndexService(options);
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _geneExpressionsIndexService = new GeneExpressionsIndexService(options);
        _proteinsIndexService = new ProteinsIndexService(options);
        _proteinExpressionsIndexService = new ProteinExpressionsIndexService(options);
        _smsIndexService = new SmsIndexService(options);
        _cnvsIndexService = new CnvsIndexService(options);
        _svsIndexService = new SvsIndexService(options);
        _cnvProfileIndexService = new CnvProfileIndexService(options);
    }


    public abstract Task<T> Get(string key);

    public abstract Task<SearchResult<T>> Search(PersonalSearchCriteria personalSearchCriteria);

    public virtual async Task<IReadOnlyDictionary<object, DataIndex>> Stats(PersonalSearchCriteria personalSearchCriteria)
    {
        var tempPersonalSearchCriteria = new PersonalSearchCriteria
        {
            UserId = personalSearchCriteria.UserId
        };
        
        tempPersonalSearchCriteria.SearchCriteria = personalSearchCriteria.SearchCriteria with { From = 0, Size = 0 };

        var lookupResult = await Search(tempPersonalSearchCriteria);

        var availableData = new Dictionary<object, DataIndex>();

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            tempPersonalSearchCriteria.SearchCriteria = personalSearchCriteria.SearchCriteria with { From = from, Size = 499 };

            var searchResult = await Search(tempPersonalSearchCriteria);

            foreach (var index in searchResult.Rows)
            {
                AddToStats(ref availableData, index);
            }
        }

        return availableData.AsReadOnly();
    }


    protected abstract void AddToStats(ref Dictionary<object, DataIndex> stats, T index);

    protected async Task<string[]> AggregateFromDonors<TProp>(Expression<Func<DonorIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new DonorFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromDonors(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    public async Task<string[]> AggregateFromImages<TProp>(Expression<Func<ImageIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new ImageFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromImages(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSpecimens<TProp>(Expression<Func<SpecimenIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new SpecimenFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromSpecimens(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new GeneFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromGenes(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromGeneExpressions<TProp>(Expression<Func<GeneExpressionIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new GeneExpressionFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromGeneExpressions(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromProteins<TProp>(Expression<Func<ProteinIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new ProteinFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromProteins(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromProteinExpressions<TProp>(Expression<Func<ProteinExpressionIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new ProteinExpressionFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromProteinExpressions(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSms<TProp>(Expression<Func<SmIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new SmFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromSms(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromCnvs<TProp>(Expression<Func<CnvIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new CnvFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromCnvs(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSvs<TProp>(Expression<Func<SvIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var filters = new SvFiltersCollection(personalCriteria.SearchCriteria);

        if (exclusive)
            filters.MakePositive();

        var aggregation = await AggregateFromSvs(property, personalCriteria, filters);

        return aggregation.Keys.ToArray();
    }
    
    protected async Task<string[]> AggregateFromCnvProfiles<TProp>(Expression<Func<Entities.CnvProfiles.CnvProfileIndex, TProp>> property, PersonalSearchCriteria personalCriteria, bool exclusive = false)
    {
        var criteria = personalCriteria.SearchCriteria;
        
        var filters = new CnvProfileFilters<Entities.CnvProfiles.CnvProfileIndex>(criteria.CnvProfile, cnvProfile => cnvProfile);

        if (exclusive)
            filters.MakePositive();

        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<Entities.CnvProfiles.CnvProfileIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property);

        var result = await _cnvProfileIndexService.Search(query);

        return result.Aggregations[aggregationName].Keys.ToArray();
    }

    protected static bool HandleFoundDonors(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Donor = Set(criteria.Donor, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Donor.Id.Length == 0)
                return true;
        }

        return false;
    }

    protected static bool HandleFoundImages(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Image.Id.Length == 0)
                return true;
        }

        return false;
    }

    protected static bool HandleFoundSpecimens(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Specimen.Id.Length == 0)
                return true;
        }

        return false;
    }

    protected static bool HandleFoundGenes(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Gene.Id.Length == 0)
                return true;
        }

        return false;
    }

    protected static bool HandleFoundProteins(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Protein = Set(criteria.Protein, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Protein.Id.Length == 0)
                return true;
        }

        return false;
    }

    protected static DonorCriteria Set(DonorCriteria criteria, int[] ids, bool? exclude = null)
    {
        return (criteria ?? new DonorCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id?.Value, ids), exclude) };
    }

    protected static ImagesCriteria Set(ImagesCriteria criteria, int[] ids, bool? exclude = null)
    {
        return (criteria ?? new ImagesCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id?.Value, ids), exclude) };
    }

    protected static SpecimensCriteria Set(SpecimensCriteria criteria, int[] ids, bool? exclude = null)
    {
        return (criteria ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id?.Value, ids), exclude) };
    }

    protected static GeneCriteria Set(GeneCriteria criteria, int[] ids, bool? exclude = null)
    {
        return (criteria ?? new GeneCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id?.Value, ids), exclude) };
    }

    protected static ProteinCriteria Set(ProteinCriteria criteria, int[] ids, bool? exclude = null)
    {
        return (criteria ?? new ProteinCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id?.Value, ids), exclude) };
    }

    protected static int[] Intersect(int[] a, int[] b)
    {
        if (a == null || a.Length == 0)
            return b;
        else
            return a.Intersect(b).ToArray();
    }

    protected static int[] Subtract(int[] a, int[] b)
    {
        if (a == null || a.Length == 0)
            return [];
        else
            return a.Except(b).ToArray();
    }

    protected void PersonalizeDonorsCriteria(PersonalSearchCriteria criteria)
    {
        var projectList = new List<string>();
        var task = _projectsIndexService.GetAccessibleProjects(criteria.UserId);
        task.Wait();
        var accessibleProjects = task.Result;
        
        var donorsCriteria = criteria.SearchCriteria.Donor ?? (criteria.SearchCriteria.Donor = new DonorCriteria());
        var projects = donorsCriteria.Project ?? (donorsCriteria.Project = new ValuesCriteria<string>());

        foreach (var accessibleProject in accessibleProjects.Rows)
        {
            var accepted = false;
            if (projects.Length > 0)
            {
                foreach (var project in projects.Value)
                {
                    if (accessibleProject.Name == project)
                    {
                        accepted = true;
                        break;
                    }
                }
            }
            else
            {
                accepted = true;
            }

            if (accepted)
            {
                projectList.Add(accessibleProject.Name);
            }
        }

        if (projectList.Count == 0)
            projectList.Add("-"); //force no matches
        
        donorsCriteria.Project = new ValuesCriteria<string>(projectList.ToArray());
    }
    
    private async Task<IDictionary<string, long>> AggregateFromDonors<TProp>(Expression<Func<DonorIndex, TProp>> property, PersonalSearchCriteria personalCriteria, DonorFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<DonorIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Images)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _donorsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromImages<TProp>(Expression<Func<ImageIndex, TProp>> property, PersonalSearchCriteria personalCriteria, ImageFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<ImageIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Donor)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _imagesIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSpecimens<TProp>(Expression<Func<SpecimenIndex, TProp>> property, PersonalSearchCriteria personalCriteria, SpecimenFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Donor)
            .AddExclusion(index => index.Images)
            .AddExclusion(index => index.Samples)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _specimensIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, PersonalSearchCriteria personalCriteria, GeneFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _genesIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromGeneExpressions<TProp>(Expression<Func<GeneExpressionIndex, TProp>> property, PersonalSearchCriteria personalCriteria, GeneExpressionFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<GeneExpressionIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Gene)
            .AddExclusion(index => index.Specimen);

        var result = await _geneExpressionsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromProteins<TProp>(Expression<Func<ProteinIndex, TProp>> property, PersonalSearchCriteria personalCriteria, ProteinFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();
        
        var criteria = personalCriteria.SearchCriteria;

        var query = new SearchQuery<ProteinIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _proteinsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromProteinExpressions<TProp>(Expression<Func<ProteinExpressionIndex, TProp>> property, PersonalSearchCriteria personalCriteria, ProteinExpressionFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<ProteinExpressionIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Protein)
            .AddExclusion(index => index.Specimen);

        var result = await _proteinExpressionsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSms<TProp>(Expression<Func<SmIndex, TProp>> property, PersonalSearchCriteria personalCriteria, SmFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<SmIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _smsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromCnvs<TProp>(Expression<Func<CnvIndex, TProp>> property, PersonalSearchCriteria personalCriteria, CnvFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<CnvIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _cnvsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSvs<TProp>(Expression<Func<SvIndex, TProp>> property, PersonalSearchCriteria personalCriteria, SvFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var criteria = personalCriteria.SearchCriteria;
        
        var query = new SearchQuery<SvIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _svsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }
}
