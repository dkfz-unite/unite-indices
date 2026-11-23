using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Engine2.References;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using ProteinIndex = Unite.Indices.Entities.Proteins.ProteinIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Registries built from the index markup: documents, fields, entities, trees and keys.
/// </summary>
public class RegistryTests
{
    [Fact]
    public void All_searchable_documents_are_found()
    {
        Assert.Equal(
            ["cnv-profiles", "cnvs", "donors", "gene_expressions", "genes", "images", "projects", "protein_expressions", "proteins", "sms", "specimens", "svs"],
            SearchDocuments.All.Select(document => document.Name).Order().ToArray());
    }

    [Fact]
    public void Entity_trees_are_derived_from_references()
    {
        Assert.Equal("donors", SearchTrees.Main.Name);
        Assert.Equal(2, SearchTrees.Attachments.Single(entry => entry.Key.Name == "genes").Value);

        var entities = SearchEntities.All.Values.ToDictionary(entity => entity.Document.Name);

        Assert.Equal(["donors", "genes", "proteins", "specimens"], entities.Keys.Order().ToArray());
        Assert.Equal((1, "donors"), (entities["donors"].Level, entities["donors"].Root.Name));
        Assert.Equal((2, "donors"), (entities["specimens"].Level, entities["specimens"].Root.Name));
        Assert.Equal((1, "genes"), (entities["genes"].Level, entities["genes"].Root.Name));
        Assert.Equal((2, "genes"), (entities["proteins"].Level, entities["proteins"].Root.Name));
    }

    [Fact]
    public void Variants_refer_to_genes_and_proteins_through_affected_features()
    {
        var keys = DocumentKeys<SmIndex>.All.Values.ToDictionary(key => key.Entity.Document.Name, key => key.Name);

        Assert.Equal("specimens.id", keys["specimens"]);
        Assert.Equal("affectedFeatures.gene.id", keys["genes"]);
        Assert.Equal("affectedFeatures.transcript.feature.protein.id", keys["proteins"]);
        Assert.All(DocumentKeys<SmIndex>.All.Values, key => Assert.True(key.IsCollection));
    }

    [Fact]
    public void Parent_links_are_single_object_references()
    {
        var specimenDonor = DocumentKeys<SpecimenIndex>.All[typeof(DonorIndex)];
        var proteinGene = DocumentKeys<ProteinIndex>.All[typeof(GeneIndex)];

        Assert.Equal(("donor.id", false), (specimenDonor.Name, specimenDonor.IsCollection));
        Assert.Equal(("gene.id", false), (proteinGene.Name, proteinGene.IsCollection));
    }

    [Fact]
    public void Fields_of_the_old_filters_are_searchable()
    {
        Assert.Contains("clinicalData.sex", SearchFields<DonorIndex>.All.Keys);
        Assert.Contains("treatments.therapy", SearchFields<DonorIndex>.All.Keys);
        Assert.Contains("material.molecularData.idhStatus", SearchFields<SpecimenIndex>.All.Keys);
        Assert.Contains("xenograft.survivalDaysFrom", SearchFields<SpecimenIndex>.All.Keys);
        Assert.Contains("line.drugScreenings.dss", SearchFields<SpecimenIndex>.All.Keys);
        Assert.Contains("mr.wholeTumor", SearchFields<ImageIndex>.All.Keys);
        Assert.Contains("affectedFeatures.gene.symbol", SearchFields<SmIndex>.All.Keys);
        Assert.Contains("affectedFeatures.effects.impact", SearchFields<SmIndex>.All.Keys);
        Assert.Contains("data.sms", SearchFields<GeneIndex>.All.Keys);
    }

    [Fact]
    public void Access_control_and_statistics_are_not_searchable()
    {
        Assert.DoesNotContain(SearchFields<ProjectIndex>.All.Keys, name => name.StartsWith("users") || name == "isPublic" || name == "isAccessible");
        Assert.DoesNotContain(SearchFields<DonorIndex>.All.Keys, name => name.StartsWith("projects"));
        Assert.DoesNotContain(SearchFields<DonorIndex>.All.Keys, name => name.StartsWith("stats"));
    }

    [Fact]
    public void Exact_text_matches_use_the_keyword_sub_field()
    {
        var field = (SearchField<SmIndex, string>)SearchFields<SmIndex>.Find("type");

        Assert.Contains("Suffix(\"keyword\")", field.TermProperty.ToString());
    }
}
