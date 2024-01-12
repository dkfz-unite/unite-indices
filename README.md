# Unite Indices
Index models, services and the search engine.

Repository contains the following packages:
- [Indices](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices) - index models of the domain
    - Independent package, contains only index models
- [Context](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Context) - index context and services
    - Depends on `Indices` package
    - Requires [Elasticsearch](https://github.com/dkfz-unite/unite-environment/tree/main/programs/elasticsearch) to map index models to index services
- [Search](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Search) - search engine
    - Depends on `Context` package
    - Contains search [engine](./Docs/search-engine.md) and related high-level services allowing to query indexed data based on cross referense filters [criteria](./Docs/search-criteria.md).
