# SV Filters Criteria
Structural variant (SV) filters criteria. Allows to filter the data by SV specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/SvCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-variant-base.md) filters.

```jsonc
{
    // SV specific filters
    "type": ["DUP", "TDUP", "INS", "DEL", "INV", "ITX" "CTX"],
    "inverted": true
}
```


## General Fields
General SV filters applicable to any type of the index.

**`type`** - Type of the SV.
- Options: `DUP`, `TDUP`, `INS`, `DEL`, `INV`, `ITX`, `CTX`.
- Type: String[].
- Filter: **Options**.
- Example: `["DUP"]`

### SV Types
- `DUP` - Duplication.
- `TDUP` - Tandem duplication.
- `INS` - Insertion.
- `DEL` - Deletion.
- `INV` - Inversion.
- `ITX` - Intra-chromosomal translocation.
- `CTX` - Inter-chromosomal translocation.

**`inverted`** - Inverted flag.
- Type: Boolean ( `true` - inverted, `false` - not inverted).
- Filter: **Equals**.
- Example: `true`


## Example 1
Data, where SV type is `INS` **or** `DEL`.

```json
{
    "type": ["INS", "DEL"]
}
```

## Example 2
Data, where SV is located beetween `1000` and `2000` of the chromosome `1` **and** it's type is `INS` **or** `DEL`.

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
