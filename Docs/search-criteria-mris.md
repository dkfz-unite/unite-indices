# MRI Image Filters Criteria
MRI image filters criteria. Allows to filter the data by MRI image specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Images/Criteria/MriImageCriteria.cs).

```jsonc
{
    // General filters
    "id": [1, 2, 3],
    "referenceId": ["I01", "I02", "I03"],

    // MRI specific filters
    "wholeTumor": { "from": 40, "to": 50 },
    "contrastEnhancing": { "from": 5, "to": 10 },
    "nonContrastEnhancing": { "from": 5, "to": 10 }
}
```


## General Fields
General MRI image filters applicable to any type of the index.

**`id`** - Internal image identifier.
- Description: Allows to filter images by internal identifiers.
- Type: Integer[].
- Filter: Equals.
- Example: `[1, 2, 3]`.

**`referenceId`** - External image identifier.
- Description: Allows to filter images by external identifiers (Which were provided during the data submission).
- Type: String[].
- Filter: Equals.
- Example: `["I01", "I02", "I03"]`.

**`wholeTumor`** - Whole tumor volume (cm3).
- Type: Range\<Decimal\>.
- Filter: Range.
- Example: `{ "from": 40, "to": 50 }`.

**`contrastEnhancing`** - Contrast enhancing volume (cm3).
- Type: Range\<Decimal\>.
- Filter: Range.
- Example: `{ "from": 5, "to": 10 }`.

**`nonContrastEnhancing`** - Non-contrast enhancing volume (cm3).
- Type: Range\<Decimal\>.
- Filter: Range.
- Example: `{ "from": 5, "to": 10 }`.


## Example 1
Data, where images have voume between `40` and `50` cm3.

```json
{
    "wholeTumor": { "from": 40, "to": 50 }
}
```

## Example 2
Data, where image have ID `1` **or** `2`.

```json
{
    "id": [1, 2]
}
``` 


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
