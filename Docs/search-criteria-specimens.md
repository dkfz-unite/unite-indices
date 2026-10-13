# Specimen Filters Criteria
Common specimen filters criteria (`specimen`). Allows to filter the data by criteria shared by all specimen types. Specimens of all types share one collection. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/SpecimensCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/SpecimensNavFilters.cs).

Type specific criteria are in separate groups: [materials](search-criteria-materials.md) (`material`), [cell lines](search-criteria-lines.md) (`line`), [organoids](search-criteria-organoids.md) (`organoid`) and [xenografts](search-criteria-xenografts.md) (`xenograft`).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["S01", "S02", "S03"] },
    "specimenType": { "value": ["Material", "Line", "Organoid", "Xenograft"] },

    // Data availability filters
    "hasExp": { "value": true },
    "hasExpSc": { "value": true },
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasCnvps": { "value": true },
    "hasSvs": { "value": true },
    "hasMeth": { "value": true },
    "hasProt": { "value": true }
}
```


## General Fields
**`id`** - Internal specimen identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`referenceId`** - External specimen identifier (provided during data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["S01", "S02"] }`.

**`specimenType`** - Type of the specimen.
- Options: `Material`, `Line`, `Organoid`, `Xenograft`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Material"] }`.

`id`, `referenceId` and `specimenType` are also applied directly to the
specimen navigation fields of donors, images, genes, proteins, variants and
projects. Cross-reference stages add found specimen IDs to `id`.

### Specimen Types
- `Material` - Material taken from the donor.
- `Line` - Cell line.
- `Organoid` - Organoid.
- `Xenograft` - Xenograft.


## Data Availability Fields
`hasExp`, `hasExpSc`, `hasSms`, `hasCnvs`, `hasCnvps`, `hasSvs`, `hasMeth`,
`hasProt` - whether the specimen has the corresponding data. See
[Data availability criteria](./search-criteria.md#data-availability-criteria).
Applied where the specimens collection is queried.


## Example 1
Material specimens that have simple mutations, copy number variants, structural variants and bulk gene expression data.
```json
{
    "specimenType": { "value": ["Material"] },
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasExp": { "value": true }
}
```

## Example 2
Specimens with ID `1` **or** `2`.
```json
{
    "id": { "value": [1, 2] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters). The `specimen` group alone does not make the specimen stage an exclusion; only the type specific groups do.
