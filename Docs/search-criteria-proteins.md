# Protein Filters Criteria
Protein filters criteria (`protein`). Allows to filter the data by protein specific criteria, including protein expression. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Proteins/Criteria/ProteinCriteria.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Proteins/Criteria/ProteinsCriteria.cs), filters in [Proteins](../Unite.Indices.Search/Services/Filters/Base/Proteins/).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "symbol": { "value": ["TP53"] },
    "accession": { "value": ["P04637"] },
    "chromosome": { "value": ["17"] },
    "position": { "value": { "from": 7660000, "to": 7690000 } },

    // Expression filters
    "expression": { "value": { "from": 10, "to": null } },

    // Data availability filters
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasCnvps": { "value": true },
    "hasSvs": { "value": true }
}
```


## General Fields
**`id`** - Internal protein identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`symbol`** - Protein symbol.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["TP53"] }`.

**`accession`** - Protein accession identifier.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["P04637"] }`.

**`chromosome`** - Chromosome of the protein coding region.
- Options: `1`, ..., `22`, `X`, `Y`, `MT`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["17"] }`.

**`position`** - Protein position range.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Behaviour: a protein matches when its `start` or its `end` lies within the range. The position is not tied to a chromosome; combine it with `chromosome`.
- Example: `{ "value": { "from": 7660000, "to": 7690000 } }`.

In SM and CNV searches and stages, `symbol`, `accession`, `chromosome` and
`position` are also applied to the proteins of the transcripts affected by the
variant. In SV searches and stages only the protein IDs found by the protein
stage are applied.


## Expression Fields
**`expression`** - Normalized expression of the protein in a specimen.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Behaviour: resolved in the protein expressions collection, which holds one
  entry per protein and specimen. The stage selects specimens in which a
  protein matching the other protein criteria has an expression within the
  range. Not applied in gene and protein searches.
- Example: `{ "value": { "from": 10 } }`.


## Data Availability Fields
Applied where the proteins collection is queried (protein search and protein stages).

**`hasSms`** - Whether the protein is affected by simple mutations.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvs`** - Whether the protein is affected by copy number variants.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvps`** - Whether CNV profiles data is available for the protein (`data.cnvps`).
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Note: the omics feed does not currently set this flag for proteins, so `{ "value": true }` matches no proteins.
- Example: `{ "value": true }`.

**`hasSvs`** - Whether the protein is affected by structural variants.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

The protein criteria have no `hasExp`, `hasExpSc`, `hasMeth` or `hasProt` criteria.


## Example 1
Protein with accession `P04637`.
```json
{
    "accession": { "value": ["P04637"] }
}
```

## Example 2
In a donor search: donors having a specimen in which `TP53` protein has a normalized expression of at least `10`.
```json
{
    "protein": {
        "symbol": { "value": ["TP53"] },
        "expression": { "value": { "from": 10 } }
    }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all `protein` filters (including `expression`) are negated, the protein stages are exclusions, see [Negative Filters](./search-criteria.md#negative-filters).
