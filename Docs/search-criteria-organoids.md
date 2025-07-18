# Organoid Filters Criteria
Organoid filters criteria. Allows to filter the data by organoid specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/OrganoidCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Organoid specific filters
    "medium": { "value": ["StemCult"] },
    "intervention": { "value": ["Metalisonib"] },
    "tumorigenicity": { "value": true }
}
```


## General Fields
General organoid filters applicable to any type of the index.

**`medium`** - Nutrient solution to support the growth and differentiation of organoid.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["StemCult"] }`

**`intervention`** - Intervention type.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Metalisonib"] }`

**`tumorigenicity`** - Does tumour grow in organoid or not.
- Values: `true` - tumour grows, `false` - no tumour growth.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`


## Example 1
Data, where organoid medium is `StemCult` **and** intervention type is `Metalisonib`.
```json
{
    "medium": { "value": ["StemCult"] },
    "intervention": { "value": ["Metalisonib"] }
}
```

## Example 2
Data, where organoid medium is `StemCult` **and** intervention type is `Metalisonib` **or** `Cloxinomab` **and** MGMT status is `Methylated`.
```json
{
    "medium": { "value": ["StemCult"] },
    "intervention": { "value": ["Metalisonib", "Cloxinomab"] },
    "mgmtStatus": { "value": ["Methylated"] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
