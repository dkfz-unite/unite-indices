# Base Variant Filters Criteria
Base variant filters criteria. These criteria are part of all variant specific criteria: [SM](search-criteria-sms.md) (`sm`), [CNV](search-criteria-cnvs.md) (`cnv`) and [SV](search-criteria-svs.md) (`sv`). Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/VariantCriteria.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/VariantsCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Variants/VariantFilters.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Variants/VariantsNavFilters.cs).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } },
    "length": { "value": { "from": 1, "to": 20 } },

    // Consequence filters
    "gene": { "value": ["TP53"] },
    "impact": { "value": ["High", "Moderate"] },
    "effect": { "value": ["missense_variant"] }
}
```


## General Fields
**`id`** - Internal variant identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`chromosome`** - Chromosome of the variant.
- Options: `1`, ..., `22`, `X`, `Y`, `MT`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["1"] }`.

**`position`** - Variant position range.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Behaviour: a variant matches when its `start` or its `end` lies within the range (SVs use different positions, see the [SV page](search-criteria-svs.md#position)). The position is not tied to a chromosome; combine it with `chromosome`.
- Example: `{ "value": { "from": 1000, "to": 2000 } }`.

**`length`** - Variant length range.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Example: `{ "value": { "from": 1, "to": 20 } }`.


## Consequence Fields
These criteria apply to the features (genes and transcripts) affected by the
variant and their consequences. A variant matches when any of its affected
features matches. Affected features are not indexed as nested objects, so
`gene`, `impact` and `effect` can be satisfied by different affected features
of the same variant.

**`gene`** - Symbol of a gene affected by the variant.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["TP53"] }`.

**`impact`** - Impact of a variant consequence.
- Options: `High`, `Moderate`, `Low`, `Unknown`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Moderate"] }`.

**`effect`** - Type of a variant consequence.
- Options: [Ensembl](https://www.ensembl.org) variant consequence [terms](https://www.ensembl.org/info/genome/variation/prediction/predicted_data.html) (for example `missense_variant`).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["missense_variant"] }`.

[Gene](search-criteria-genes.md) and [protein](search-criteria-proteins.md)
criteria are also applied to the affected features in variant searches and
stages.


## Example 1
Variants on chromosome `1` starting or ending between `1000` and `2000`, with a length between `1` and `20`.
```json
{
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } },
    "length": { "value": { "from": 1, "to": 20 } }
}
```

## Example 2
Variants with a `Moderate` impact consequence **and** a `missense_variant` consequence.
```json
{
    "impact": { "value": ["Moderate"] },
    "effect": { "value": ["missense_variant"] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all filters of a variant group are negated, its stage is an exclusion, see [Negative Filters](./search-criteria.md#negative-filters).
