# Search Engine
The search engine searches the data indexed by UNITE. It is based on
[Elasticsearch](https://www.elastic.co/) (7.17, accessed with the NEST client).

This repository supplies libraries, not a standalone HTTP API. `Unite.Indices`
defines the index documents, `Unite.Indices.Context` supplies the Elasticsearch
connection and the services used by the feeds to write and delete documents,
and `Unite.Indices.Search` supplies the query engine and the domain search
services. The HTTP data API is provided by
[unite-composer](https://github.com/dkfz-unite/unite-composer).

All searches are filtered by cross-reference search
[criteria](./search-criteria.md).


## Collections
Each data type has its own collection. Documents of one collection refer to
related data through compact navigation objects (IDs, reference IDs, types)
instead of embedding all related records. Collection names are defined in
[IndexNames](../Unite.Indices.Context/Constants/IndexNames.cs).

| Collection | Document | Populated by | Search service |
| --- | --- | --- | --- |
| `projects` | [ProjectIndex](../Unite.Indices/Entities/Projects/ProjectIndex.cs) | [Donors Feed](https://github.com/dkfz-unite/unite-feed-donors) | `ProjectsSearchService` |
| `donors` | [DonorIndex](../Unite.Indices/Entities/Donors/DonorIndex.cs) | [Donors Feed](https://github.com/dkfz-unite/unite-feed-donors) | `DonorsSearchService` |
| `images` | [ImageIndex](../Unite.Indices/Entities/Images/ImageIndex.cs) | [Images Feed](https://github.com/dkfz-unite/unite-feed-images) | `ImagesSearchService` |
| `specimens` | [SpecimenIndex](../Unite.Indices/Entities/Specimens/SpecimenIndex.cs) | [Specimens Feed](https://github.com/dkfz-unite/unite-feed-specimens) | `SpecimensSearchService` |
| `genes` | [GeneIndex](../Unite.Indices/Entities/Genes/GeneIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `GenesSearchService` |
| `gene_expressions` | [GeneExpressionIndex](../Unite.Indices/Entities/Genes/GeneExpressionIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | stages only |
| `proteins` | [ProteinIndex](../Unite.Indices/Entities/Proteins/ProteinIndex.cs.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `ProteinsSearchService` |
| `protein_expressions` | [ProteinExpressionIndex](../Unite.Indices/Entities/Proteins/ProteinExpressionIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | stages only |
| `sms` | [SmIndex](../Unite.Indices/Entities/Variants/SmIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `SmsSearchService` |
| `cnvs` | [CnvIndex](../Unite.Indices/Entities/Variants/CnvIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `CnvsSearchService` |
| `svs` | [SvIndex](../Unite.Indices/Entities/Variants/SvIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `SvsSearchService` |
| `cnv-profiles` | [CnvProfileIndex](../Unite.Indices/Entities/CnvProfiles/CnvProfileIndex.cs) | [Omics Feed](https://github.com/dkfz-unite/unite-feed-omics) | `CnvProfileSearchService` |

Images of all types (`MR`, `CT`) share the `images` collection; specimens of all
types (`Material`, `Line`, `Organoid`, `Xenograft`) share the `specimens`
collection. CNVs of type `Neutral` without loss of heterozygosity are not
indexed (`Predicates.IsInfluentCnv` in `unite-data`).


## Search Services
Search services are in [Services](../Unite.Indices.Search/Services/) and derive
from [SearchService](../Unite.Indices.Search/Services/SearchService.cs). Each
implements [ISearchService](../Unite.Indices.Search/Services/ISearchService.cs):

- `Search(PersonalSearchCriteria)` - searches the target collection.
- `Get(PersonalGetCriteria)` - returns one document by ID.
- `Stats(PersonalSearchCriteria)` - returns the availability flags
  (`DataIndex`) of all documents found by a search.

`PersonalSearchCriteria` and `PersonalGetCriteria` carry the
[SearchCriteria](./search-criteria.md) (or the requested ID) together with the
authenticated user's claims (user ID and whether the user is a root user). The
claims are supplied by the calling service (Composer) from the authenticated
request, not by the client's criteria.

### Access Scope
Searches are limited to the data of projects the user may access: public
projects, projects the user is a member of, and all projects for root users.
The search service resolves these projects
(`ProjectsIndexService.GetAccessibleProjects`) and combines them with the
`donor.project` criterion (see
[Project Scope](./search-criteria-donors.md#project-scope)). The projects
search limits the returned projects to the accessible ones.

### Search
`Search` runs in three steps:

1. **Scope** - combine the user's accessible projects with the criteria.
2. **Cross-reference stages** - for each criteria group of another collection
   that is present and supported, query that collection and collect the IDs
   linking it to the target (see [Cross-Reference Stages](#cross-reference-stages)).
3. **Final query** - build the filters of the target collection
   (`*FiltersCollection`) from the original criteria and the collected IDs, and
   query the target collection with `from`, `size` and `term`.

### Get
`Get` accepts integer keys only; another key returns `null`. For donors,
images, specimens, genes, proteins, SMs, CNVs, SVs and projects it first runs a
search with criteria containing only the requested ID, under the user's access
scope. If that search finds nothing, `Get` returns `null`; otherwise it reads
the document by key. Genes, proteins, SMs, CNVs and SVs are returned without
their `specimens` field.

### Stats
`Stats` runs the search once with `size = 0` to get the total, then pages
through the results in pages of 499 documents and returns a map from document ID
to its `DataIndex`. For CNV profiles it returns an empty map.


## Cross-Reference Stages
A stage queries a related collection with the filters of that collection
(built from all criteria that collection supports, including IDs collected by
earlier stages) and collects linked IDs with a terms aggregation. The stage
query uses `size = 0` and the same `term` as the final query.

### Positive Stages
- The found IDs are written to the ID criterion of the linked type (for example
  `specimen.id`). If that criterion already holds IDs from an earlier stage (or
  from the request), the two sets are intersected.
- Later stages and the final query see the updated criteria. This is how
  specimen and mutation criteria are kept on the
  [same specimen](./search-criteria.md#same-specimen).
- If a stage finds no IDs, or the intersection is empty, the search returns an
  empty result immediately, without running later stages or the final query.

### Exclusion Stages
When all criteria of a group are negated, its stage is an exclusion (see
[Negative Filters](./search-criteria.md#negative-filters)):

- The related collection is queried with the negated filters turned positive.
- The found IDs are collected in a set of IDs to exclude.
- The set is added to the criteria as a negated ID criterion (for example
  `specimen.id` with `not: true`) at a fixed point of the stage order, and is
  then applied as a `must_not` clause by later stages and the final query.
- An empty set excludes nothing and does not end the search.

### Limits
- Each stage collects at most **1000** distinct IDs (size of the terms
  aggregation in [SearchQuery](../Unite.Indices.Search/Engine/Queries/SearchQuery.cs)).
  IDs beyond that are not considered.
- Collections are not mapped with nested objects; criteria on arrays of objects
  (for example affected features of a variant) can be satisfied by different
  array elements.
- If Elasticsearch returns an invalid response, the search returns an empty
  result.

### Stage Order
Stages run in the following order. Each entry shows the collection queried and
the linked IDs collected. Stages without criteria are skipped. The order matters
because each stage uses the IDs collected by earlier stages.

**Donors** - images (image IDs), specimens (specimen IDs), genes (specimen IDs,
then gene IDs), gene expressions (specimen IDs), proteins (specimen IDs, then
protein IDs), protein expressions (specimen IDs), SMs, CNVs, SVs, CNV profiles
(specimen IDs).

**Images** - donors (donor IDs), specimens (specimen IDs), genes (specimen IDs,
then gene IDs), gene expressions (specimen IDs), proteins (specimen IDs, then
protein IDs), protein expressions (specimen IDs), SMs, CNVs, SVs, CNV profiles
(specimen IDs).

**Specimens** - donors (donor IDs), images (image IDs), genes (specimen IDs,
then gene IDs), gene expressions (specimen IDs), proteins (specimen IDs, then
protein IDs), protein expressions (specimen IDs), SMs, CNVs, SVs, CNV profiles
(specimen IDs).

**Genes** - donors, images, specimens (specimen IDs), proteins (gene IDs), SMs,
CNVs, SVs (gene IDs of affected genes), CNV profiles (specimen IDs).

**Proteins** - donors, images, specimens (specimen IDs), genes (gene IDs), SMs,
CNVs, SVs (protein IDs of affected transcripts), CNV profiles (specimen IDs).

**SMs, CNVs, SVs** - donors, images, specimens (specimen IDs), genes (specimen
IDs, then gene IDs), gene expressions (specimen IDs), proteins (specimen IDs,
then protein IDs), protein expressions (specimen IDs).

**CNV profiles** - no stages; only `cnvProfile` criteria are applied.

**Projects** - donors (donor IDs), images (image IDs), specimens (specimen IDs),
genes (specimen IDs, then gene IDs), gene expressions (specimen IDs), proteins
(specimen IDs, then protein IDs), protein expressions (specimen IDs), SMs,
CNVs, SVs (specimen IDs).

Where criteria of a group are also applied directly to the target, see the
[coverage table](./search-criteria.md#coverage-by-target-collection).

### Final Query
The final query applies all filters of the target collection: positive filters
as `must` clauses and negated filters as `must_not` clauses. Results are
sorted as follows (descending unless noted):

| Target | Sort |
| --- | --- |
| Donors, images, specimens | number of genes (`stats.genes`) |
| Genes, proteins | number of donors (`stats.donors`) |
| SMs, CNVs, SVs | number of donors (`stats.donors`), then chromosome and start (ascending) |
| Projects | number of donors (`stats.donors.number`) |
| CNV profiles | no explicit sort |

Total hit counts are tracked exactly (`track_total_hits`).


## Document Structure
Main documents and their nested structures. Navigation objects hold IDs and a
few identifying fields of the related entity.

### Projects
- [Project](../Unite.Indices/Entities/Projects/ProjectIndex.cs)
    - Donors - [Donor](../Unite.Indices/Entities/Projects/DonorIndex.cs)[]
        - Images - [Image](../Unite.Indices/Entities/Projects/ImageIndex.cs)[]
        - Specimens - [Specimen](../Unite.Indices/Entities/Projects/SpecimenIndex.cs)[]
            - Samples - [Sample](../Unite.Indices/Entities/Projects/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Projects/StatsIndex.cs)

### Donors
- [Donor](../Unite.Indices/Entities/Donors/DonorIndex.cs)
    - Images - [Image](../Unite.Indices/Entities/Donors/ImageIndex.cs)[]
    - Specimens - [Specimen](../Unite.Indices/Entities/Donors/SpecimenIndex.cs)[]
        - Samples - [Sample](../Unite.Indices/Entities/Donors/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Donors/StatsIndex.cs)

### Images
- [Image](../Unite.Indices/Entities/Images/ImageIndex.cs)
    - Donor - [Donor](../Unite.Indices/Entities/Images/DonorIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Images/SpecimenIndex.cs)[]
        - Samples - [Sample](../Unite.Indices/Entities/Images/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Images/StatsIndex.cs)

Images are linked only to tumor material specimens without a parent specimen.

### Specimens
- [Specimen](../Unite.Indices/Entities/Specimens/SpecimenIndex.cs)
    - Donor - [Donor](../Unite.Indices/Entities/Specimens/DonorIndex.cs)
    - Images - [Image](../Unite.Indices/Entities/Specimens/ImageIndex.cs)[]
    - Samples - [Sample](../Unite.Indices/Entities/Specimens/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Specimens/StatsIndex.cs)

### Genes
- [Gene](../Unite.Indices/Entities/Genes/GeneIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Genes/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Genes/StatsIndex.cs)

### Gene Expressions
One document per gene and specimen.
- [Gene expression](../Unite.Indices/Entities/Genes/GeneExpressionIndex.cs) - gene and specimen navigation objects, normalized expression value.

### Proteins
- [Protein](../Unite.Indices/Entities/Proteins/ProteinIndex.cs.cs)
    - Gene - [Gene](../Unite.Indices/Entities/Proteins/GeneIndex.cs)
    - Transcript - [Transcript](../Unite.Indices/Entities/Proteins/TranscriptIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Proteins/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Proteins/StatsIndex.cs)

### Protein Expressions
One document per protein and specimen.
- [Protein expression](../Unite.Indices/Entities/Proteins/ProteinExpressionIndex.cs) - protein and specimen navigation objects, normalized expression value.

### SMs, CNVs, SVs
Variant documents include the affected features (genes, transcripts, proteins)
and their consequences.
- [SM](../Unite.Indices/Entities/Variants/SmIndex.cs), [CNV](../Unite.Indices/Entities/Variants/CnvIndex.cs), [SV](../Unite.Indices/Entities/Variants/SvIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Variants/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Variants/StatsIndex.cs)

### CNV Profiles
One document per profile entry (a sample's chromosome arm).
- [CNV profile](../Unite.Indices/Entities/CnvProfiles/CnvProfileIndex.cs) - chromosome, chromosome arm, gain, loss and neutral fractions, specimen navigation object.
