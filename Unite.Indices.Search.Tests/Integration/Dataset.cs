using Nest;
using Unite.Indices.Context;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Entities.Basic;
using Unite.Indices.Entities.Basic.Projects;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Entities.Basic.Specimens.Constants;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Entities.Basic.Donors;
using Unite.Indices.Entities.Basic.Omics.Variants;

namespace Unite.Indices.Search.Tests.Integration;

/// <summary>
/// Small dataset covering every searchable document and every reference.
/// <code>
/// Projects  P1 "Public" (public): D1, D2    P2 "Private" (user 7): D3    P3 "Hidden" (user 99): D4
/// Donors    D1 Male 40: S1 Material IDH+, S2 Line    D2 Female 60: S3 Material IDH-    D3 Male 55: S4 Material IDH+    D4 Female 30: S5 Material IDH+
/// Images    I1 MR 10 (D1, S1)    I2 MR 50 (D2, S3)    I3 MR 30 (D3, S4)
/// Genes     G1 EGFR chr7    G2 TP53 chr17    G3 IDH1 chr2;  protein PR1 of G1
/// Exprs     E1 G1 S1 5.0    E2 G2 S2 2.0    E3 G1 S3 0.5;  PE1 PR1 S1 3.0
/// SMs       SM1 SNV chr7 S1 (G1, PR1)    SM2 SNV chr17 S4 (G2)    SM3 DEL chr2 S1+S5 (G3)
/// CNVs      CNV1 Loss chr7 S3 (G1)     SVs  SV1 DEL chr17 S4 (G2)     CNV profiles  CP1 chr7 S1
/// </code>
/// </summary>
public static class Dataset
{
    public const int Member = 7;
    public const int Stranger = 99;

    private record Specimen(int Id, int Donor, string Type, bool? Idh);

    private static readonly (int Id, string Name, bool IsPublic, int[] Users, int[] Donors)[] Projects =
    [
        (1, "Public", true, [], [1, 2]),
        (2, "Private", false, [Member], [3]),
        (3, "Hidden", false, [Stranger], [4]),
    ];

    private static readonly (int Id, string Sex, int Age, int Project)[] Donors =
    [
        (1, "Male", 40, 1),
        (2, "Female", 60, 1),
        (3, "Male", 55, 2),
        (4, "Female", 30, 3),
    ];

    private static readonly Specimen[] Specimens =
    [
        new(1, 1, SpecimenType.Material, true),
        new(2, 1, SpecimenType.Line, null),
        new(3, 2, SpecimenType.Material, false),
        new(4, 3, SpecimenType.Material, true),
        new(5, 4, SpecimenType.Material, true),
    ];

    private static readonly (int Id, int Donor, int Specimen, double WholeTumor)[] Images =
    [
        (1, 1, 1, 10),
        (2, 2, 3, 50),
        (3, 3, 4, 30),
    ];

    private static readonly (int Id, string Symbol, string Chromosome, int Start)[] Genes =
    [
        (1, "EGFR", "7", 55_000_000),
        (2, "TP53", "17", 7_600_000),
        (3, "IDH1", "2", 208_000_000),
    ];

    private static readonly (int Gene, int Specimen, double Expression)[] GeneExpressions =
    [
        (1, 1, 5.0),
        (2, 2, 2.0),
        (1, 3, 0.5),
    ];

    private static readonly (int Id, int Gene, string Symbol)[] Proteins = [(1, 1, "EGFR")];

    private static readonly (int Protein, int Specimen, double Expression)[] ProteinExpressions = [(1, 1, 3.0)];

    private static readonly (int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene, int? Protein)[] Sms =
    [
        (1, "SNV", "7", 55_000_100, [1], 1, 1),
        (2, "SNV", "17", 7_600_100, [4], 2, null),
        (3, "DEL", "2", 208_000_100, [1, 5], 3, null),
    ];

    private static readonly (int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene)[] Cnvs =
    [
        (1, "Loss", "7", 55_000_000, [3], 1),
    ];

    private static readonly (int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene)[] Svs =
    [
        (1, "DEL", "17", 7_600_000, [4], 2),
    ];


