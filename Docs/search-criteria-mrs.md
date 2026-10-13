# MR Image Filters Criteria
MR image filters criteria (`mr`). Allows to filter the data by MR image specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Images/Criteria/MrImageCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Images/MrImageFilters.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Images/ImageFilters.cs).

The filters apply to the MR part of an image document, so any positive `mr`
criterion selects MR images only.

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["I01", "I02", "I03"] },

    // MR specific filters
    "wholeTumor": { "value": { "from": 40, "to": 50 } },
    "contrastEnhancing": { "value": { "from": 5, "to": 10 } },
    "nonContrastEnhancing": { "value": { "from": 5, "to": 10 } }
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
- Example: `{ "value": ["I01", "I02", "I03"] }`.


## MR Specific Fields
**`wholeTumor`** - Whole tumor volume.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 40, "to": 50 } }`.

**`contrastEnhancing`** - Contrast enhancing tumor volume.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 5, "to": 10 } }`.

**`nonContrastEnhancing`** - Non-contrast enhancing tumor volume.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 5, "to": 10 } }`.


## Not Applied
`MrImageCriteria` inherits `imageType` and the
[data availability criteria](./search-criteria.md#data-availability-criteria)
from the image criteria classes, but the search engine does not apply them in
the `mr` group. Use the [`image`](search-criteria-images.md) group instead.


## Example 1
MR images with whole tumor volume between `40` and `50`.
```json
{
    "wholeTumor": { "value": { "from": 40, "to": 50 } }
}
```

## Example 2
MR images with ID `1` **or** `2`.
```json
{
    "id": { "value": [1, 2] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all `mr` filters are negated, the image stage is an exclusion, see [Negative Filters](./search-criteria.md#negative-filters).
