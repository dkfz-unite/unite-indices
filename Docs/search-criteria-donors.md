# Donor Filters Criteria
Donor filters criteria. Allows to filter the data by donor specific criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Donors/Criteria/DonorCriteria.cs).

```jsonc
{
    // General filters
    "id": [1, 2, 3],
    "referenceId": ["D01", "D02", "D03"],

    // Specific filters
    "sex": ["Male", "Female", "Other"],
    "age": { "from": 50, "to": 60 },
    "diagnosis": ["Glioblastoma"],
    "primarySite": ["Brain"],
    "localization": ["Frontal Lobe"],
    "vitalStatus": false,
    "vitalStatusChangeDay": { "from": 300, "to": null },
    "progressionStatus": true,
    "progressionStatusChangeDay": { "from": 200, "to": null },
    "therapy": ["Radiotherapy"],
    "mtaProtected": true,
    "project": ["Project 1"],
    "study": ["Study 1"],

    // Data availability filters
    "hasSms": true,
    "hasCnvs": true,
    "hasSvs": true,
    "hasGeneExp": true
}
```


## General Fields
General donor filters applicable to any type of the index.

**`id`** - Internal donor identifier.
- Description: Allows to filter donors by internal identifiers.
- Type: Integer[].
- Filter: **Equals**.
- Example: `[1, 2, 3]`.

**`referenceId`** - External donor identifier.
- Description: Allows to filter donors by external identifiers (Which were provided during the data submission).
- Type: String[].
- Filter: **Like**.
- Example: `["D01", "D02", "D03"]`.

**`sex`** - Biological sex of the donor.
- Options: `Male`, `Female`, `Other`.
- Type: String[].
- Filter: **Options**.
- Example" `["Male", "Female"]`

**`age`** - Age of the donor at the time of the diagnosis.
- Type: Range\<Integer\>.
- Filter: **Range**.
- Example: `{ "from": 50, "to": 60 }`.

**`diagnosis`** - Diagnosis of the donor.
- Type: String[].
- Filter: **Like**.
- Example: `["Glioblastoma"]`.

**`primarySite`** - Primary site of the tumor.
- Type: String[].
- Filter: **Like**.
- Example: `["Brain"]`.

**`localization`** - Localization of the tumor considering it's primary site.
- Type: String[].
- Filter: **Like**.
- Example: `["Frontal Lobe"]`.

**`vitalStatus`** - Vital status of the donor.
- Type: Boolean (`true` - alive, `false` - deceased).
- Filter: **Equals**.
- Example: `false`.

**`vitalStatusChangeDay`** - Number of days since diagnosis statement when the vital status was last revised.
- Type: Range\<Integer\>.
- Filter: **Range**.
- Example: `{ "from": 300, "to": null }`.

**`progressionStatus`** - Whether or not the disease is progressing after treatment.
- Type: Boolean (`true` - progressing, `false` - not progressing).
- Filter: **Equals**.
- Example: `true`.

**`progressionStatusChangeDay`** - Number of days since diagnosis statement when the progression status was last revised.
- Type: Range\<Integer\>.
- Filter: **Range**.
- Example: `{ "from": 200, "to": null }`.

**`therapy`** - Therapy applied to the donor.
- Type: String[].
- Filter: **Like**.
- Example: `["Radiotherapy"]`.

**`mtaProtected`** - Whether or not the donor data is protected by MTA.
- Type: Boolean (`true` - protected, `false` - not protected).
- Filter: **Equals**.
- Example: `true`.

**`project`** - Project the donor data belongs to.
- Type: String[].
- Filter: **Like**.
- Example: `["Project 1"]`.

**`study`** - Study the donor data belongs to.
- Type: String[].
- Filter: **Like**.
- Example: `["Study 1"]`.


## Specific Fields
Special filters applicable only to donors-centric index.

**`hasSms`** - Whether or not the donor has simple mutations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasCnvs`** - Whether or not the donor has copy number variations data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasSvs`** - Whether or not the donor has structural variants data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.

**`hasGeneExp`** - Whether or not the donor has bulk gene expression data available.
- Type: Boolean (`true` - has data, `false` - no data).
- Filter: **Equals**.
- Example: `true`.


## Example 1
Data, where donors have diagnosis `glioblastoma`, they are `alive` **and** their `age` is between `50` and `60` years.

```json
{
    "diagnosis": ["Glioblastoma"],
    "vitalStatus": true,
    "age": { "from": 50, "to": 60 }
}
```

## Example 2
Data, where donors have tumour located at `Frontal left` **or** `Frontal right` part of the `Brain` **and** their age is between `50` and `60` years.

```json
{
    "primarySite": ["Brain"],
    "localization": ["Frontal left", "Frontal right"],
    "age": { "from": 50, "to": 60 }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
