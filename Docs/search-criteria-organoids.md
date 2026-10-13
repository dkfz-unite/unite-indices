# Organoid Filters Criteria
Organoid filters criteria (`organoid`). Allows to filter the data by organoid specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/OrganoidCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/OrganoidFilters.cs). Criteria inherit and include all [base](./search-criteria-specimens-base.md) specimen filters (including `intervention`).

```jsonc
{
    // Organoid specific filters
    "medium": { "value": ["StemCult"] },
    "tumorigenicity": { "value": true }
}
```


## Organoid Specific Fields
**`medium`** - Organoid medium.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["StemCult"] }`.

**`tumorigenicity`** - Whether the organoid gives rise to progressively growing tumors.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.


## Example 1
Organoids grown in `StemCult` medium **with** intervention type `Metalisonib`.
```json
{
    "medium": { "value": ["StemCult"] },
    "intervention": { "value": ["Metalisonib"] }
}
```

## Example 2
Tumorigenic organoids with intervention type `Metalisonib` **or** `Cloxinomab` **and** a methylated MGMT promoter.
```json
{
    "tumorigenicity": { "value": true },
    "intervention": { "value": ["Metalisonib", "Cloxinomab"] },
    "mgmtStatus": { "value": true }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
