# Specimen Filters Criteria
Common specimen filters criteria. Allows to filter the data by general specimen criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/SpecimenCriteria.cs). Specimens can be of different types but they share the same index, that's why they have common filters.

```jsonc
{
    // General filters
    "id": [1, 2, 3],
    "type": ["Tissue", "CellLine", "Organoid", "Xenograft"],

    // Data availability filters
    "hasSsms": true,
    "hasCnvs": true,
    "hasSvs": true,
    "hasGeneExp": true
}
```


## General Fields
General specimen filters applicable to any type of the index.

**`id`** - Internal specimen identifier.
- Description: Allows to filter specimens by internal identifiers.
- Type: Integer[].
- Filter: **Equals**.
- Example: `[1, 2, 3]`.

**`type`** - Type of the specimen.
- Options: `Tissue`, `CellLine`, `Organoid`, `Xenograft`.
- Type: String[].
- Filter: **Options**.
- Example: `["Tissue"]`

### Specimen Types
- `Tissue` - Tissues.
- `CellLine` - Cell lines.
- `Organoid` - Organoids.
- `Xenograft` - Xenografts.


## Specific Fields
Special filters applicable only to specimen-centric index.

**`hasSsms`** - Whether or not any specimen has simple somatic mutations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasCnvs`** - Whether or not any specimen has copy number variations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasSvs`** - Whether or not any specimen has structural variants data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasGeneExp`** - Whether or not any specimen has bulk gene expression data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.


## Example 1
Data, where specimens are `Tissue` specimens **and** they have all types of the variants data associated.

```json
{
    "type": ["Tissue"],
    "hasSsms": true,
    "hasCnvs": true,
    "hasSvs": true,
    "hasGeneExp": true
}
```

## Example 2
Data, where specimens have ID `1` **or** `2`.

```json
{
    "id": [1, 2]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
