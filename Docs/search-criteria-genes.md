# Gene Filters Criteria
Gene filters criteria. Allows to filter the data by gene specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Genes/Criteria/GeneCriteria.cs).

```jsonc
{
    // General filters
    "id": [1, 2, 3],

    // Specific filters
    "symbol": { "value": ["TP53", "EGFR", "KRAS"] },
    "biotype": { "value": ["protein_coding"] },
    "chromosome": { "value": ["1", "2", "3"] },
    "position": { "value": { "from": 1000, "to": 2000 } },

    // Data availability filters
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasGeneExp": { "value": true }
}
```


## General Fields
General gene filters applicable to any type of the index.

**`id`** - Internal gene identifier.
- Description: Allows to filter genes by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": [1, 2, 3], "not": false }`.

**`symbol`** - Gene symbol.
- Description: Allows to filter genes by gene symbol.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["TP53", "EGFR", "KRAS"] }`.

**`biotype`** - Gene biotype.
- Options: [Ensembl](https://www.ensembl.org) gene [biotypes](https://www.ensembl.org/info/genome/genebuild/biotypes.html).
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["protein_coding"] }`.

**`chromosome`** - Gene chromosome name.
- Options: `1`, ..., `22`, `X`, `Y`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["1"] }`.

**`position`** - Gene position range.
- Note: Is allowed only if `chromosome` is specified and contains only one value.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 1000, "to": 2000 } }`.


## Specific Fields
Gene specific filters applicable to gene index only.

**`hasSms`** - Whether or not the gene is affected by any simple somitic mutation.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvs`** - Whether or not the gene is affected by any copy number variant.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasSvs`** - Whether or not the gene is affected by any structural variant.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasGeneExp`** - Whether or not the gene has bulk expression data available.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.


## Example 1
Data, where gene symbol is `TP53`.
```json
{
    "symbol": { "value": ["TP53"] }
}
```

## Example 2
Data, where gene is located beetween `1000` and `2000` of the chromosome `1`.
```json
{
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } }
}
```

##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