    public static async Task Index(IElasticOptions options)
    {
        await Index(new ProjectsIndexService(options), Projects.Select(CreateProject));
        await Index(new DonorsIndexService(options), Donors.Select(CreateDonor));
        await Index(new SpecimensIndexService(options), Specimens.Select(CreateSpecimen));
        await Index(new ImagesIndexService(options), Images.Select(CreateImage));
        await Index(new GenesIndexService(options), Genes.Select(CreateGene));
        await Index(new GeneExpressionsIndexService(options), GeneExpressions.Select(CreateGeneExpression));
        await Index(new ProteinsIndexService(options), Proteins.Select(CreateProtein));
        await Index(new ProteinExpressionsIndexService(options), ProteinExpressions.Select(CreateProteinExpression));
        await Index(new SmsIndexService(options), Sms.Select(CreateSm));
        await Index(new CnvsIndexService(options), Cnvs.Select(CreateCnv));
        await Index(new SvsIndexService(options), Svs.Select(CreateSv));
        await Index(new CnvProfilesIndexService(options), [CreateCnvProfile()]);

        var client = new ElasticClient(new ConnectionSettings(new Uri(options.Host)).EnableApiVersioningHeader());
        var refresh = await client.Indices.RefreshAsync(Nest.Indices.All);

        if (!refresh.IsValid)
            throw new InvalidOperationException(refresh.DebugInformation);
    }

    private static async Task Index<T>(Context.IndexService<T> service, IEnumerable<T> documents) where T : class
    {
        await service.CreateIndex();
        await service.AddRange(documents.ToArray());
    }


    private static Specimen SpecimenOf(int id) => Specimens.Single(specimen => specimen.Id == id);

    private static (int Id, string Sex, int Age, int Project) DonorOf(int id) => Donors.Single(donor => donor.Id == id);

    private static T Nav<T>(int id) where T : SpecimenNavIndex, new() =>
        new() { Id = id, ReferenceId = $"S{id}", Type = SpecimenOf(id).Type };

    private static T[] ImagesOf<T>(Func<(int Id, int Donor, int Specimen, double WholeTumor), bool> predicate) where T : ImageNavIndex, new() =>
        Images.Where(predicate).Select(image => new T { Id = image.Id, ReferenceId = $"I{image.Id}", Type = "MR" }).ToArray();

    private static Entities.Basic.Omics.GeneIndex GeneOf(int id)
    {
        var gene = Genes.Single(gene => gene.Id == id);
        return new() { Id = gene.Id, Symbol = gene.Symbol, Chromosome = gene.Chromosome, Start = gene.Start };
    }

    /// <summary>
    /// Donors of the specimens (for document stats).
    /// </summary>
    private static int DonorsOf(IEnumerable<int> specimens) => specimens.Select(id => SpecimenOf(id).Donor).Distinct().Count();

    private static int[] SpecimensOfGene(int gene) =>
        Sms.Where(sm => sm.Gene == gene).SelectMany(sm => sm.Specimens)
            .Concat(Cnvs.Where(cnv => cnv.Gene == gene).SelectMany(cnv => cnv.Specimens))
            .Concat(Svs.Where(sv => sv.Gene == gene).SelectMany(sv => sv.Specimens))
            .Concat(GeneExpressions.Where(expression => expression.Gene == gene).Select(expression => expression.Specimen))
            .Distinct().Order().ToArray();

    private static int GenesOfSpecimens(IEnumerable<int> specimens) =>
        Genes.Count(gene => SpecimensOfGene(gene.Id).Intersect(specimens).Any());


    private static Entities.Projects.ProjectIndex CreateProject((int Id, string Name, bool IsPublic, int[] Users, int[] Donors) project) => new()
    {
        Id = project.Id,
        Name = project.Name,
        IsPublic = project.IsPublic,
        Users = project.Users.Select(user => new DataUserNavIndex { Id = user, UserId = user }).ToArray(),
        Stats = new() { Donors = new() { Number = project.Donors.Length } },
        Data = new DataIndex { Donors = true },
        Donors = project.Donors.Select(donor => new Entities.Projects.DonorIndex
        {
            Id = donor,
            ReferenceId = $"D{donor}",
            Specimens = Specimens.Where(specimen => specimen.Donor == donor).Select(specimen => Nav<Entities.Projects.SpecimenIndex>(specimen.Id)).ToArray(),
            Images = ImagesOf<Entities.Projects.ImageIndex>(image => image.Donor == donor)
        }).ToArray()
    };

