# Gene Filters Criteria
Gene filters criteria (`gene`). Allows to filter the data by gene specific criteria, including gene expression. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Genes/Criteria/GeneCriteria.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Genes/Criteria/GenesCriteria.cs), filters in [Genes](../Unite.Indices.Search/Services/Filters/Base/Genes/).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "symbol": { "value": ["TP53", "EGFR", "KRAS"] },
    "biotype": { "value": ["protein_coding"] },
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
**`id`** - Internal gene identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`symbol`** - Gene symbol.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["TP53", "EGFR"] }`.

**`biotype`** - Gene biotype.
- Options: [Ensembl](https://www.ensembl.org) gene [biotypes](https://www.ensembl.org/info/genome/genebuild/biotypes.html).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["protein_coding"] }`.

**`chromosome`** - Chromosome of the gene.
- Options: `1`, ..., `22`, `X`, `Y`, `MT`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["17"] }`.

**`position`** - Gene position range.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Behaviour: a gene matches when its `start` or its `end` lies within the range. The position is not tied to a chromosome; combine it with `chromosome`.
- Example: `{ "value": { "from": 7660000, "to": 7690000 } }`.

In SM, CNV and SV searches and stages these criteria (`id`, `symbol`,
`biotype`, `chromosome`, `position`) are also applied to the genes affected by
the variant.


## Expression Fields
**`expression`** - Normalized expression of the gene in a specimen (bulk gene expression).
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Behaviour: resolved in the gene expressions collection, which holds one entry
  per gene and specimen. The stage selects specimens in which a gene matching
  the other gene criteria has an expression within the range. Not applied in
  gene and protein searches.
- Example: `{ "value": { "from": 10 } }`.


## Data Availability Fields
Applied where the genes collection is queried (gene search and gene stages).

**`hasSms`** - Whether the gene is affected by simple mutations.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvs`** - Whether the gene is affected by copy number variants.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvps`** - Whether CNV profiles data is available for the gene (`data.cnvps`).
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Note: the omics feed does not currently set this flag for genes, so `{ "value": true }` matches no genes.
- Example: `{ "value": true }`.

**`hasSvs`** - Whether the gene is affected by structural variants.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

The gene criteria have no `hasExp`, `hasExpSc`, `hasMeth` or `hasProt` criteria.


## Example 1
Gene `TP53`.
```json
{
    "symbol": { "value": ["TP53"] }
}
```

## Example 2
Genes on chromosome `17` starting or ending between `7660000` and `7690000`.
```json
{
    "chromosome": { "value": ["17"] },
    "position": { "value": { "from": 7660000, "to": 7690000 } }
}
```

## Example 3
In a donor search: donors having a specimen in which `EGFR` has a normalized expression of at least `100`.
```json
{
    "gene": {
        "symbol": { "value": ["EGFR"] },
        "expression": { "value": { "from": 100 } }
    }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all `gene` filters (including `expression`) are negated, the gene stages are exclusions, see [Negative Filters](./search-criteria.md#negative-filters).
