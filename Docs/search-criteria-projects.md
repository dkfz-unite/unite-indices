# Project Filters Criteria
Project filters criteria (`project`). Allows to filter projects in the projects search. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Projects/Criteria/ProjectCriteria.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Projects/Criteria/ProjectsCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Projects/ProjectFilters.cs) and [here](../Unite.Indices.Search/Services/Filters/Base/Projects/ProjectsNavFilters.cs).

The `project` group is applied **only** by the projects search. To restrict
other searches to projects, use the donor criterion
[`donor.project`](search-criteria-donors.md#other-fields).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "name": { "value": ["Project 1"] },

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
**`id`** - Internal project identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`name`** - Project name.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Project 1"] }`.


## Data Availability Fields
`hasExp`, `hasExpSc`, `hasSms`, `hasCnvs`, `hasCnvps`, `hasSvs`, `hasMeth`,
`hasProt` - whether the project has data of the corresponding type. See
[Data availability criteria](./search-criteria.md#data-availability-criteria).


## Other Criteria in the Projects Search
The projects search also accepts criteria of other groups. They select projects
containing at least one matching donor, image or specimen (see
[coverage](./search-criteria.md#coverage-by-target-collection)). `cnvProfile`
criteria are not applied. Results are limited to projects the user may access.


## Example 1
Project `Project 1`.
```json
{
    "project": {
        "name": { "value": ["Project 1"] }
    }
}
```

## Example 2
Projects with simple mutations data that contain glioblastoma donors.
```json
{
    "project": {
        "hasSms": { "value": true }
    },
    "donor": {
        "diagnosis": { "value": ["Glioblastoma"] }
    }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