    private static Entities.Donors.DonorIndex CreateDonor((int Id, string Sex, int Age, int Project) donor)
    {
        var specimens = Specimens.Where(specimen => specimen.Donor == donor.Id).Select(specimen => specimen.Id).ToArray();
        var project = Projects.Single(project => project.Id == donor.Project);

        return new()
        {
            Id = donor.Id,
            ReferenceId = $"D{donor.Id}",
            ClinicalData = new ClinicalDataIndex { Sex = donor.Sex, Age = donor.Age },
            Projects = [new ProjectNavIndex { Id = project.Id, Name = project.Name }],
            Specimens = specimens.Select(Nav<Entities.Donors.SpecimenIndex>).ToArray(),
            Images = ImagesOf<Entities.Donors.ImageIndex>(image => image.Donor == donor.Id),
            Stats = new() { Genes = GenesOfSpecimens(specimens) },
            Data = new DataIndex { Donors = true, Clinical = true }
        };
    }

    private static Entities.Specimens.SpecimenIndex CreateSpecimen(Specimen specimen)
    {
        var index = new Entities.Specimens.SpecimenIndex
        {
            Id = specimen.Id,
            ReferenceId = $"S{specimen.Id}",
            Type = specimen.Type,
            Donor = new Entities.Specimens.DonorIndex { Id = specimen.Donor, ReferenceId = $"D{specimen.Donor}" },
            Images = ImagesOf<Entities.Specimens.ImageIndex>(image => image.Specimen == specimen.Id),
            Stats = new() { Genes = GenesOfSpecimens([specimen.Id]) },
            Data = new DataIndex { Materials = specimen.Type == SpecimenType.Material, Lines = specimen.Type == SpecimenType.Line }
        };

        if (specimen.Type == SpecimenType.Material)
            index.Material = new MaterialIndex { Id = specimen.Id, ReferenceId = $"S{specimen.Id}", MolecularData = new MolecularDataIndex { IdhStatus = specimen.Idh } };
        else
            index.Line = new LineIndex { Id = specimen.Id, ReferenceId = $"S{specimen.Id}" };

        return index;
    }

    private static Entities.Images.ImageIndex CreateImage((int Id, int Donor, int Specimen, double WholeTumor) image) => new()
    {
        Id = image.Id,
        ReferenceId = $"I{image.Id}",
        Type = "MR",
        Mr = new MrImageIndex { Id = image.Id, WholeTumor = image.WholeTumor },
        Donor = new Entities.Images.DonorIndex { Id = image.Donor, ReferenceId = $"D{image.Donor}" },
        Specimens = [Nav<Entities.Images.SpecimenIndex>(image.Specimen)],
        Stats = new() { Genes = GenesOfSpecimens([image.Specimen]) },
        Data = new DataIndex { Mrs = true }
    };

    private static Entities.Genes.GeneIndex CreateGene((int Id, string Symbol, string Chromosome, int Start) gene) => new()
    {
        Id = gene.Id,
        Symbol = gene.Symbol,
        Biotype = "protein_coding",
        Chromosome = gene.Chromosome,
        Start = gene.Start,
        End = gene.Start + 1000,
        Specimens = SpecimensOfGene(gene.Id).Select(Nav<Entities.Genes.SpecimenIndex>).ToArray(),
        Stats = new() { Donors = DonorsOf(SpecimensOfGene(gene.Id)) },
        Data = new DataIndex { Exp = true }
    };

    private static Entities.Genes.GeneExpressionIndex CreateGeneExpression((int Gene, int Specimen, double Expression) expression) => new()
    {
        Id = $"{expression.Gene}-{expression.Specimen}",
        Gene = new Entities.Basic.Omics.GeneNavIndex { Id = expression.Gene },
        Specimen = Nav<SpecimenNavIndex>(expression.Specimen),
        Expression = expression.Expression
    };

