# Base Variant Filters Criteria
Base variant filters criteria. These criteria are part of all variant specific filters criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/VariantBaseCriteria.cs).

```jsonc
{
    // General filters filters
    "id": ["SSM1221", "SSM1222"],
    "chromosome": ["1"],
    "position": { "from": 1000, "to": 2000 },
    "length": { "from": 1, "to": 20 },
    "impact": ["High", "Moderate"],
    "effect": ["missense_variant"]
}
```


## General Fields
General variant filters applicable to any type of the index.

**`id`** - Internal variant identifier.
- Description: Allows to filter variants by internal identifiers.
- Type: String[].
- Filter: **Equals**.
- Example: `["SSM1221", "SSM1222"]`.

**`chromosome`** - Variant chromosome name.
- Type: String[].
- Options: `1`, ..., `22`, `X`, `Y`.
- Filter: **Options**.
- Example: `["1"]`.

**`position`** - Variant position range.
- Note: Is allowed only if `chromosome` is specified and contains only one value.
- Type: Range\<Integer\>
- Filter: **Range**.
- Example: `{ "from": 1000, "to": 2000 }`.

**`length`** - Variant length range.
- Type: Range\<Integer\>
- Filter: **Range**.
- Example: `{ "from": 1, "to": 20 }`.

**`impact`** - Variant impact type.
- Type: String[].
- Options: `High`, `Moderate`, `Low`, `Unknown`.
- Filter: **Options**.
- Example: `["Moderate"]`.

**`effect`** - Variant effect type.
- Type: String[].
- Options: [Ensembl](https://www.ensembl.org/index.html) variant effect (consequence) [types](https://www.ensembl.org/info/genome/variation/prediction/predicted_data.html).
- Filter: **Options**.
- Example: `["missense_variant"]`.


## Example 1
Data, where variant chromosome is `1` **and** position is between `1000` and `2000` **and** length is between `1` and `20`.

```json
{
    "chromosome": ["1"],
    "position": { "from": 1000, "to": 2000 },
    "length": { "from": 1, "to": 20 }
}
```

## Example 2
Data, where variant impact is `Moderate` **and** effect is `missense_variant`.

```json
{
    "impact": ["Moderate"],
    "effect": ["missense_variant"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
