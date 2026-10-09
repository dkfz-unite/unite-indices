# Search Criteria
Search criteria describe what to search for. A single criteria object is sent to
the search service of a **target collection** (donors, images, specimens, genes,
proteins, SMs, CNVs, SVs, CNV profiles or projects). It can combine criteria of
many data types: a donor search can include mutation criteria, and a mutation
search can include donor criteria. The search engine resolves criteria of other
data types in separate cross-reference stages and carries the found IDs into the
target query (see [Search Engine](search-engine.md)).

Which criteria groups take effect depends on the target collection; see
[Coverage by target collection](#coverage-by-target-collection). A criteria
group or criterion that a target does not support is not applied.

The criteria model is
[SearchCriteria](../Unite.Indices.Search/Services/Filters/Criteria/SearchCriteria.cs).
The library declares no JSON attributes; the JSON property names below are the
camelCase forms of the C# property names, as used by the
[Composer](https://github.com/dkfz-unite/unite-composer) web API (ASP.NET Core
web defaults: camelCase, property names matched case-insensitively).

```jsonc
{
    "from": 0,
    "size": 20,
    "term": null,

    "project": null,
    "donor": null,
    "image": null,
    "mr": null,
    "specimen": null,
    "material": null,
    "line": null,
    "organoid": null,
    "xenograft": null,
    "gene": null,
    "protein": null,
    "sm": null,
    "cnv": null,
    "sv": null,
    "cnvProfile": null
}
```


## General Fields
**`from`** - Zero-based offset of the first result to return.
- Type: Integer
- Default: `0`
- Example: `0`

**`size`** - Maximum number of results to return.
- Type: Integer
- Default: `20`
- Example: `20`

**`term`** - Full text search query.
- Type: String
- Default: `null`
- Behaviour: when set, it is added as an Elasticsearch `multi_match` query
  without explicit fields to **every** collection queried during the search,
  including cross-reference stages. Its practical effect has not been verified;
  the code marks it as possibly not working.
- Example: `"Glioblastoma"`

`from` and `size` apply only to the final query of the target collection.


## Criteria Groups
**`project`** - [Project](search-criteria-projects.md) criteria. Used only by the projects search.

**`donor`** - [Donor](search-criteria-donors.md) criteria.

**`image`** - [Image](search-criteria-images.md) criteria, common to all image types.

**`mr`** - [MR image](search-criteria-mrs.md) criteria.

**`specimen`** - [Specimen](search-criteria-specimens.md) criteria, common to all specimen types.

**`material`** - [Material](search-criteria-materials.md) criteria.

**`line`** - [Cell line](search-criteria-lines.md) criteria.

**`organoid`** - [Organoid](search-criteria-organoids.md) criteria.

**`xenograft`** - [Xenograft](search-criteria-xenografts.md) criteria.

**`gene`** - [Gene](search-criteria-genes.md) criteria, including gene expression.

**`protein`** - [Protein](search-criteria-proteins.md) criteria, including protein expression.

**`sm`** - [Simple mutation](search-criteria-sms.md) criteria.

**`cnv`** - [Copy number variant](search-criteria-cnvs.md) criteria.

**`sv`** - [Structural variant](search-criteria-svs.md) criteria.

**`cnvProfile`** - [CNV profile](search-criteria-cnv-profiles.md) criteria.

Shared pages:
- [Base specimen criteria](search-criteria-specimens-base.md) - included in `material`, `line`, `organoid` and `xenograft`.
- [Base variant criteria](search-criteria-variant-base.md) - included in `sm`, `cnv` and `sv`.
- [Data availability criteria](#data-availability-criteria) - included in `project`, `donor`, `image` and `specimen`.

A CT image group (`ct`) does not exist in the current criteria model.


## Criteria Types
Every criterion is an object with a `value` and an optional `not` flag:

```jsonc
{ "value": <value>, "not": false }
```

- `not` - `true` negates the criterion (see [Negative Filters](#negative-filters)).
  Omitted, `null` and `false` mean a positive criterion.
- A criterion without a value (`null`, an empty array, or a range without
  `from` and `to`) is ignored.

### Values Criteria
Matches when the property equals (or matches, see [Value matching](#value-matching))
**any** of the given values; values are combined with logical `OR`.

- Shape: `{ "value": [ ... ], "not": false }`
- Model: [ValuesCriteria](../Unite.Indices.Search/Services/Filters/Criteria/ValuesCriteria.cs)

The following criteria select donors with ID `1`, `2` **or** `3`:
```json
{
    "donor": { "id": { "value": [1, 2, 3] } }
}
```

### Range Criteria
Matches when the property lies within the range. Both bounds are inclusive and
each bound is optional: only `from` means "greater than or equal", only `to`
means "less than or equal".

- Shape: `{ "value": { "from": <number>, "to": <number> }, "not": false }`
- Model: [RangeCriteria](../Unite.Indices.Search/Services/Filters/Criteria/RangeCriteria.cs)

The following criteria select donors aged `20` to `30` (inclusive):
```json
{
    "donor": { "age": { "value": { "from": 20, "to": 30 } } }
}
```

The following criteria select donors aged `20` or older:
```json
{
    "donor": { "age": { "value": { "from": 20 } } }
}
```

### Boolean Criteria
Matches when the property equals the given boolean value.

- Shape: `{ "value": true, "not": false }`
- Model: [BoolCriteria](../Unite.Indices.Search/Services/Filters/Criteria/BoolCriteria.cs)

The following criteria select donors with vital status `true` (alive):
```json
{
    "donor": { "vitalStatus": { "value": true } }
}
```

`{ "value": false }` selects documents where the property is `false`; documents
without a value do not match. To also include documents without a value, use
`{ "value": true, "not": true }`.

### Value Matching
Values criteria use one of two matching modes. Each criteria page states the
mode of every criterion.

- **Exact** - Elasticsearch `terms` query on a keyword (or numeric) field. The
  whole value must be equal, including letter case. Used for IDs and for
  enumerated values (for example `sex`, `category`, `type`).
- **Text** - Elasticsearch `match` query with operator `AND`, one query per
  value. On analysed text fields every word of a value must occur in the
  property, case-insensitively (for example `diagnosis`, `referenceId`,
  `symbol`). Some criteria use text matching on a keyword field; there the
  whole value must be equal, including letter case. Pages mark these as
  "Text (keyword)".

Properties that hold several values per document (for example donor
treatments, specimen drug screenings or variant affected genes) match when
**any** element matches. These arrays are not mapped as nested objects, so two
criteria on the same array (for example `drug` and `dss`) can be satisfied by
different elements of that array.


## Data Availability Criteria
Boolean criteria filtering by the availability flags
([DataIndex](../Unite.Indices/Entities/DataIndex.cs)) of the target or related
document. They are declared in
[DataCriteria](../Unite.Indices.Search/Services/Filters/Base/DataCriteria.cs) and
are available in the `project`, `donor`, `image` and `specimen` groups. They are
applied only in the collection of that group (projects, donors, images,
specimens), either as the target or in a cross-reference stage.

| Criterion | Flag | Data |
| --- | --- | --- |
| `hasExp` | `data.exp` | Bulk gene expression |
| `hasExpSc` | `data.expSc` | Single-cell gene expression |
| `hasSms` | `data.sms` | Simple mutations |
| `hasCnvs` | `data.cnvs` | Copy number variants |
| `hasCnvps` | `data.cnvps` | CNV profiles |
| `hasSvs` | `data.svs` | Structural variants |
| `hasMeth` | `data.meth` | DNA methylation |
| `hasProt` | `data.prot` | Protein expression |

- Filter: [Boolean](#boolean-criteria).
- Example: `{ "value": true }`.

The `material`, `line`, `organoid`, `xenograft` and `mr` groups inherit these
properties from their base classes, but the search engine does not apply them
there. Genes and proteins have their own `hasSms`, `hasCnvs`, `hasCnvps` and
`hasSvs` criteria (see the [gene](search-criteria-genes.md) and
[protein](search-criteria-proteins.md) pages).


## Combining Criteria
- All criteria (except pagination) are optional and empty by default.
- Values of one criterion are combined with logical `OR`.
- Different criteria, within a group and across groups, are combined with
  logical `AND`.
- Criteria of a related data type restrict the target through linked entities.
  For example, `sm` criteria in a donor search select donors that have at least
  one specimen with a matching simple mutation.

### Same Specimen
Specimen criteria and criteria resolved through specimens (gene, gene and
protein expression, protein, SM, CNV, SV and CNV profile criteria) must be
satisfied by **the same specimen**. Each stage intersects the found specimen IDs
with the specimen IDs found by earlier stages. A donor with a recurrent tumor
specimen and the requested mutation only in a different specimen does not match
a search requiring both.

### Same Entry Type
Images of different types and specimens of different types share one
collection each. An entry has one type, so criteria for different types of the
same collection exclude each other: for example, `material` and `line` criteria
together select specimens that are both a material and a cell line, which never
match.


## Negative Filters
A criterion with `"not": true` is negated. How a negation behaves depends on
whether **all** criteria of a group are negated.

### Negating Single Criteria
When a group contains at least one positive criterion, every negated criterion
is applied as an Elasticsearch `must_not` clause to the documents of that
group's collection:

- A negated values criterion matches documents whose property equals none of
  the values.
- A negated range criterion matches documents whose property is outside the
  range.
- Documents without a value for the property also match a negated criterion.

The following criteria select donors diagnosed with glioblastoma whose age is
**not** between `20` and `30`. The `donor` group is not negated as a whole
because `diagnosis` is positive.

```json
{
    "donor": {
        "diagnosis": { "value": ["Glioblastoma"] },
        "age": { "value": { "from": 20, "to": 30 }, "not": true }
    }
}
```

In a cross-reference stage this selects related entities that do not match the
negated criterion. For example, in a donor search
`"sm": { "gene": { "value": ["TP53"] }, "impact": { "value": ["High"], "not": true } }`
selects donors having a specimen with an SM in `TP53` that has no `High`
impact consequence.

### Negating a Whole Group (Exclusion)
When every non-empty criterion of a group is negated, the group is treated as an
**exclusion** in cross-reference stages:

1. The related collection is queried with the group's criteria taken as
   positive.
2. The IDs linked to the found entities (for example specimen IDs) are
   collected.
3. Target entries linked to any of these IDs are excluded.

An exclusion stage that finds nothing does not end the search; nothing is
excluded.

The following criteria select glioblastoma donors who have **no** specimen with
a `High` or `Moderate` impact SM in `TP53`:

```json
{
    "donor": {
        "diagnosis": { "value": ["Glioblastoma"] }
    },
    "sm": {
        "gene": { "value": ["TP53"], "not": true },
        "impact": { "value": ["High", "Moderate"], "not": true }
    }
}
```

Compare with negating only `impact`: that selects donors having a specimen with
an SM in `TP53` that has no `High` or `Moderate` impact consequence.

Group rules:
- Specimens: the specimen stage runs as an exclusion when any of the `material`,
  `line`, `organoid` or `xenograft` groups is present and all its criteria are
  negated. The `specimen` group is evaluated in the same stage but does not
  affect this decision.
- Images: the image stage runs as an exclusion when the `mr` group is present
  and all its criteria are negated. The `image` group does not affect this
  decision.
- Genes and proteins: `expression` is part of the `gene` or `protein` group;
  the expression stage follows the decision of the whole group.
- Donors: searches limit `donor.project` to the projects the user may access
  (see [donor criteria](search-criteria-donors.md#project-scope)), which adds a
  positive criterion to the `donor` group. Negated donor criteria are therefore
  applied as single negated criteria to each donor. Images and specimens belong
  to one donor, so for them the result equals an exclusion. Genes, proteins and
  variants are selected when they are linked to at least one donor satisfying
  the donor criteria. The projects search applies the access scope to the
  projects themselves, so there the donor group can be an exclusion.
- For the specimen and image stages, a `material`, `line`, `organoid`,
  `xenograft` or `mr` group sent without criteria (for example
  `"material": {}`) counts as fully negated.
- Criteria the target applies directly (see "D" in the coverage table) are also
  applied to the target documents as single negated criteria.

Some targets do not apply every exclusion; see the notes of the
[coverage table](#coverage-by-target-collection).


## Coverage by Target Collection
Which criteria groups each search service applies, verified against the search
services (`Services/*SearchService.cs`) and filters collections
(`Services/Filters/*FiltersCollection.cs`).

- **D** - applied directly to the target documents (own fields or navigation
  fields of the target).
- **S** - resolved in a cross-reference stage; the found IDs restrict the target.
- **–** - not applied as a filter.

| Group | Donors | Images | Specimens | Genes | Proteins | SMs | CNVs | SVs | CNV profiles | Projects |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `project` | – | – | – | – | – | – | – | – | – | D |
| `donor` | D | D, S | D, S | S | S | S | S | S | – | D, S |
| `image` | D, S | D | D, S | S | S | S | S | S | – | D, S |
| `mr` | S | D | S | S | S | S | S | S | – | S |
| `specimen` | D, S | D, S | D | D, S | D, S | D, S | D, S | D, S | – | D, S |
| `material`, `line`, `organoid`, `xenograft` | S | S ¹ | D | S | S | S | S | S | – | S |
| `gene` (except `expression`) | S | S | S | D | S ³ | D, S | D, S | D, S | – | S |
| `gene.expression` | S | S | S | – | – | S ⁴ | S ⁴ | S ⁴ | – | S |
| `protein` (except `expression`) | S | S | S | S | D | D, S | D, S | S | – | S |
| `protein.expression` | S | S | S | – | – | S ⁴ | S ⁴ | S ⁴ | – | S |
| `sm` | S | S | S | S | S | D | – | – | – | S |
| `cnv` | S | S | S | S | S | – | D | – | – | S |
| `sv` | S | S | S | S | S | – | – | D | – | S |
| `cnvProfile` | S | S | S | S ² | S ² | – | – | – | D | – |

Notes:
1. Images are linked only to tumor material specimens without a parent
   specimen (`Predicates.IsImageRelatedSpecimen` in `unite-data`). `line`,
   `organoid` and `xenograft` criteria are accepted but match no images.
2. Genes and proteins: an exclusion (whole `cnvProfile` group negated) has no
   effect.
3. Proteins: gene criteria select proteins of the matching genes. An exclusion
   (whole `gene` group negated) is not supported.
4. SMs, CNVs and SVs: an exclusion of the expression stage (whole `gene` or
   `protein` group negated) has no effect on the expression part.
5. Data availability criteria of a group apply only where that group's
   collection is queried: the target itself or its cross-reference stage.
6. Variant targets do not combine criteria of other variant types (for example
   CNV criteria in an SM search).
7. Genes and proteins: `gene.expression` and `protein.expression` are not
   applied. In a gene search, a `protein` group containing only `expression`
   still runs the protein stage, without protein filters; the same applies to a
   `gene` group containing only `expression` in a protein search.

The stage order of each target is listed in
[Search Engine](search-engine.md#cross-reference-stages).


## Example
The following criteria select donors that:
- have diagnosis `Glioblastoma`,
- are aged `20` to `30`,
- have a recurrent tumor material specimen,
- and, in that same specimen, a `High` or `Moderate` impact SM in gene `TP53`.

```json
{
    "donor": {
        "diagnosis": { "value": ["Glioblastoma"] },
        "age": { "value": { "from": 20, "to": 30 } }
    },
    "material": {
        "category": { "value": ["Tumor"] },
        "tumorType": { "value": ["Recurrent"] }
    },
    "sm": {
        "gene": { "value": ["TP53"] },
        "impact": { "value": ["High", "Moderate"] }
    }
}
```
