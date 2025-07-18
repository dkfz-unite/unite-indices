# Cell Line Filters Criteria
Cell line filters criteria. Allows to filter the data by cell line specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/LineCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Cell line specific filters
    "cellsSpecies": { "value": ["Human", "Mouse"] },
    "cellsType": { "value": ["Stem Cell", "Differentiated"] },
    "cultureType": { "value": ["Suspension", "Adherent", "Both"] },
    "name": { "value": ["A549"] }
}
```


## General Fields
General cell line filters applicable to any type of the index.

**`cellsSpecies`** - Whom the cell line was initially taken from.
- Options: `Human`, `Mouse`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Human"] }`

**`cellsType`** - Type of the cell line.
- Options: `Stem Cell`, `Differentiated`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Stem Cell"] }`

**`cultureType`** - Cells harvesting type.
- Options: `Suspension`, `Adherent`, `Both`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Suspension"] }`

**`name`** - Name of the cell line if it's publicly known.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["A549"] }`


## Example 1
Data, where cells species is `Human` **and** cells type is `Stem Cell`.
```json
{
    "cellsSpecies": { "value": ["Human"] },
    "cellsType": { "value": ["Stem Cell"] }
}
```

## Example 2
Data, where cells species is `Mouse` **and** culture type is `Suspension` **or** `Both` **and** MGMT status is `Methylated`.
```json
{
    "cellsSpecies": { "value": ["Mouse"] },
    "cultureType": { "value": ["Suspension", "Both"] },
    "mgmtStatus": { "value": ["Methylated"] }
}
```

##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
