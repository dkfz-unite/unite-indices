using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria.Models;
using static Unite.Indices.Search.Tests.Integration.Searches;

namespace Unite.Indices.Search.Tests.Integration;

/// <summary>
/// The old search services (Services/*SearchService.cs) and Engine2 on the same index with equivalent criteria:
/// same totals and IDs for every target whose old service supports the criteria, for root users and for members.
/// </summary>
[Collection(nameof(ElasticCollection))]
public class ComparisonTests(ElasticFixture elastic)
{
    /// <summary>
    /// Equivalent criteria: old SearchCriteria and Engine2 JSON, with the documents the criteria are on.
    /// </summary>
    public record Case(string Name, Func<SearchCriteria> Old, string New, params string[] Sources)
    {
        /// <summary>
        /// Targets the case applies to (null: all).
        /// </summary>
        public string[] Targets { get; init; }

        public override string ToString() => Name;
    }

    private static Range<T> Between<T>(T from, T to) => new() { From = from, To = to };

    public static readonly Case[] Cases =
    [
        new("no criteria", () => new(), "{}"),
        new("donor sex", () => new() { Donor = new DonorCriteria { Sex = new(["Male"]) } },
            Criteria(Document("donors", [Text("clinicalData.sex", "Male")])), "donors"),
        new("donor age", () => new() { Donor = new DonorCriteria { Age = new() { Value = Between<int?>(35, 58) } } },
            Criteria(Document("donors", [Range("int", "clinicalData.age", 35, 58)])), "donors"),
        // Personalization adds a positive project filter to donor criteria, so the old services never treat donor criteria as exclusive:
        // "not male" selects data of donors that are not male (a negated include), not data without male donors (an exclusion).
        // Only the projects service is not personalized and excludes. For single donors (donors, images, specimens) both are the same.
        new("not donor sex", () => new() { Donor = new DonorCriteria { Sex = new(["Male"], not: true) } },
            Criteria(Document("donors", ["""{ "kind": "group", "negate": true, "nodes": [ """ + Text("clinicalData.sex", "Male") + " ] }"])), "donors")
            { Targets = ["donors", "images", "specimens", "genes", "proteins", "sms", "cnvs", "svs"] },
        new("not donor sex, projects", () => new() { Donor = new DonorCriteria { Sex = new(["Male"], not: true) } },
            Criteria(Document("donors", exclude: [Text("clinicalData.sex", "Male")])), "donors")
            { Targets = ["projects"] },
        new("mr whole tumor", () => new() { Mr = new MrImageCriteria { WholeTumor = new() { Value = Between<double?>(20, 60) } } },
            Criteria(Document("images", [Range("double", "mr.wholeTumor", 20, 60)])), "images"),
        new("specimen type", () => new() { Specimen = new SpecimensCriteria { SpecimenType = new(["Line"]) } },
            Criteria(Document("specimens", [Text("type", "Line")])), "specimens"),
        new("material idh", () => new() { Material = new MaterialCriteria { IdhStatus = new() { Value = true } } },
            Criteria(Document("specimens", [Bool("material.molecularData.idhStatus", true)])), "specimens"),
        new("gene symbol", () => new() { Gene = new GeneCriteria { Symbol = new(["EGFR"]) } },
            Criteria(Document("genes", [Text("symbol", "EGFR")])), "genes"),
        new("gene expression", () => new() { Gene = new GeneCriteria { Expression = new() { Value = Between<double?>(1, null) } } },
            Criteria(Document("gene_expressions", [Range("double", "expression", 1)])), "gene_expressions"),
        new("protein symbol", () => new() { Protein = new ProteinCriteria { Symbol = new(["EGFR"]) } },
            Criteria(Document("proteins", [Text("symbol", "EGFR")])), "proteins"),
        new("sm type", () => new() { Sm = new SmCriteria { Type = new(["SNV"]) } },
            Criteria(Document("sms", [Text("type", "SNV")])), "sms"),
        new("not sm type", () => new() { Sm = new SmCriteria { Type = new(["SNV"], not: true) } },
            Criteria(Document("sms", exclude: [Text("type", "SNV")])), "sms"),
        new("sm gene", () => new() { Sm = new SmCriteria { Gene = new(["IDH1"]) } },
            Criteria(Document("sms", [Text("affectedFeatures.gene.symbol", "IDH1")])), "sms"),
        new("sm and specimen type", () => new() { Sm = new SmCriteria { Type = new(["SNV"]) }, Specimen = new SpecimensCriteria { SpecimenType = new(["Line"]) } },
            Criteria(Document("sms", [Text("type", "SNV")]), Document("specimens", [Text("type", "Line")])), "sms", "specimens"),
        new("sm and donor sex", () => new() { Sm = new SmCriteria { Type = new(["DEL"]) }, Donor = new DonorCriteria { Sex = new(["Female"]) } },
            Criteria(Document("sms", [Text("type", "DEL")]), Document("donors", [Text("clinicalData.sex", "Female")])), "sms", "donors"),
        new("cnv type", () => new() { Cnv = new CnvCriteria { Type = new(["Loss"]) } },
            Criteria(Document("cnvs", [Text("type", "Loss")])), "cnvs"),
        new("sv type", () => new() { Sv = new SvCriteria { Type = new(["DEL"]) } },
            Criteria(Document("svs", [Text("type", "DEL")])), "svs"),
        new("project name", () => new() { Project = new ProjectCriteria { Name = new(["Public"]) } },
            Criteria(Document("projects", [Text("name", "Public")])), "projects"),
    ];

