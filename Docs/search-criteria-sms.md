# SM Filters Criteria
Simple mutation (SM) filters criteria. Allows to filter the data by SM specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/SsmCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-variant-base.md) filters.


```jsonc
{
    // SM specific filters
    "type": { "value": ["SNV", "INS", "DEL", "MNV"] }
}
```


## General Fields
General SM filters applicable to any type of the index.

**`type`** - Type of the SM.
- Options: `SNV`, `INS`, `DEL`, `MNV`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["SNV"] }`

### Variant Types
- `SNV` - Single nucleotide variant.
- `INS` - Insertion.
- `DEL` - Deletion.
- `MNV` - Multiple nucleotide variant.


## Example 1
Data, where SM type is `INS` **or** `DEL`.
```json
{
    "type": { "value": ["INS", "DEL"] }
}
```

## Example 2
Data, where SM is located beetween `1000` and `2000` of the chromosome `1` **and** it's type is `INS` **or** `DEL`.
```json
{
    "chromosome": { "value": "1" },
    "position": { "from": 1000, "to": 2000 },
    "type": { "value": ["INS", "DEL"] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
