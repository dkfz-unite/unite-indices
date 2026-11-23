using Unite.Indices.Search.Engine2;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Tests.Support;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Stage plans: the joins of the old search services (Services/*SearchService.cs) and the stage order of Engine2.
/// </summary>
public class PlanTests
{
    /// <summary>
    /// Target, source document, key the source is joined by (* identity), target entity the keys are bridged to.
    /// The old engine joined the same documents through the same IDs; where it joined by image IDs
    /// (donors, specimens), Engine2 joins by the donors/specimens the images belong to, which selects the same targets.
    /// </summary>
    public static TheoryData<string, string, string, string> OldEngineJoins => new()
    {
        // ImagesSearchService
        { "images", "donors", "id*", null },
        { "images", "specimens", "id*", null },
        { "images", "genes", "specimens.id", null },
        { "images", "gene_expressions", "specimen.id", null },
        { "images", "proteins", "specimens.id", null },
        { "images", "protein_expressions", "specimen.id", null },
        { "images", "sms", "specimens.id", null },
        { "images", "cnvs", "specimens.id", null },
        { "images", "svs", "specimens.id", null },
        { "images", "cnv-profiles", "specimen.id", null },
        // DonorsSearchService
        { "donors", "images", "specimens.id", null },
        { "donors", "specimens", "id*", null },
        { "donors", "genes", "specimens.id", null },
        { "donors", "gene_expressions", "specimen.id", null },
        { "donors", "proteins", "specimens.id", null },
        { "donors", "protein_expressions", "specimen.id", null },
        { "donors", "sms", "specimens.id", null },
        { "donors", "cnv-profiles", "specimen.id", null },
        // SpecimensSearchService
        { "specimens", "donors", "id*", null },
        { "specimens", "images", "specimens.id", null },
        { "specimens", "genes", "specimens.id", null },
        { "specimens", "gene_expressions", "specimen.id", null },
        { "specimens", "proteins", "specimens.id", null },
        { "specimens", "sms", "specimens.id", null },
        { "specimens", "cnv-profiles", "specimen.id", null },
        // GenesSearchService
        { "genes", "donors", "specimens.id", null },
        { "genes", "images", "specimens.id", null },
        { "genes", "specimens", "id*", null },
        { "genes", "proteins", "gene.id", null },
        { "genes", "sms", "affectedFeatures.gene.id", null },
        { "genes", "cnvs", "affectedFeatures.gene.id", null },
        { "genes", "svs", "affectedFeatures.gene.id", null },
        { "genes", "cnv-profiles", "specimen.id", null },
        // ProteinsSearchService
        { "proteins", "donors", "specimens.id", null },
        { "proteins", "images", "specimens.id", null },
        { "proteins", "specimens", "id*", null },
        { "proteins", "genes", "id*", null },
        { "proteins", "sms", "affectedFeatures.transcript.feature.protein.id", null },
        { "proteins", "cnvs", "affectedFeatures.transcript.feature.protein.id", null },
        { "proteins", "svs", "affectedFeatures.transcript.feature.protein.id", null },
        { "proteins", "cnv-profiles", "specimen.id", null },
        // SmsSearchService, CnvsSearchService, SvsSearchService
        { "sms", "donors", "specimens.id", null },
        { "sms", "images", "specimens.id", null },
        { "sms", "specimens", "id*", null },
        { "sms", "genes", "id*", null },
        { "sms", "gene_expressions", "specimen.id", null },
        { "sms", "proteins", "id*", null },
        { "sms", "protein_expressions", "specimen.id", null },
        { "cnvs", "genes", "id*", null },
        { "svs", "proteins", "id*", null },
        // ProjectsSearchService: specimen-level documents through project.Donors[].Specimens[], bridged to donors
        { "projects", "donors", "id*", null },
        { "projects", "images", "donor.id", null },
        { "projects", "specimens", "donor.id", null },
        { "projects", "genes", "specimens.id", "donors" },
        { "projects", "gene_expressions", "specimen.id", "donors" },
        { "projects", "proteins", "specimens.id", "donors" },
        { "projects", "protein_expressions", "specimen.id", "donors" },
        { "projects", "sms", "specimens.id", "donors" },
        { "projects", "cnvs", "specimens.id", "donors" },
        { "projects", "svs", "specimens.id", "donors" },
        // Engine2 only: project criteria for targets below donors, bridged down to specimens
        { "genes", "projects", "donors.id", "specimens" },
        { "sms", "projects", "donors.id", "specimens" },
    };

    [Theory]
    [MemberData(nameof(OldEngineJoins))]
    public void Joins_documents_as_the_old_engine(string target, string source, string join, string bridge)
    {
        var stage = Plan(target, source).Single();

        Assert.Equal(join, stage.Join.Name + (stage.Join.IsIdentity ? "*" : ""));
        Assert.Equal(bridge, stage.Bridge?.Document.Name);
    }

    [Fact]
    public void Gene_search_joins_gene_expressions_by_gene()
    {
        var stage = Plan("genes", "gene_expressions").Single();

        Assert.Equal("gene.id", stage.Join.Name);
    }

    [Fact]
    public void Stages_run_from_the_main_tree_root_to_attached_trees_and_then_to_other_documents()
    {
        var order = Plan("images", SearchDocuments.All.Select(document => document.Name).Where(name => name != "images").ToArray())
            .Select(stage => stage.Stage.Document.Name)
            .ToList();

        Assert.True(order.IndexOf("donors") < order.IndexOf("projects"));
        Assert.True(order.IndexOf("projects") < order.IndexOf("specimens"));
        Assert.True(order.IndexOf("specimens") < order.IndexOf("genes"));
        Assert.True(order.IndexOf("genes") < order.IndexOf("proteins"));

        foreach (var fact in new[] { "gene_expressions", "protein_expressions", "sms", "cnvs", "svs", "cnv-profiles" })
            Assert.True(order.IndexOf("proteins") < order.IndexOf(fact), $"proteins before {fact}");
    }

    [Fact]
    public void Entity_stages_also_collect_their_own_keys()
    {
        var genes = Plan("images", "genes").Single();

        Assert.Equal(["specimens", "genes"], genes.Collects.Select(key => key.Entity.Document.Name).ToArray());
    }

    [Fact]
    public void Include_stage_runs_before_the_exclude_stage_of_the_same_document()
    {
        var criteria = FakeIndices.Criteria("""{ "sms": { "include": { "nodes": [] }, "exclude": { "nodes": [] } } }""");
        var plan = new FakeIndices().SearchService<Unite.Indices.Entities.Images.ImageIndex>().Plan(criteria);

        Assert.Equal([false, true], plan.Select(stage => stage.IsExclusion).ToArray());
    }


    /// <summary>
    /// Plan of a search of the target with (empty) include criteria on the source documents.
    /// </summary>
    private static IReadOnlyList<SearchStage> Plan(string target, params string[] sources)
    {
        var json = "{" + string.Join(",", sources.Select(source => $$"""
            "{{source}}": { "include": { "nodes": [] } }
            """)) + "}";

        var service = Activator.CreateInstance(typeof(SearchService<>).MakeGenericType(SearchDocuments.Find(target).Type), [new FakeIndices().Services]);
        var plan = service.GetType().GetMethod(nameof(SearchService<object>.Plan));

        return (IReadOnlyList<SearchStage>)plan.Invoke(service, [FakeIndices.Criteria(json)]);
    }
}
