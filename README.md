# Unite Indices

Shared architecture, development workflows, and agent instructions:
[UNITE project documentation](https://github.com/dkfz-unite/.github/blob/main/README.md).

Index models, services and the search engine.

Repository contains the following packages:
- [Indices](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices) - index models of the domain
    - Independent package, contains only index models
- [Context](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Context) - index context and services
    - Depends on `Indices` package
    - Requires [Elasticsearch](https://github.com/dkfz-unite/unite-env/tree/main/programs/elasticsearch) to map index models to index services
- [Search](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Search) - search engine
    - Depends on `Context` package
    - Contains search [engine](./Docs/search-engine.md) and related high-level services allowing to query indexed data based on cross referense filters [criteria](./Docs/search-criteria.md).

This repository exposes a code API through packages. The HTTP API for indexed
data access is provided by [unite-composer](https://github.com/dkfz-unite/unite-composer).