    private static Entities.Proteins.ProteinIndex CreateProtein((int Id, int Gene, string Symbol) protein) => new()
    {
        Id = protein.Id,
        Symbol = protein.Symbol,
        Chromosome = GeneOf(protein.Gene).Chromosome,
        Gene = new Entities.Proteins.GeneIndex { Id = protein.Gene, Symbol = GeneOf(protein.Gene).Symbol },
        Specimens = ProteinExpressions.Where(expression => expression.Protein == protein.Id)
            .Select(expression => Nav<Entities.Proteins.SpecimenIndex>(expression.Specimen)).ToArray(),
        Stats = new() { Donors = 1 },
        Data = new DataIndex { Prot = true }
    };

    private static Entities.Proteins.ProteinExpressionIndex CreateProteinExpression((int Protein, int Specimen, double Expression) expression) => new()
    {
        Id = $"{expression.Protein}-{expression.Specimen}",
        Protein = new Entities.Basic.Omics.ProteinNavIndex { Id = expression.Protein },
        Specimen = Nav<SpecimenNavIndex>(expression.Specimen),
        Expression = expression.Expression
    };

    private static AffectedFeatureIndex[] Affects(int gene, int? protein) =>
    [
        new()
        {
            Gene = GeneOf(gene),
            Transcript = protein == null ? null : new AffectedTranscriptIndex
            {
                Feature = new Entities.Basic.Omics.TranscriptIndex { Id = protein.Value, Protein = new Entities.Basic.Omics.ProteinIndex { Id = protein.Value, Symbol = Proteins.Single(entry => entry.Id == protein).Symbol } }
            }
        }
    ];

    private static Entities.Variants.SmIndex CreateSm((int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene, int? Protein) sm) => new()
    {
        Id = sm.Id,
        Type = sm.Type,
        Chromosome = sm.Chromosome,
        Start = sm.Start,
        End = sm.Start + 1,
        Length = 1,
        AffectedFeatures = Affects(sm.Gene, sm.Protein),
        Specimens = sm.Specimens.Select(Nav<Entities.Variants.SpecimenIndex>).ToArray(),
        Stats = new() { Donors = DonorsOf(sm.Specimens), Genes = 1 },
        Data = new DataIndex { Sms = true }
    };

    private static Entities.Variants.CnvIndex CreateCnv((int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene) cnv) => new()
    {
        Id = cnv.Id,
        Type = cnv.Type,
        Chromosome = cnv.Chromosome,
        Start = cnv.Start,
        End = cnv.Start + 10_000,
        Length = 10_000,
        AffectedFeatures = Affects(cnv.Gene, null),
        Specimens = cnv.Specimens.Select(Nav<Entities.Variants.SpecimenIndex>).ToArray(),
        Stats = new() { Donors = DonorsOf(cnv.Specimens), Genes = 1 },
        Data = new DataIndex { Cnvs = true }
    };

    private static Entities.Variants.SvIndex CreateSv((int Id, string Type, string Chromosome, int Start, int[] Specimens, int Gene) sv) => new()
    {
        Id = sv.Id,
        Type = sv.Type,
        Chromosome = sv.Chromosome,
        OtherChromosome = sv.Chromosome,
        Start = sv.Start,
        End = sv.Start + 10,
        OtherStart = sv.Start + 5_000,
        OtherEnd = sv.Start + 5_010,
        Length = 5_000,
        AffectedFeatures = Affects(sv.Gene, null),
        Specimens = sv.Specimens.Select(Nav<Entities.Variants.SpecimenIndex>).ToArray(),
        Stats = new() { Donors = DonorsOf(sv.Specimens), Genes = 1 },
        Data = new DataIndex { Svs = true }
    };

    private static Entities.CnvProfiles.CnvProfileIndex CreateCnvProfile() => new()
    {
        Id = 1,
        Chromosome = "7",
        ChromosomeArm = "p",
        Gain = 0.5f,
        Loss = 0.1f,
        Neutral = 0.4f,
        Specimen = Nav<SpecimenNavIndex>(1)
    };
}
