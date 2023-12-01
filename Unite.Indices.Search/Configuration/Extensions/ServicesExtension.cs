using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Services;

namespace Unite.Indices.Search.Configuration.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddSearchEngine(this IServiceCollection services)
    {
        services.AddTransient<IIndexService<Entities.Donors.DonorIndex>, DonorsIndexService>();
        services.AddTransient<IIndexService<Entities.Images.ImageIndex>, ImagesIndexService>();
        services.AddTransient<IIndexService<Entities.Specimens.SpecimenIndex>, SpecimensIndexService>();
        services.AddTransient<IIndexService<Entities.Genes.GeneIndex>, GenesIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.VariantIndex>, VariantsIndexService>();

        return services;
    }

    public static IServiceCollection AddSearchServices(this IServiceCollection services)
    {
        services.AddTransient<IDonorsSearchService, DonorsSearchService>();
        services.AddTransient<IImagesSearchService, ImagesSearchService>();
        services.AddTransient<ISpecimensSearchService, SpecimensSearchService>();
        services.AddTransient<IGenesSearchService, GenesSearchService>();
        services.AddTransient<IVariantsSearchService, VariantsSearchService>();

        return services;
    }
}
