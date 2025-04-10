using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Services;

namespace Unite.Indices.Search.Configuration.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddSearchEngine(this IServiceCollection services)
    {
        services.AddTransient<IIndexService<Entities.Projects.ProjectIndex>, ProjectsIndexService>();
        services.AddTransient<IIndexService<Entities.Donors.DonorIndex>, DonorsIndexService>();
        services.AddTransient<IIndexService<Entities.Images.ImageIndex>, ImagesIndexService>();
        services.AddTransient<IIndexService<Entities.Specimens.SpecimenIndex>, SpecimensIndexService>();
        services.AddTransient<IIndexService<Entities.Genes.GeneIndex>, GenesIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.SmIndex>, SmsIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.CnvIndex>, CnvsIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.SvIndex>, SvsIndexService>();

        return services;
    }

    public static IServiceCollection AddSearchServices(this IServiceCollection services)
    {
        services.AddTransient<ISearchService<Entities.Projects.ProjectIndex>, ProjectsSearchService>();
        services.AddTransient<ISearchService<Entities.Donors.DonorIndex>, DonorsSearchService>();
        services.AddTransient<ISearchService<Entities.Images.ImageIndex>, ImagesSearchService>();
        services.AddTransient<ISearchService<Entities.Specimens.SpecimenIndex>, SpecimensSearchService>();
        services.AddTransient<ISearchService<Entities.Genes.GeneIndex>, GenesSearchService>();
        services.AddTransient<ISearchService<Entities.Variants.SmIndex>, SmsSearchService>();
        services.AddTransient<ISearchService<Entities.Variants.CnvIndex>, CnvsSearchService>();
        services.AddTransient<ISearchService<Entities.Variants.SvIndex>, SvsSearchService>();

        return services;
    }
}