    private const string ProjectsNotPersonalized =
        "The old projects service is not personalized: criteria on other documents also find projects the user has no access to. " +
        "Engine2 lists all projects, but finds them by accessible data only.";

    private const string ProjectsNotCorrelated =
        "The old projects service matches donor IDs and specimen IDs on different elements of donors[]: P1 has a female donor (D2) and a DEL specimen of another donor (S1). " +
        "Engine2 keeps the same-specimen correlation (only S5 of D4 in P3 has both).";

    /// <summary>
    /// Deliberate differences: (target, case, root) → reason, IDs of the old engine, IDs of Engine2.
    /// </summary>
    public static readonly Dictionary<(string Target, string Case, bool Root), (string Reason, string[] Old, string[] New)> Differences = new()
    {
        [("projects", "sm and donor sex", true)] = (ProjectsNotCorrelated, ["1", "3"], ["3"]),
        [("projects", "sm and donor sex", false)] = (ProjectsNotCorrelated + " " + ProjectsNotPersonalized, ["1", "3"], []),
        [("projects", "sm gene", false)] = (ProjectsNotPersonalized, ["1", "3"], ["1"]),
        [("projects", "material idh", false)] = (ProjectsNotPersonalized, ["1", "2", "3"], ["1", "2"]),
        [("projects", "not sm type", false)] = (ProjectsNotPersonalized, ["3"], []),
        [("projects", "not donor sex, projects", false)] = (ProjectsNotPersonalized, ["3"], []),
    };

    private static readonly Dictionary<string, Func<IElasticOptions, PersonalSearchCriteria, Task<IEnumerable<object>>>> OldServices = new()
    {
        ["projects"] = async (options, criteria) => (await new ProjectsSearchService(options).Search(criteria)).Rows,
        ["donors"] = async (options, criteria) => (await new DonorsSearchService(options).Search(criteria)).Rows,
        ["images"] = async (options, criteria) => (await new ImagesSearchService(options).Search(criteria)).Rows,
        ["specimens"] = async (options, criteria) => (await new SpecimensSearchService(options).Search(criteria)).Rows,
        ["genes"] = async (options, criteria) => (await new GenesSearchService(options).Search(criteria)).Rows,
        ["proteins"] = async (options, criteria) => (await new ProteinsSearchService(options).Search(criteria)).Rows,
        ["sms"] = async (options, criteria) => (await new SmsSearchService(options).Search(criteria)).Rows,
        ["cnvs"] = async (options, criteria) => (await new CnvsSearchService(options).Search(criteria)).Rows,
        ["svs"] = async (options, criteria) => (await new SvsSearchService(options).Search(criteria)).Rows,
    };

    /// <summary>
    /// Whether the old service of the target used criteria on the source (joins of <see cref="PlanTests.OldEngineJoins"/>, or the target itself).
    /// </summary>
    private static bool OldServiceSupports(string target, string source) =>
        target == source || PlanTests.OldEngineJoins.Any(join => (string)join[0] == target && (string)join[1] == source && !(target is "genes" or "sms" && source == "projects"));

    public static TheoryData<string, string, bool> Comparisons()
    {
        var data = new TheoryData<string, string, bool>();

        foreach (var target in OldServices.Keys)
            foreach (var @case in Cases.Where(@case => (@case.Targets?.Contains(target) ?? true) && @case.Sources.All(source => OldServiceSupports(target, source))))
                foreach (var root in new[] { true, false })
                    data.Add(target, @case.Name, root);

        return data;
    }


    [SkippableTheory]
    [MemberData(nameof(Comparisons))]
    public async Task Engine2_finds_what_the_old_engine_finds(string target, string name, bool root)
    {
        Skip.If(elastic.Unavailable != null, elastic.Unavailable);

        var @case = Cases.Single(@case => @case.Name == name);
        var criteria = @case.Old() with { From = 0, Size = 100 };

        var old = Ids(await OldServices[target](elastic.Options, new PersonalSearchCriteria(root ? 1 : Dataset.Member, root, criteria)));
        var found = await Searches.Search(elastic.Services, target, @case.New, root ? SearchScope.All : SearchScope.ForUser(Dataset.Member));

        Assert.Equal(found.Ids.Length, found.Total);

        if (Differences.TryGetValue((target, name, root), out var difference))
        {
            Assert.Equal(difference.Old, old.Order());
            Assert.Equal(difference.New, found.Ids.Order());
        }
        else
        {
            Assert.Equal(old.Order(), found.Ids.Order());
        }
    }
}
