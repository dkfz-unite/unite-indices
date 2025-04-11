# Image Filters Criteria
Common image filters criteria. Allows to filter the data by general image criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Images/Criteria/ImageCriteria.cs). Images can be of different types but they share the same index, that's why they have common filters.

```jsonc
{
    // General filters
    "id": [1, 2, 3],
    "type": ["MR", "CT"],

    // Data availability filters
    "hasSms": true,
    "hasCnvs": true,
    "hasSvs": true,
    "hasGeneExp": true
}
```


## General Fields
General image filters applicable to any type of the index.

**`id`** - Internal image identifier.
- Description: Allows to filter images by internal identifiers.
- Type: Integer[].
- Filter: **Equals**.
- Example: `[1, 2, 3]`.

**`type`** - Type of the image.
- Options: `MR`, `CT`.
- Type: String[].
- Filter: **Options**.
- Example: `["MR"]`

### Image Types
- `MR` - Magnetic resonance image.
- `CT` - Computed tomography image.


## Specific Fields
Special filters applicable only to image-centric index.

**`hasSms`** - Whether or not any image related specimen has simple mutations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasCnvs`** - Whether or not any image related specimen has copy number variations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasSvs`** - Whether or not any image related specimen has structural variants data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasGeneExp`** - Whether or not any image related specimen has bulk gene expression data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.


## Example 1
Data, where images are `MR` images **and** they have all types of the variants data associated.

```json
{
    "type": ["MR"],
    "hasSms": true,
    "hasCnvs": true,
    "hasSvs": true
}
```

## Example 2
Data, where image ID's are `1` **or** `2` and they have bulk gene expression data available.

```json
{
    "id": [1, 2],
    "hasGeneExp": true
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
