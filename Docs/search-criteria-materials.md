# Material Filters Criteria
Material filters criteria (`material`). Allows to filter the data by material specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/MaterialCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/MaterialFilters.cs). Criteria inherit and include the [base](./search-criteria-specimens-base.md) specimen filters, except intervention and drug screening filters.

```jsonc
{
    // Base specimen filters (see base page)
    "category": { "value": ["Tumor"] },
    "tumorType": { "value": ["Primary"] },

    // Material specific filters
    "fixationType": { "value": ["FFPE", "Fresh Frozen"] },
    "source": { "value": ["Solid tissue"] }
}
```


## Material Specific Fields
**`fixationType`** - Material preservation type.
- Options: `FFPE`, `Fresh Frozen`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["FFPE"] }`.

**`source`** - Material source (for example solid tissue or plasma).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Solid tissue"] }`.


## Example 1
Tumor materials of primary tumors.
```json
{
    "category": { "value": ["Tumor"] },
    "tumorType": { "value": ["Primary"] }
}
```

## Example 2
Normal materials from `Blood` **or** `Solid tissue` that are `FFPE` preserved.
```json
{
    "category": { "value": ["Normal"] },
    "source": { "value": ["Blood", "Solid tissue"] },
    "fixationType": { "value": ["FFPE"] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
