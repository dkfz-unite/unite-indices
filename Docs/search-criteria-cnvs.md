# CNV Filters Criteria
Copy number variant (CNV) filters criteria (`cnv`). Allows to filter the data by CNV specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/CnvCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Variants/CnvFilters.cs). Criteria inherit and include all [base](./search-criteria-variant-base.md) variant filters.

CNVs of type `Neutral` without loss of heterozygosity are not indexed
(`Predicates.IsInfluentCnv` in `unite-data`), so they are not found by CNV
criteria.

```jsonc
{
    // CNV specific filters
    "type": { "value": ["Gain", "Loss", "Neutral", "Undetermined"] },
    "loh": { "value": true },
    "del": { "value": false }
}
```


## CNV Specific Fields
**`type`** - Type of the CNV.
- Options: `Gain`, `Loss`, `Neutral`, `Undetermined`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Gain"] }`.

**`loh`** - Loss of heterozygosity (LOH) flag.
- Values: `true` - LOH, `false` - no LOH.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`del`** - Homozygous deletion flag.
- Values: `true` - homozygous deletion, `false` - no homozygous deletion.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": false }`.

### CNV Types
- `Gain` - Total copy number gain.
- `Loss` - Total copy number loss.
- `Neutral` - Total copy number neutral.
- `Undetermined` - Undetermined.


## Example 1
CNVs of type `Gain` **or** `Loss` affecting gene `TP53`.
```json
{
    "gene": { "value": ["TP53"] },
    "type": { "value": ["Gain", "Loss"] }
}
```

## Example 2
CNVs affecting gene `TP53` that are **not** of type `Neutral`.
```json
{
    "gene": { "value": ["TP53"] },
    "type": { "value": ["Neutral"], "not": true }
}
```

## Example 3
In a cross-reference stage (for example a donor search), all `cnv` filters negated: exclude entries linked to specimens with a `Gain` CNV affecting gene `TP53`.
```json
{
    "gene": { "value": ["TP53"], "not": true },
    "type": { "value": ["Gain"], "not": true }
}
```
In a CNV search the same criteria are applied directly: CNVs that do not affect `TP53` **and** are not of type `Gain`.


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
