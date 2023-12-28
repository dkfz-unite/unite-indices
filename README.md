# Unite Indices

This repository contains the following connectd packages:
- [Unite.Indices](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices) - contains all indices and related entities, the package is fully independent and stores only index structure.
- [Unite.Indices.Context](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Context) - contains services related to Elasticsearch where entities from **Unite.Indices** are stored.
- [Unite.Indices.Search](https://github.com/dkfz-unite/unite-indices/pkgs/nuget/Unite.Indices.Search) - contains search [engine](./Docs/search-engine.md) and related high-level services allowing to query indexed data based on cross referense filters [criteria](./Docs/search-criteria.md). Depends on **Unite.Indices.Context** as connection to Elasticsearch.
