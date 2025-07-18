# Xenograft Filters Criteria
Xenograft filters criteria. Allows to filter the data by xenograft specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/XenograftCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Xenograft specific filters
    "mouseStrain": { "value": ["NOD/SCID"] },
    "intervention": { "value": ["Metalisonib"] },
    "survivalDays": { "value": { "from": 10, "to": 20 } },
    "tumorigenicity": { "value": true },
    "tumorGrowthForm": { "value": ["Encapsulated", "Ivasive"] }
}
```


## General Fields
General xenograft filters applicable to any type of the index.

**`mouseStrain`** - Mouse strain used for xenograft.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["NOD/SCID"] }`

**`intervention`** - Intervention type.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Metalisonib"] }`

**`survivalDays`** - Survival days range.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 10, "to": 20 } }`

**`tumorigenicity`** - Does tumour grow in xenograft or not.
- Values: `true` - tumour grows, `false` - no tumour growth.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`

**`tumorGrowthForm`** - Tumor growth form.
- Options: `Encapsulated`, `Ivasive`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Encapsulated"] }`


## Example 1
Data, where mouse strain is `NOD/SCID` **and** intervention type is `Metalisonib`.
```json
{
    "mouseStrain": { "value": ["NOD/SCID"] },
    "intervention": { "value": ["Metalisonib"] }
}
```

## Example 2
Data, where mouse strain is `NOD/SCID` **and** intervention type is `Metalisonib` **or** `Cloxinomab` **and** MGMT status is `Methylated`.
```json
{
    "mouseStrain": { "value": ["NOD/SCID"] },
    "intervention": { "value": ["Metalisonib", "Cloxinomab"] },
    "mgmtStatus": { "value": ["Methylated"] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
