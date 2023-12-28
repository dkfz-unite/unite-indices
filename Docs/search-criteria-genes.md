# Gene Filters Criteria
Gene filters criteria. Allows to filter the data by gene specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Genes/Criteria/GeneCriteria.cs).

```jsonc
{
    // General filters
    "id": [1, 2, 3],

    // Specific filters
    "symbol": ["TP53", "EGFR", "KRAS"],
    "biotype": ["protein_coding"],
    "chromosome": ["1", "2", "3"],
    "position": { "from": 1000, "to": 2000 },

    // Data availability filters
    "hasSsms": true,
    "hasCnvs": true,
    "hasSvs": true,
    "hasGeneExp": true
}
```


## General Fields
General gene filters applicable to any type of the index.

**`id`** - Internal gene identifier.
- Description: Allows to filter genes by internal identifiers.
- Type: Integer[].
- Filter: **Equals**.
- Example: `[1, 2, 3]`.

**`symbol`** - Gene symbol.
- Description: Allows to filter genes by gene symbol.
- Type: String[].
- Filter: **Like**.
- Example: `["TP53", "EGFR", "KRAS"]`.

**`biotype`** - Gene biotype.
- Options: [Ensembl](https://www.ensembl.org) gene [biotypes](https://www.ensembl.org/info/genome/genebuild/biotypes.html).
- Type: String[].
- Filter: **Options**.
- Example: `["protein_coding"]`.

**`chromosome`** - Gene chromosome name.
- Type: String[].
- Options: `1`, ..., `22`, `X`, `Y`.
- Filter: **Options**.
- Example: `["1"]`.

**`position`** - Gene position range.
- Note: Is allowed only if `chromosome` is specified and contains only one value.
- Type: Range\<Integer\>
- Filter: **Range**.
- Example: `{ "from": 1000, "to": 2000 }`.


## Specific Fields
Gene specific filters applicable to gene index only.

**`hasSsms`** - Whether or not the gene is affected by any simple somitic mutation.
- Type: Boolean ( `true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`.

**`hasCnvs`** - Whether or not the gene is affected by any copy number variant.
- Type: Boolean ( `true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`.

**`hasSvs`** - Whether or not the gene is affected by any structural variant.
- Type: Boolean ( `true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`.

**`hasGeneExp`** - Whether or not the gene has bulk expression data available.
- Type: Boolean ( `true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`.


## Example 1
Data, where gene symbol is `TP53`.

```json
{
    "symbol": ["TP53"]
}
```

## Example 2
Data, where gene is located beetween `1000` and `2000` of the chromosome `1`.

```json
{
    "chromosome": "1",
    "positiom": { "from": 1000, "to": 2000 }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
