# Search Criteria
Search criteria is used to filter the data indexed by the search engine. It is cross reference and applied to all the fields in the index.

```JSONC
{
    "from": 1,
    "size": 20,
    "term": null,

    "donor": null,
    "image": null,
    "mr": null,
    "specimen": null,
    "material": null,
    "line": null,
    "organoid": null,
    "xenograft": null,
    "gene": null,
    "variant": null,
    "sm": null,
    "cnv": null,
    "sv": null
}
```


## General Fields
Allows to filter data by general criteria.

**`from`** - Starting position of the results to return.
- Note: Should be greater than 0.
- Type: Integer
- Default: 1
- Example: `1`

**`size`** - Number of results to return.
- Note: Should be greater than `from`.
- Type: Integer
- Default: 20
- Example: `20`

**`term`** - General full text search query.
- Type: String
- Default: `null`
- Example: `"Glioblastoma"`


## Domain Specific Fields
Allows to filter data by domain specific criteria.

**`donor`** - [Donor](search-criteria-donors.md) filters criteria.

**`image`** - [Image](search-criteria-images.md) common filters criteria.

**`mr`** - [MR image](search-criteria-mrs.md) filters criteria.

**`specimen`** - [Specimen](search-criteria-specimens.md) common filters criteria.

**`material`** - [Material](search-criteria-materials.md) filters criteria.

**`line`** - [Cell line](search-criteria-lines.md) filters criteria.

**`organoid`** - [Organoid](search-criteria-organoids.md) filters criteria.

**`xenograft`** - [Xenograft](search-criteria-xenografts.md) filters criteria.

**`gene`** - [Gene](search-criteria-genes.md) filters criteria.

**`variant`** - [Variant](search-criteria-variants.md) common filters criteria.

**`sm`** - [Simple mutation](search-criteria-sms.md) filters criteria.

**`cnv`** - [Copy number variant](search-criteria-cnvs.md) filters criteria.

**`sv`** - [Structural variant](search-criteria-svs.md) filters criteria.


## Notes
Depending on the index type, the following rules apply:

- Donors Index - has **all** filters available.
- Images Index - doesnt have `line`, `organoid` and `xenograft` filters (images are currently associated only with tumor material specimens without parents).
- Specimens Index - has **all** filters available.
- Genes Index - has **all** filters available.
- Variants Index - has **all** filters available.

Filters of specific entry type conflict with other specific type filters of the same entry type.

For example:
- An image can not be MR and CT at the same time, therefore setting `mr` filters will conflict with `ct` filters and vice versa.
- A specimen can not be a material and a cell line at the same time, therefore setting `material` filters will conflict with `line`, `organoid` and `xenograft` filters and vice versa.
- A variant can not be a SM and a CNV at the same time, therefore setting `sm` filters will conflict with `cnv` and `sv` filters and vice versa.


## Filters
Filters can be of different types what defines thier behaviour.

### Equals
Filters the data by exact match of one or more values.

For example, the following criteria will filter the data where donors have id **equal** to 1, 2 or 3:
```json
{ 
    "donor": { "id": [1, 2, 3] } 
}
```

### Like
Filters the data by partial match of one or more values.

For example, the following criteria will filter the data where donors have diagnosis **similar** to `glioblastoma`:
```json
{ 
    "donor": { "diagnosis": ["glioblastoma"] } 
}
```

### Range
Filters the data by range **beetween** two **or** any of specified **borders**.

For example, the following criteria will filter the data where donors have age **beetween** `20` and `30`:
```json
{ 
    "donor": { "age": { "from": 20, "to": 30 } } 
}
```

For example, the following criteria will filter the data where donors have age greater than `20`:
```json
{ 
    "donor": { "age": { "from": 20 } } 
}
```


## Rules
- All filters (except pagination) are optional and empty by default.
- filters for the same field of criteria are combined with logical `OR` operator.
- filters for different fields of criteria are combined with logical `AND` operator.


For example, the following criteria will filter the data where donors have diagnisis similar to `glioblastoma` **and** age **beetween** `20` and `30`:
```json
{ 
    "donor": { 
        "diagnosis": ["glioblastoma"], 
        "age": { "from": 20, "to": 30 } 
    } 
}
```

For example, the following criteria will filter the data where donors have id `1`, `2` or `3` **and** `High` **or** `Moderate` impact SMs in gene `TP53`:
```json
{ 
    "donor": { "id": [1, 2, 3] },
    "sm": { "impact": ["High", "Moderate"] },
    "gene": { "symbol": ["TP53"] }
}
``` 
