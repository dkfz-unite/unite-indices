# UNITE repository instructions

Before changing this repository, read the shared UNITE instructions and
relevant project documentation maintained in
[dkfz-unite/.github](https://github.com/dkfz-unite/.github/blob/main/README.md).

1. Prefer the local shared checkout: `../unite-meta/AGENTS.md`, relative to this
   repository root. A sibling `.github/AGENTS.md` or a user-provided path is also
   valid. Resolve its document links relative to that shared checkout.
2. Read the shared `AGENTS.md`, project overview, repository map, and topic
   documents relevant to the task, following the shared README's task index.
3. If no shared checkout is accessible, use the GitHub versions beginning with
   [shared AGENTS.md](https://github.com/dkfz-unite/.github/blob/main/AGENTS.md).
   If those are unavailable too, report the missing context and proceed only
   with what local documentation/code establishes; do not invent shared rules.
4. Read this repository's README and relevant local docs; inspect Git status.
   More specific instructions here supplement the shared guidance.

Keep common rules in `.github`; keep component details in this repository.

## Indices

Status: reviewed with the owner (2026-10-08/09).

`unite-indices` is a **library** (no service, no HTTP API) that defines the
Elasticsearch side of UNITE: index document models, index read/write services
used by the feeds while indexing, and the search engine used by Composer and
Analysis. Search semantics, cross-reference search and personal access scope
are described in [Search and access](../unite-meta/docs/search-and-access.md);
availability flags and statistics in
[Datasets and portal](../unite-meta/docs/datasets-and-portal.md#availability-and-statistics).

### Projects and packages

| Project (NuGet package) | Contents | Used by |
| --- | --- | --- |
| `Unite.Indices` | Index documents (`Entities/`): donors, images, specimens, genes, proteins, variants, CNV profiles, projects, expressions; shared `DataIndex` (availability), `Basic/` navigation and sample models | Indirectly by all consumers |
| `Unite.Indices.Context` | NEST client options and per-collection index services (create, add, delete) in the root folder; `Constants/IndexNames.cs` | Feeds (indexing) |
| `Unite.Indices.Search` | Search engine (`Engine/`: low-level index services, generic filters, query builders, `SearchQuery`) and domain search (`Services/`: search services, criteria, filters collections, personal criteria) | Composer, Analysis |

Context and Search both have per-collection "index services" with similar
names; check the project and namespace before changing one. There are no test
projects (the workflow's `dotnet test` step has nothing to run).

### Build and publish

```sh
dotnet build Unite.Indices.sln
```

- No `dev` branch: work on `main`. A push to `main` runs
  `.github/workflows/publish-nuget.yml`, which builds, packs all three projects
  and pushes them to the GitHub Packages NuGet source.
- All three `.csproj` files carry the same `<Version>` (13.10.20 on
  2026-10-08); bump it in all three. Increase the patch number for each
  package published during development, the minor number when the change is
  released, and the major number for breaking changes; see
  [Developing shared libraries](../unite-meta/docs/deployment-and-development.md#developing-shared-libraries). Consumers pin explicit
  versions and must be updated separately; see the
  [package versions per consumer](../unite-meta/docs/repository-map.md#package-versions-per-consumer).
- To try a change in a consumer before publishing, temporarily enable its
  commented `ProjectReference` to this repository; see
  [Developing shared libraries](../unite-meta/docs/deployment-and-development.md#developing-shared-libraries).
  Verify in a consumer (Composer for search, a feed for indexing).

### Documentation

Human-readable docs are in [Docs/](Docs/), revalidated against the code on
2026-10-09: [search-criteria.md](Docs/search-criteria.md) (criteria types,
negative filters and exclusion stages, how criteria combine, coverage per target
collection, one page per criteria group) and
[search-engine.md](Docs/search-engine.md) (collections, search/get/stats, stage
order per target, document structure). Read those first, and update them with
every change to criteria, filters or stages.

### Search patterns

- Every target collection has a search service in `Services/` derived from
  `SearchService<T>`. `Search` personalizes the criteria (`PersonalSearchCriteria`,
  accessible projects from `ProjectsIndexService`), then runs the cross-reference
  stages (`AggregateFrom*`, `HandleFound*`, intersecting ID constraints, early
  exit on an empty required stage), and finally filters the target collection
  with its `*FiltersCollection`.
- `Get` runs a personalized search for the requested ID before returning the
  document; `Stats` pages through `Search` in pages of 499 and merges each
  entry's `DataIndex`.
- Cross-reference stages collect linked IDs with a terms aggregation limited to
  **1000** values (`Engine/Queries/SearchQuery.AddAggregation`). The owner
  considers this safe at the current scale (about 5,000 samples); if data grows
  or the portal becomes public, switch to paging with a composite aggregation
  (see [Search and access](../unite-meta/docs/search-and-access.md#sequential-cross-reference-search)).
- Same-specimen correlation: specimen and mutation criteria must match the same
  specimen; preserve this when carrying IDs between collections.
- Why collections are flat views, how entities connect (specimen as the hub;
  genes and variants by position and through expression in the same specimen),
  and why stages run in their order: see
  [How entities are connected](../unite-meta/docs/search-and-access.md#how-entities-are-connected).
  Stats are static and per target type, not dynamic counts.

### Where to change things

| Change | Location |
| --- | --- |
| New filter on an existing criterion | Criteria class (`Services/Filters/Criteria` or `Base/<Domain>`), base filters (`Services/Filters/Base/<Domain>/*Filters.cs`), every `*FiltersCollection` that should support it, `Docs/search-criteria-*.md`; portal filter models in `unite` |
| New criteria group | `SearchCriteria`, base criteria and filters, filters collections, the cross-reference stages of each search service that should use it (order matters), docs, portal |
| Index document shape | `Unite.Indices/Entities/...`, then the feed index creators/builders that fill it and Composer resources that read it; reindex after deployment |
| New collection | Entity (`Unite.Indices`), `IndexNames`, Context index service and `AddIndexServices`, Engine index service, search service and filters collection, `AddSearchEngine`/`AddSearchServices`, cross-reference stages in the other search services; see [Integrating a new data type](../unite-meta/docs/integrating-a-data-type.md) |
| Availability flags (`DataIndex`) | `Unite.Indices/Entities/DataIndex.cs`, data criteria/filters (`Services/Filters/Base/DataCriteria.cs`, `DataFilters.cs`), every index creator in the feeds |

### Shared libraries

`Unite.Indices.Search` depends on `Unite.Essentials` 1.4.0 (latest 1.6.0);
Context uses NEST 7.17.5 (Elasticsearch 7.17), the last NEST version.

Moving to Elasticsearch 8 would mean a new client and rewriting the query layer;
whether UNITE needs Elasticsearch at all, or could work with SQL only, is also
untested. Both are future options without a decision (owner, 2026-10-09; Jira
epic "Switch to newest Elasticsearch"). Stay on NEST 7.17 and do not start a
migration or replacement without the owner's agreement.

### CI

`publish-nuget.yml` on pushes to `main` (see Build and publish).

### Known issues

Defects are tracked in the private Jira; see the shared
[Track defects](../unite-meta/AGENTS.md#track-defects).
