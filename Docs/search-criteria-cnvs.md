# CNV Filters Criteria
Copy number variant (CNV) filters criteria. Allows to filter the data by CNV specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/CnvCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-variant-base.md) filters.

```jsonc
{
    // CNV specific filters
    "type": { "value": ["Gain", "Neutral", "Loss", "Undetermined"], "not": false },
    "loh": { "value": true },
    "del": { "value": false },
}
```


## General Fields
General CNV filters applicable to any type of the index.

**`type`** - Type of the CNV.
- Options: `Gain`, `Neutral`, `Loss`, `Undetermined`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Gain"], "not": false }`

**`loh`** - Loss of heterozygosity (LOH) flag.
- Values: `true` - LOH, `false` - no LOH.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`

**`del`** - Homozygous deletion flag.
- Values: `true` - homozygous deletion, `false` - no homozygous deletion.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": false }`

### CNV Types
- `Gain` - total copy number is certainly higher than sample ploidy.
- `Neutral` - total copy number is very close or equal to sample ploidy.
- `Loss` - total copy number is certainly lower than sample ploidy.
- `Undetermined` - total copy number is too far from the nearest integer.


## Example 1
Data, where CNV is of type `Gain` **or** `Loss` **and** located in gene `TP53`.
```json
{
    "gene": { "value": ["TP53"] },
    "type": { "value": ["Gain", "Loss"] }
}
```

## Example 2
Data, where CNV is of type **not** `Neutral` **and** located in gene `TP53`.
```json
{
    "gene": { "value": ["TP53"] },
    "type": { "value": ["Neutral"], "not": true }
}
```

## Example 3
Data, where CNV is **not** (of type `Gain` **and** located in gene `TP53`)
```json
{
    "gene": { "value": ["TP53"], "not": true },
    "type": { "value": ["Gain"], "not": true }
}
```

##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
