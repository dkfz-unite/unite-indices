# Xenograft Filters Criteria
Xenograft filters criteria (`xenograft`). Allows to filter the data by xenograft specific criteria. Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/XenograftCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/XenograftFilters.cs). Criteria inherit and include all [base](./search-criteria-specimens-base.md) specimen filters (including `intervention`).

```jsonc
{
    // Xenograft specific filters
    "mouseStrain": { "value": ["NOD/SCID"] },
    "survivalDays": { "value": { "from": 10, "to": 20 } },
    "tumorigenicity": { "value": true },
    "tumorGrowthForm": { "value": ["Encapsulated", "Invasive"] }
}
```


## Xenograft Specific Fields
**`mouseStrain`** - Strain of the mice used in the xenograft model.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["NOD/SCID"] }`.

**`survivalDays`** - Survival length of the mice, in days.
- Filter: [Range](./search-criteria.md#range-criteria), integers.
- Behaviour: survival is indexed as a range (`survivalDaysFrom`, `survivalDaysTo`); a xenograft matches when either bound lies within the criterion range.
- Example: `{ "value": { "from": 10, "to": 20 } }`.

**`tumorigenicity`** - Whether the xenograft gives rise to progressively growing tumors.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`tumorGrowthForm`** - Growth form of the tumor cells.
- Options: `Encapsulated`, `Invasive`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Encapsulated"] }`.


## Example 1
Xenografts in `NOD/SCID` mice **with** intervention type `Metalisonib`.
```json
{
    "mouseStrain": { "value": ["NOD/SCID"] },
    "intervention": { "value": ["Metalisonib"] }
}
```

## Example 2
Xenografts with survival between `10` and `20` days **and** a methylated MGMT promoter.
```json
{
    "survivalDays": { "value": { "from": 10, "to": 20 } },
    "mgmtStatus": { "value": true }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`, see [Negative Filters](./search-criteria.md#negative-filters).
