# Cell Line Filters Criteria
Cell line filters criteria (`line`). Allows to filter the data by cell line specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/LineCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/LineFilters.cs). Criteria inherit and include all [base](./search-criteria-specimens-base.md) specimen filters.

```jsonc
{
    // Cell line specific filters
    "cellsSpecies": { "value": ["Human", "Mouse"] },
    "cellsType": { "value": ["Stem Cell", "Differentiated"] },
    "cultureType": { "value": ["Suspension", "Adherent", "Both"] },
    "name": { "value": ["A549"] }
}
```


## Cell Line Specific Fields
**`cellsSpecies`** - Species of the cells in the line.
- Options: `Human`, `Mouse`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Human"] }`.

**`cellsType`** - Type of the cells in the line.
- Options: `Stem Cell`, `Differentiated`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Stem Cell"] }`.

**`cultureType`** - Way of cells harvesting (culture type).
- Options: `Suspension`, `Adherent`, `Both`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Suspension"] }`.

**`name`** - Name of the cell line (given at publication).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["A549"] }`.


## Example 1
Human stem cell lines.
```json
{
    "cellsSpecies": { "value": ["Human"] },
    "cellsType": { "value": ["Stem Cell"] }
}
```

## Example 2
Mouse cell lines with culture type `Suspension` **or** `Both` **and** a methylated MGMT promoter.
```json
{
    "cellsSpecies": { "value": ["Mouse"] },
    "cultureType": { "value": ["Suspension", "Both"] },
    "mgmtStatus": { "value": true }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
