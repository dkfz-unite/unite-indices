# SM Filters Criteria
Simple mutation (SM) filters criteria (`sm`). Allows to filter the data by SM specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/SmCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Variants/SmFilters.cs). Criteria inherit and include all [base](./search-criteria-variant-base.md) variant filters.

```jsonc
{
    // SM specific filters
    "type": { "value": ["SNV", "INS", "DEL", "MNV"] }
}
```


## SM Specific Fields
**`type`** - Type of the SM.
- Options: `SNV`, `INS`, `DEL`, `MNV`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["SNV"] }`.

### SM Types
- `SNV` - Single nucleotide variant.
- `INS` - Insertion.
- `DEL` - Deletion.
- `MNV` - Multiple nucleotide variant.


## Example 1
SMs of type `INS` **or** `DEL`.
```json
{
    "type": { "value": ["INS", "DEL"] }
}
```

## Example 2
SMs of type `INS` **or** `DEL` on chromosome `1`, starting or ending between `1000` and `2000`.
```json
{
    "chromosome": { "value": ["1"] },
    "position": { "value": { "from": 1000, "to": 2000 } },
    "type": { "value": ["INS", "DEL"] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
