# Cell Line Filters Criteria
Cell line filters criteria. Allows to filter the data by cell line specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/LineCriteria.cs). Criteria inheirts and includes all filters from [base](./search-criteria-specimens-base.md) filters.

```jsonc
{
    // Cell line specific filters
    "cellsSpecies": ["Human", "Mouse"],
    "cellsType": ["Stem Cell", "Differentiated"],
    "cultureType": ["Suspension", "Adherent", "Both"],
    "name": ["A549"]
}
```


## General Fields
General cell line filters applicable to any type of the index.

**`cellsSpecies`** - Whom the cell line was initially taken from.
- Options: `Human`, `Mouse`.
- Type: String[].
- Filter: **Options**.
- Example: `["Human"]`

**`cellsType`** - Type of the cell line.
- Options: `Stem Cell`, `Differentiated`.
- Type: String[].
- Filter: **Options**.
- Example: `["Stem Cell"]`

**`cultureType`** - Cells harvesting type.
- Options: `Suspension`, `Adherent`, `Both`.
- Type: String[].
- Filter: **Options**.
- Example: `["Suspension"]`

**`name`** - Name of the cell line if it's publicly known.
- Type: String[].
- Filter: **Like**.
- Example: `["A549"]`


## Example 1
Data, where cells species is `Human` **and** cells type is `Stem Cell`.

```json
{
    "cellsSpecies": ["Human"],
    "cellsType": ["Stem Cell"]
}
```

## Example 2
Data, where cells species is `Mouse` **and** culture type is `Suspension` **or** `Both` **and** MGMT status is `Methylated`.

```json
{
    "cellsSpecies": ["Mouse"],
    "cultureType": ["Suspension", "Both"],
    "mgmtStatus": ["Methylated"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
