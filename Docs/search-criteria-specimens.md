# Specimen Filters Criteria
Common specimen filters criteria. Allows to filter the data by general specimen criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/SpecimenCriteria.cs). Specimens can be of different types but they share the same index, that's why they have common filters.

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "type": { "value": ["Tissue", "CellLine", "Organoid", "Xenograft"] },

    // Data availability filters
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasGeneExp": { "value": true }
}
```


## General Fields
General specimen filters applicable to any type of the index.

**`id`** - Internal specimen identifier.
- Description: Allows to filter specimens by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": [1, 2, 3] }`.

**`type`** - Type of the specimen.
- Options: `Tissue`, `CellLine`, `Organoid`, `Xenograft`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Tissue"] }`

### Specimen Types
- `Tissue` - Tissues.
- `CellLine` - Cell lines.
- `Organoid` - Organoids.
- `Xenograft` - Xenografts.


## Specific Fields
Special filters applicable only to specimen-centric index.

**`hasSms`** - Whether or not any specimen has simple mutations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvs`** - Whether or not any specimen has copy number variations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasSvs`** - Whether or not any specimen has structural variants data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasGeneExp`** - Whether or not any specimen has bulk gene expression data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.


## Example 1
Data, where specimens are `Tissue` specimens **and** they have all types of the variants data associated.
```json
{
    "type": ["Tissue"],
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasGeneExp": { "value": true }
}
```

## Example 2
Data, where specimens have ID `1` **or** `2`.
```json
{
    "id": { "value": [1, 2] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
