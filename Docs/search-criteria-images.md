# Image Filters Criteria
Common image filters criteria. Allows to filter the data by general image criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Images/Criteria/ImageCriteria.cs). Images can be of different types but they share the same index, that's why they have common filters.

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "type": { "value": ["MR", "CT"] },

    // Data availability filters
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasGeneExp": { "value": true }
}
```


## General Fields
General image filters applicable to any type of the index.

**`id`** - Internal image identifier.
- Description: Allows to filter images by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": [1, 2, 3], "not": false }`.

**`type`** - Type of the image.
- Options: `MR`, `CT`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["MR"], "not": false }`

### Image Types
- `MR` - Magnetic resonance image.
- `CT` - Computed tomography image.


## Specific Fields
Special filters applicable only to image-centric index.

**`hasSms`** - Whether or not any image related specimen has simple mutations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`

**`hasCnvs`** - Whether or not any image related specimen has copy number variations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasSvs`** - Whether or not any image related specimen has structural variants data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasGeneExp`** - Whether or not any image related specimen has bulk gene expression data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.


## Example 1
Data, where images are `MR` images **and** they have all types of the variants data associated.
```json
{
    "type": { "value": ["MR"] },
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true }
}
```

## Example 2
Data, where image ID's are `1` **or** `2` and they have bulk gene expression data available.
```json
{
    "id": { "value": [1, 2] },
    "hasGeneExp": { "value": true }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
