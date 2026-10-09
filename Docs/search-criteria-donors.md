# Donor Filters Criteria
Donor filters criteria (`donor`). Allows to filter the data by donor specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Donors/Criteria/DonorCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Donors/DonorFilters.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Donors/DonorsNavFilters.cs).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["D01", "D02", "D03"] },

    // Clinical data filters
    "sex": { "value": ["Male", "Female", "Other"] },
    "age": { "value": { "from": 50, "to": 60 } },
    "diagnosis": { "value": ["Glioblastoma"] },
    "primarySite": { "value": ["Brain"] },
    "localization": { "value": ["Frontal Lobe"] },
    "vitalStatus": { "value": false },
    "vitalStatusChangeDay": { "value": { "from": 300, "to": null } },
    "progressionStatus": { "value": true },
    "progressionStatusChangeDay": { "value": { "from": 200, "to": null } },

    // Treatment filters
    "therapy": { "value": ["Radiotherapy"] },

    // Other filters
    "mtaProtected": { "value": true },
    "project": { "value": ["Project 1"] },
    "study": { "value": ["Study 1"] },

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
**`id`** - Internal donor identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`referenceId`** - External donor identifier (provided during data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["D01", "D02", "D03"] }`.

`id` and `referenceId` are also applied directly to the donor navigation fields
of images, specimens and projects.


## Clinical Data Fields
**`sex`** - Biological sex of the donor.
- Options: `Male`, `Female`, `Other`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Female"] }`.

**`age`** - Age of the donor at enrollment.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Example: `{ "value": { "from": 50, "to": 60 } }`.

**`diagnosis`** - Diagnosis of the donor.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Glioblastoma"] }`.

**`primarySite`** - Primary site of the tumor.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Brain"] }`.

**`localization`** - Localization of the tumor within its primary site.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Frontal Lobe"] }`.

**`vitalStatus`** - Vital status of the donor.
- Values: `true` - alive, `false` - deceased.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`vitalStatusChangeDay`** - Number of days since enrollment when the vital status was last revised.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Example: `{ "value": { "from": 300 } }`.

**`progressionStatus`** - Whether the disease is progressing after treatment.
- Values: `true` - progressing, `false` - not progressing.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`progressionStatusChangeDay`** - Number of days since treatment start when the progression status was last revised.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Example: `{ "value": { "from": 200 } }`.


## Treatment Fields
**`therapy`** - Therapy name of any of the donor's treatments.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Radiotherapy"] }`.


## Other Fields
**`mtaProtected`** - Whether the donor data is protected by a material transfer agreement (MTA).
- Values: `true` - protected, `false` - not protected.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`project`** - Name of any of the projects the donor belongs to.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Note: combined with the user's project scope, see [Project Scope](#project-scope).
- Example: `{ "value": ["Project 1"] }`.

**`study`** - Name of any of the studies the donor belongs to.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Study 1"] }`.

### Project Scope
Searches are limited to the data of projects the user may access. The search
service resolves these projects and replaces `project` with the resulting list
before the search runs:
- `project` not set - all accessible projects.
- `project` set - the listed projects that are accessible (names compared exactly).
- `project` set with `"not": true` - the accessible projects except the listed ones.

If no project remains, the search returns no results. The projects search
applies the scope to the projects themselves; there, `project` is an ordinary
criterion.


## Data Availability Fields
`hasExp`, `hasExpSc`, `hasSms`, `hasCnvs`, `hasCnvps`, `hasSvs`, `hasMeth`,
`hasProt` - whether the donor has the corresponding data. See
[Data availability criteria](./search-criteria.md#data-availability-criteria).
Applied where the donors collection is queried.


## Example 1
Donors with diagnosis `Glioblastoma` who are alive **and** aged between `50` and `60`.
```json
{
    "diagnosis": { "value": ["Glioblastoma"] },
    "vitalStatus": { "value": true },
    "age": { "value": { "from": 50, "to": 60 } }
}
```

## Example 2
Donors with a tumor in the `Brain`, localized in `Frontal left` **or** `Frontal right`, who have simple mutations data.
```json
{
    "primarySite": { "value": ["Brain"] },
    "localization": { "value": ["Frontal left", "Frontal right"] },
    "hasSms": { "value": true }
}
```

## Example 3
Donors of all accessible projects except `Project 1`.
```json
{
    "project": { "value": ["Project 1"], "not": true }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
