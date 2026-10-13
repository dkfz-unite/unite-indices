# Image Filters Criteria
Common image filters criteria (`image`). Allows to filter the data by criteria shared by all image types. Images of all types share one collection. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Images/Criteria/ImagesCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Images/ImagesNavFilters.cs).

Type specific criteria are in separate groups: [MR images](search-criteria-mrs.md) (`mr`).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["I01", "I02", "I03"] },
    "imageType": { "value": ["MR", "CT"] },

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
**`id`** - Internal image identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`referenceId`** - External image identifier (provided during data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["I01", "I02"] }`.

**`imageType`** - Type of the image.
- Options: `MR` - magnetic resonance image, `CT` - computed tomography image.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["MR"] }`.

`id`, `referenceId` and `imageType` are also applied directly to the image
navigation fields of donors, specimens and projects.


## Data Availability Fields
`hasExp`, `hasExpSc`, `hasSms`, `hasCnvs`, `hasCnvps`, `hasSvs`, `hasMeth`,
`hasProt` - whether data of the corresponding type is available for the image.
See [Data availability criteria](./search-criteria.md#data-availability-criteria).
Applied where the images collection is queried.


## Example 1
`MR` images that have simple mutations, copy number variants and structural variants data available.
```json
{
    "imageType": { "value": ["MR"] },
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true }
}
```

## Example 2
Images with ID `1` **or** `2` that have bulk gene expression data available.
```json
{
    "id": { "value": [1, 2] },
    "hasExp": { "value": true }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters). The `image` group alone does not make the image stage an exclusion; only `mr` does.
