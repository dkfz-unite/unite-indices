# Donor Filters Criteria
Donor filters criteria. Allows to filter the data by donor specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Donors/Criteria/DonorCriteria.cs).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["D01", "D02", "D03"] },

    // Specific filters
    "sex": { "value": ["Male", "Female", "Other"] },
    "age": { "value": { "from": 50, "to": 60 } },
    "diagnosis": { "value": ["Glioblastoma"] },
    "primarySite": { "value": ["Brain"] },
    "localization": { "value": ["Frontal Lobe"] },
    "vitalStatus": { "value": false },
    "vitalStatusChangeDay": { "value": { "from": 300, "to": null } },
    "progressionStatus": { "value": true },
    "progressionStatusChangeDay": { "value": { "from": 200, "to": null } },
    "therapy": { "value": ["Radiotherapy"] },
    "mta": { "value": true },
    "project": { "value": ["Project 1"] },
    "study": { "value": ["Study 1"] },

    // Data availability filters
    "hasSms": { "value": true },
    "hasCnvs": { "value": true },
    "hasSvs": { "value": true },
    "hasGeneExp": { "value": true }
}
```


## General Fields
General donor filters applicable to any type of the index.

**`id`** - Internal donor identifier.
- Description: Allows to filter donors by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": [1, 2, 3], "not": false }`.

**`referenceId`** - External donor identifier.
- Description: Allows to filter donors by external identifiers (Which were provided during the data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["D01", "D02", "D03"], "not": false }`.

**`sex`** - Biological sex of the donor.
- Options: `Male`, `Female`, `Other`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Male", "Female"], "not": false }`

**`age`** - Age of the donor at the time of the diagnosis.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 50, "to": 60 }, "not": false }`.

**`diagnosis`** - Diagnosis of the donor.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Glioblastoma"], "not": false }`.

**`primarySite`** - Primary site of the tumor.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Brain"], "not": false }`.

**`localization`** - Localization of the tumor considering it's primary site.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Frontal Lobe"], "not": false }`.

**`vitalStatus`** - Vital status of the donor.
- Values: `true` - alive, `false` - deceased.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`vitalStatusChangeDay`** - Number of days since diagnosis statement when the vital status was last revised.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 300, "to": null }, "not": false }`.

**`progressionStatus`** - Whether or not the disease is progressing after treatment.
- Values: `true` - progressing, `false` - not progressing.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`progressionStatusChangeDay`** - Number of days since diagnosis statement when the progression status was last revised.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 200, "to": null }, "not": false }`.

**`therapy`** - Therapy applied to the donor.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Radiotherapy"], "not": false }`.

**`mta`** - Whether or not the donor data is protected by MTA.
- Values: `true` - protected, `false` - not protected.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`project`** - Project the donor data belongs to.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Project 1"], "not": false }`.

**`study`** - Study the donor data belongs to.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Study 1"], "not": false }`.


## Specific Fields
Special filters applicable only to donors-centric index.

**`hasSms`** - Whether or not the donor has simple mutations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasCnvs`** - Whether or not the donor has copy number variations data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasSvs`** - Whether or not the donor has structural variants data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`hasGeneExp`** - Whether or not the donor has bulk gene expression data available.
- Values: `true` - has data, `false` - no data.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.


## Example 1
Data, where donors have diagnosis `glioblastoma`, they are `alive` **and** their `age` is between `50` and `60` years.
```json
{
    "diagnosis": { "value": ["Glioblastoma"] },
    "vitalStatus": { "value": true },
    "age": { "value": { "from": 50, "to": 60 } }
}
```

## Example 2
Data, where donors have tumour located at `Frontal left` **or** `Frontal right` part of the `Brain` **and** their age is between `50` and `60` years.
```json
{
    "primarySite": { "value": ["Brain"] },
    "localization": { "value": ["Frontal left", "Frontal right"] },
    "age": { "value": { "from": 50, "to": 60 } }
}
```

##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
