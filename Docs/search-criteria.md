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

**`sm`** - [Simple mutation](search-criteria-sms.md) filters criteria.

**`cnv`** - [Copy number variant](search-criteria-cnvs.md) filters criteria.

**`sv`** - [Structural variant](search-criteria-svs.md) filters criteria.


## Notes
Depending on the index type, the following rules apply:

- Donors Index - has **all** filters available.
- Images Index - doesnt have `line`, `organoid` and `xenograft` filters (images are currently associated only with tumor material specimens without parents).
- Specimens Index - has **all** filters available.
- Genes Index - has **all** filters available.
- SMs Index - doesn't have `cnv` and `sv` filters (search engine does not handle variants intersections).
- CNVs Index - doesn't have `sm` and `sv` filters (search engine does not handle variants intersections).
- SVs Index - doesn't have `sm` and `cnv` filters (search engine does not handle variants intersections).

Filters of specific entry type conflict with other specific type filters of the same entry type.

For example:
- An image can not be MR and CT at the same time, therefore setting `mr` filters will conflict with `ct` filters and vice versa.
- A specimen can not be a material and a cell line at the same time, therefore setting `material` filters will conflict with `line`, `organoid` and `xenograft` filters and vice versa.


## Criteria Types
Criteria can be of different types, which defines their behaviour.

### Values Criteria
Filters property by one of the specified values.  
Values are combined with logical `OR` operator.  
Criteria is inclusive by default, but can also be exclusive.

#### Example 1
The following criteria will filter the data where donors have id `1`, `2` or `3`:
```json
{ 
    "donor": { "id": { "value": [1, 2, 3], "not": false } }
}
```

#### Example 2
The following criteria will filter the data where donors have id **not** equal to `1`, `2` or `3`:
```json
{ 
    "donor": { "id": { "value": [1, 2, 3], "not": true } }
}
```

### Range Criteria
Filters property by range or threshold.  
Criteria is inclusive by default, but can also be exclusive.

#### Example 1
The following criteria will filter the data where donors have age **beetween** `20` and `30`:
```json
{
    "donor": { "age": { "value": { "from": 20, "to": 30 } } }
}
```

#### Example 2
The following criteria will filter the data where donors have age greater than `20`:
```json
{ 
    "donor": { "age": { "value": { "from": 20 } } }
}
```

#### Example 3
The following criteria will filter the data where donors have age less than `20`:
```json
{
    "donor": { "age": { "value": { "to": 20 } } }
}
```

#### Example 4
The following criteria will filter the data where donors have age **not** beetween `20` and `30`:
```json
{ 
    "donor": { "age": { "value": { "from": 20, "to": 30 }, "not": true } }
}
```

### Boolean Criteria
Filters property by boolean value.

#### Example 1
The following criteria will filter the data where donors are alive:
```json
{ 
    "donor": { "vitalStatus": { "value": true } }
}
```

#### Example 2
The following criteria will filter the data where donors are **not** alive:
```json
{ 
    "donor": { "vitalStatus": { "value": false } }
}
```


## Complex Criteria
Filters can be combined into complex cross reference (cross type) search criteria.
- All filters (except pagination) are optional and empty by default.
- Filters for the same field of criteria are combined with logical `OR` operator.
- Filters for different fields of criteria are combined with logical `AND` operator.
- If all filters withing the type are exclusive, then the criteria is considered exclusive for this type.

### Example 1
The following criteria will filter the data where donors:
- have diagnosis `glioblastoma`
- aged **not** **beetween** `20` and `30`
- having recurrent tumor material

**Note**: exclusive `age` filter, does not make the whole `donor` criteria exclusive, because not all `donor` filters are exclusive.

```json
{ 
    "donor": { 
        "diagnosis": { "value": ["glioblastoma"] },
        "age": { "value": { "from": 20, "to": 30 }, "not": true }
    },
    "material": {
        "type": { "value": ["Tumor"] },
        "tumorType": { "value": ["Recurrent"] }
    }
}
```

### Example 2
The following criteria will filter the data where donors:
- have diagnosis `glioblastoma`
- aged **beetween** `20` and `30`
- having `high` or `moderate` impact SMs in gene `TP53`

```json
{ 
    "donor": { 
        "diagnosis": { "value": ["glioblastoma"] },
        "age": { "value": { "from": 20, "to": 30 } } 
    },
    "sm": {
        "gene": { "value": ["TP53"] },
        "impact": { "value": ["High", "Moderate"] }
    }
}
```

### Example 3
The following criteria will filter the data where donors:
- have diagnosis `glioblastoma`
- aged **beetween** `20` and `30`
- **not** having `high` or `moderate` impact SMs in gene `TP53`

**Note**: exclusive `gene` and `impact` filters make the whole `sm` criteria exclusive, because all `sm` filters are exclusive.

```json
{ 
    "donor": { 
        "diagnosis": { "value": ["glioblastoma"] },
        "age": { "value": { "from": 20, "to": 30 } } 
    },
    "sm": {
        "gene": { "value": ["TP53"], "not": true },
        "impact": { "value": ["High", "Moderate"], "not": true }
    }
}
```