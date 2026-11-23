using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine2;
using Unite.Indices.Search.Engine2.Criteria;
using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using CnvProfileIndex = Unite.Indices.Entities.CnvProfiles.CnvProfileIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneExpressionIndex = Unite.Indices.Entities.Genes.GeneExpressionIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using ProteinExpressionIndex = Unite.Indices.Entities.Proteins.ProteinExpressionIndex;
using ProteinIndex = Unite.Indices.Entities.Proteins.ProteinIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;

namespace Unite.Indices.Search.Tests.Support;

/// <summary>
/// Fake index services of all searchable documents, with search services using them.
/// </summary>
public class FakeIndices
{
    public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public FakeIndexService<ProjectIndex> Projects { get; } = new();
    public FakeIndexService<DonorIndex> Donors { get; } = new();
    public FakeIndexService<ImageIndex> Images { get; } = new();
    public FakeIndexService<SpecimenIndex> Specimens { get; } = new();
    public FakeIndexService<GeneIndex> Genes { get; } = new();
    public FakeIndexService<GeneExpressionIndex> GeneExpressions { get; } = new();
    public FakeIndexService<ProteinIndex> Proteins { get; } = new();
    public FakeIndexService<ProteinExpressionIndex> ProteinExpressions { get; } = new();
    public FakeIndexService<SmIndex> Sms { get; } = new();
    public FakeIndexService<CnvIndex> Cnvs { get; } = new();
    public FakeIndexService<SvIndex> Svs { get; } = new();
    public FakeIndexService<CnvProfileIndex> CnvProfiles { get; } = new();

    public IServiceProvider Services { get; }


    public FakeIndices()
    {
        Services = new ServiceCollection()
            .AddSingleton<IIndexService<ProjectIndex>>(Projects)
            .AddSingleton<IIndexService<DonorIndex>>(Donors)
            .AddSingleton<IIndexService<ImageIndex>>(Images)
            .AddSingleton<IIndexService<SpecimenIndex>>(Specimens)
            .AddSingleton<IIndexService<GeneIndex>>(Genes)
            .AddSingleton<IIndexService<GeneExpressionIndex>>(GeneExpressions)
            .AddSingleton<IIndexService<ProteinIndex>>(Proteins)
            .AddSingleton<IIndexService<ProteinExpressionIndex>>(ProteinExpressions)
            .AddSingleton<IIndexService<SmIndex>>(Sms)
            .AddSingleton<IIndexService<CnvIndex>>(Cnvs)
            .AddSingleton<IIndexService<SvIndex>>(Svs)
            .AddSingleton<IIndexService<CnvProfileIndex>>(CnvProfiles)
            .BuildServiceProvider();
    }


    public SearchService<T> SearchService<T>() where T : class => new(Services);

    public static CriteriaContainer Criteria(string json) => JsonSerializer.Deserialize<CriteriaContainer>(json, JsonOptions);
}
