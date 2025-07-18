# Base Variant Filters Criteria
Base variant filters criteria. These criteria are part of all variant specific filters criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/VariantBaseCriteria.cs).

```jsonc
{
    // General filters filters
    "id": { "value": ["SSM1221", "SSM1222"] },
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } },
    "length": { "value": { "from": 1, "to": 20 } },
    "impact": { "value": ["High", "Moderate"] },
    "effect": { "value": ["missense_variant"] }
}
```


## General Fields
General variant filters applicable to any type of the index.

**`id`** - Internal variant identifier.
- Description: Allows to filter variants by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["SSM1221", "SSM1222"] }`.

**`chromosome`** - Variant chromosome name.
- Options: `1`, ..., `22`, `X`, `Y`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["1"] }`.

**`position`** - Variant position range.
- Note: Is allowed only if `chromosome` is specified and contains only one value.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 1000, "to": 2000 } }`.

**`length`** - Variant length range.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 1, "to": 20 } }`.

**`impact`** - Variant impact type.
- Options: `High`, `Moderate`, `Low`, `Unknown`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Moderate"] }`.

**`effect`** - Variant effect type.
- Type: String[].
- Options: [Ensembl](https://www.ensembl.org/index.html) variant effect (consequence) [types](https://www.ensembl.org/info/genome/variation/prediction/predicted_data.html).
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["missense_variant"] }`.


## Example 1
Data, where variant chromosome is `1` **and** position is between `1000` and `2000` **and** length is between `1` and `20`.
```json
{
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } },
    "length": { "value": { "from": 1, "to": 20 } }
}
```

## Example 2
Data, where variant impact is `Moderate` **and** effect is `missense_variant`.
```json
{
    "impact": { "value": ["Moderate"] },
    "effect": { "value": ["missense_variant"] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
