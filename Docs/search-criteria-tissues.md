# Tissue Filters Criteria
Tissue filters criteria. Allows to filter the data by tissue specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/TissueCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Tissue specific filters
    "type": ["Control", "Tumor"],
    "tumorType": ["Primary", "Recurrent", "Metastasis"],
    "source": ["Solid Tissue"]
}
```


## General Fields
General tissue filters applicable to any type of the index.

**`type`** - Type of the tissue.
- Options: `Control`, `Tumor`.
- Type: String[].
- Filter: **Options**.
- Example: `["Control"]`

**`tumorType`** - Type of the tumor.
- Note: Can be set only if `type` is set to `Tumor`.
- Options: `Primary`, `Recurrent`, `Metastasis`.
- Type: String[].
- Filter: **Options**.
- Example: `["Primary"]`

**`source`** - Source of the tissue.
- Type: String[].
- Filter: **Like**.
- Example: `["Solid Tissue"]`


## Example 1
Data, where tissue type is `Tumor` **and** tumor type is `Primary`.

```json
{
    "type": ["Tumor"],
    "tumorType": ["Primary"]
}
```

## Example 2
Data, where tissue type is `Control` **and** source is `Boold derived` **or** `Solid tissue` **and** MGMT status is `Methylated`.

```json
{
    "type": ["Control"],
    "source": ["Blood derived", "Solid tissue"],
    "mgmtStatus": ["Methylated"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
