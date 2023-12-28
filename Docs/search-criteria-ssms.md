# SSM Filters Criteria
Simple somatic mutation (SSM) filters criteria. Allows to filter the data by SSM specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/SsmCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-variant-base.md) filters.


public string[] Type { get; set; }

```jsonc
{
    // SSM specific filters
    "type": ["SNV", "INS", "DEL", "MNV"]
}
```


## General Fields
General SSM filters applicable to any type of the index.

**`type`** - Type of the SSM.
- Options: `SNV`, `INS`, `DEL`, `MNV`.
- Type: String[].
- Filter: **Options**.
- Example: `["SNV"]`

### Variant Types
- `SNV` - Single nucleotide variant.
- `INS` - Insertion.
- `DEL` - Deletion.
- `MNV` - Multiple nucleotide variant.


## Example 1
Data, where SSM type is `INS` **or** `DEL`.

```json
{
    "type": ["INS", "DEL"]
}
```

## Example 2
Data, where SSM is located beetween `1000` and `2000` of the chromosome `1` **and** it's type is `INS` **or** `DEL`.

```json
{
    "chromosome": "1",
    "positiom": { "from": 1000, "to": 2000 },
    "type": ["INS", "DEL"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
