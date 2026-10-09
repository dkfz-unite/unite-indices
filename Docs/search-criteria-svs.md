# SV Filters Criteria
Structural variant (SV) filters criteria (`sv`). Allows to filter the data by SV specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/SvCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Variants/SvFilters.cs). Criteria inherit and include all [base](./search-criteria-variant-base.md) variant filters; `position` behaves differently for SVs.

```jsonc
{
    // SV specific filters
    "type": { "value": ["DUP", "TDUP", "INS", "DEL", "INV", "ITX", "CTX", "COM"] },
    "inverted": { "value": true }
}
```


## SV Specific Fields
**`type`** - Type of the SV.
- Options: `DUP`, `TDUP`, `INS`, `DEL`, `INV`, `ITX`, `CTX`, `COM`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["DUP"] }`.

**`inverted`** - Inverted flag.
- Values: `true` - inverted, `false` - not inverted.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

### Position
**`position`** - SV breakpoint position range.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Behaviour: an SV matches when the end of its first breakpoint (`end`) or the start of its second breakpoint (`otherStart`) lies within the range. The base variant rule (`start` or `end`) is not used for SVs. The position is not tied to a chromosome; combine it with `chromosome`.
- Example: `{ "value": { "from": 1000, "to": 2000 } }`.

### SV Types
- `DUP` - Duplication.
- `TDUP` - Tandem duplication.
- `INS` - Insertion.
- `DEL` - Deletion.
- `INV` - Inversion.
- `ITX` - Intra-chromosomal translocation.
- `CTX` - Inter-chromosomal translocation.
- `COM` - Complex rearrangement.


## Example 1
SVs of type `INS` **or** `DEL`.
```json
{
    "type": { "value": ["INS", "DEL"] }
}
```

## Example 2
SVs of type `INS` **or** `DEL` on chromosome `1` with a breakpoint position between `1000` and `2000`.
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
