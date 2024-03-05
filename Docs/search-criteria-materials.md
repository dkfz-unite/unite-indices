# Material Filters Criteria
Material filters criteria. Allows to filter the data by material specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/MaterialCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Material specific filters
    "type": ["Normal", "Tumor"],
    "tumorType": ["Primary", "Recurrent", "Metastasis"],
    "source": ["Tissue"]
}
```


## General Fields
General material filters applicable to any type of the index.

**`type`** - Type of the material.
- Options: `Normal`, `Tumor`.
- Type: String[].
- Filter: **Options**.
- Example: `["Tumor"]`

**`tumorType`** - Type of the tumor.
- Note: Can be set only if `type` is set to `Tumor`.
- Options: `Primary`, `Recurrent`, `Metastasis`.
- Type: String[].
- Filter: **Options**.
- Example: `["Primary"]`

**`source`** - Source of the material.
- Type: String[].
- Filter: **Like**.
- Example: `["Tissue"]`


## Example 1
Data, where material type is `Tumor` **and** tumor type is `Primary`.

```json
{
    "type": ["Tumor"],
    "tumorType": ["Primary"]
}
```

## Example 2
Data, where material type is `Normal` **and** source is `Boold derived` **or** `Solid tissue` **and** MGMT status is `Methylated`.

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
