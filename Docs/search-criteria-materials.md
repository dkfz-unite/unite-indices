# Material Filters Criteria
Material filters criteria. Allows to filter the data by material specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/MaterialCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Material specific filters
    "type": { "value": ["Normal", "Tumor"] },
    "tumorType": { "value": ["Primary", "Recurrent", "Metastasis"] },
    "tumorGrade": { "value": { "from": 1, "to": 4 } },
    "fixationType": { "value": ["FFPE", "Fresh Frozen"] },
    "source": { "value": ["Tissue"] }
}
```


## General Fields
General material filters applicable to any type of the index.

**`type`** - Type of the material.
- Options: `Normal`, `Tumor`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Tumor"] }`

**`tumorType`** - Type of the tumor.
- Note: Can be set only if `type` is set to `Tumor`.
- Options: `Primary`, `Recurrent`, `Metastasis`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Primary"] }`

**`source`** - Source of the material.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Tissue"] }`


## Example 1
Data, where material type is `Tumor` **and** tumor type is `Primary`.
```json
{
    "type": { "value": ["Tumor"] },
    "tumorType": { "value": ["Primary"] }
}
```

## Example 2
Data, where material type is `Normal` **and** source is `Blood derived` **or** `Solid tissue` **and** MGMT status is `Methylated`.
```json
{
    "type": { "value": ["Control"] },
    "source": { "value": ["Blood derived", "Solid tissue"] },
    "mgmtStatus": { "value": ["Methylated"] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
