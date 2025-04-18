# Variant Filters Criteria
Common variant filters criteria. Allows to filter the data by general variant criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/VariantCriteria.cs). Variants can be of different types but they share the same index, that's why they have common filters.

```jsonc
{
    // General filters
    "id": ["SSM1221", "SSM1222"],
    "type": ["SM", "CNV", "SV"]
}
```


## General Fields
General variant filters applicable to any type of the index.

**`id`** - Internal variant identifier.
- Description: Allows to filter variants by internal identifiers.
- Type: String[].
- Filter: **Equals**.
- Example: `["SSM1221", "SSM1222"]`.

**`type`** - Type of the variant.
- Options: `SM`, `CNV`, `SV`.
- Type: String[].
- Filter: **Options**.
- Example: `["SM"]`

### Variant Types
- `SM` - Simple mutation.
- `CNV` - Copy number variant.
- `SV` - Structural variant.


## Example 1
Data, where variant id is `SSM1221` and type is `SM`.

```jsonc
{
    "id": ["SSM1221"],
    "type": ["SM"]
}
```

## Example 2
Data, where variant type is `SM` or `CNV`.

```jsonc
{
    "type": ["SM", "CNV"]
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
