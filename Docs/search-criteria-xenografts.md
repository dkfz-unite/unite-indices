# Xenograft Filters Criteria
Xenograft filters criteria. Allows to filter the data by xenograft specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/XenograftCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Xenograft specific filters
    "mouseStrain": ["NOD/SCID"],
    "intervention": ["Metalisonib"],
    "survivalDays": { "from": 10, "to": 20 },
    "tumorigenicity": true,
    "tumorGrowthForm": ["Encapsulated", "Ivasive"]
}
```


## General Fields
General xenograft filters applicable to any type of the index.

**`mouseStrain`** - Mouse strain used for xenograft.
- Type: String[].
- Filter: **Like**.
- Example: `["NOD/SCID"]`

**`intervention`** - Intervention type.
- Type: String[].
- Filter: **Like**.
- Example: `["Metalisonib"]`

**`survivalDays`** - Survival days range.
- Type: Range\<Integer\>
- Filter: **Range**.
- Example: `{ "from": 10, "to": 20 }`

**`tumorigenicity`** - Does tumour grow in xenograft or not.
- Type: Boolean (`true` - yes, `false` - no).
- Filter: **Equals**.
- Example: `true`

**`tumorGrowthForm`** - Tumor growth form.
- Options: `Encapsulated`, `Ivasive`.
- Type: String[].
- Filter: **Options**.
- Example: `["Encapsulated"]`


## Example 1
Data, where mouse strain is `NOD/SCID` **and** intervention type is `Metalisonib`.

```json
{
    "mouseStrain": ["NOD/SCID"],
    "intervention": ["Metalisonib"]
}
```

## Example 2
Data, where mouse strain is `NOD/SCID` **and** intervention type is `Metalisonib` **or** `Cloxinomab` **and** MGMT status is `Methylated`.

```json
{
    "mouseStrain": ["NOD/SCID"],
    "intervention": ["Metalisonib", "Cloxinomab"],
    "mgmtStatus": ["Methylated"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
