using Microsoft.Extensions.DependencyInjection;

namespace Unite.Indices.Context.Configuration.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddIndexServices(this IServiceCollection services)
    {
        services.AddTransient<IIndexService<Entities.Projects.ProjectIndex>, ProjectsIndexService>();
        services.AddTransient<IIndexService<Entities.Donors.DonorIndex>, DonorsIndexService>();
        services.AddTransient<IIndexService<Entities.Images.ImageIndex>, ImagesIndexService>();
        services.AddTransient<IIndexService<Entities.Specimens.SpecimenIndex>, SpecimensIndexService>();
        services.AddTransient<IIndexService<Entities.Genes.GeneIndex>, GenesIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.SsmIndex>, SsmsIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.CnvIndex>, CnvsIndexService>();
        services.AddTransient<IIndexService<Entities.Variants.SvIndex>, SvsIndexService>();
        
        return services;
    }
}
