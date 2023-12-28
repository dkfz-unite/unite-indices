# CNV Filters Criteria
Copy number variant (CNV) filters criteria. Allows to filter the data by CNV specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/CnvCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-variant-base.md) filters.

```jsonc
{
    // CNV specific filters
    "type": ["Gain", "Neutral", "Loss", "Undetermined"],
    "loh": true,
    "homoDel": false
}
```


## General Fields
General CNV filters applicable to any type of the index.

**`type`** - Type of the CNV.
- Options: `Gain`, `Neutral`, `Loss`, `Undetermined`.
- Type: String[].
- Filter: **Options**.
- Example: `["Gain"]`

### CNV Types
- `Gain` - total copy number is certainly higher than sample ploidy.
- `Neutral` - total copy number is very close or equal to sample ploidy.
- `Loss` - total copy number is certainly lower than sample ploidy.
- `Undetermined` - total copy number is too far from the nearest integer.

**`loh`** - Loss of heterozygosity (LOH) flag.
- Type: Boolean ( `true` - LOH, `false` - no LOH).
- Filter: **Equals**.
- Example: `true`

**`homoDel`** - Homozygous deletion flag.
- Type: Boolean ( `true` - homozygous deletion, `false` - no homozygous deletion).
- Filter: **Equals**.
- Example: `false`


## Example 1
Data, where CNV type is `Gain` **or** `Loss`.

```json
{
    "type": ["Gain", "Loss"]
}
```

## Example 2
Data, where CNV is located beetween `1000` and `2000` of the chromosome `1` **and** it's type is `Gain` **or** `Loss`.

```json
{
    "chromosome": "1",
    "positiom": { "from": 1000, "to": 2000 },
    "type": ["Gain", "Loss"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
