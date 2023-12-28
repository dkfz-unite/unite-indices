# Organoid Filters Criteria
Organoid filters criteria. Allows to filter the data by organoid specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/OrganoidCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Organoid specific filters
    "medium": ["StemCult"],
    "intervention": ["Metalisonib"],
    "tumorigenicity": true
}
```


## General Fields
General organoid filters applicable to any type of the index.

**`medium`** - Nutrient solution to support the growth and differentiation of organoid.
- Type: String[].
- Filter: **Like**.
- Example: `["StemCult"]`

**`intervention`** - Intervention type.
- Type: String[].
- Filter: **Like**.
- Example: `["Metalisonib"]`

**`tumorigenicity`** - Does tumour grow in organoid or not.
- Type: Boolean (`true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`


## Example 1
Data, where organoid medium is `StemCult` **and** intervention type is `Metalisonib`.

```json
{
    "medium": ["StemCult"],
    "intervention": ["Metalisonib"]
}
```

## Example 2
Data, where organoid medium is `StemCult` **and** intervention type is `Metalisonib` **or** `Cloxinomab` **and** MGMT status is `Methylated`.

```json
{
    "medium": ["StemCult"],
    "intervention": ["Metalisonib", "Cloxinomab"],
    "mgmtStatus": ["Methylated"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
