# CNV Profile Filters Criteria
CNV profile filters criteria (`cnvProfile`). Allows to filter the data by CNV profile criteria. A CNV profile entry describes, for one chromosome arm of a specimen's sample, which fraction of the arm is gained, lost or neutral (see the [omics feed CNV profile model](https://github.com/dkfz-unite/unite-feed-omics/blob/main/Docs/models-dna-cnvp.md)). Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Variants/Criteria/CnvProfileCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Variants/CnvProfileFilters.cs).

The criteria are applied in the CNV profiles search and, as a cross-reference
stage selecting specimens, in the donor, image, specimen, gene and protein
searches (see [coverage](./search-criteria.md#coverage-by-target-collection)).

```jsonc
{
    "specimenId": { "value": [1, 2, 3] },
    "chromosome": { "value": ["7"] },
    "chromosomeArm": { "value": ["P", "Q"] },
    "gain": { "value": { "from": 0.5, "to": 1 } },
    "loss": { "value": { "from": 0.5, "to": 1 } },
    "neutral": { "value": { "from": 0, "to": 0.5 } }
}
```


## Fields
**`specimenId`** - Internal identifier of the specimen the profile belongs to.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`chromosome`** - Chromosome.
- Options: `1`, ..., `22`, `X`, `Y`, `MT`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["7"] }`.

**`chromosomeArm`** - Chromosome arm.
- Options: `P` - short arm, `Q` - long arm.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Q"] }`.

**`gain`** - Fraction of the arm with copy number gain.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 0.5 } }`.

**`loss`** - Fraction of the arm with copy number loss.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 0.5 } }`.

**`neutral`** - Fraction of the arm without copy number change.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "to": 0.5 } }`.

All criteria of the group are applied to the same profile entry (one
chromosome arm of a sample of the specimen). In a cross-reference stage, a specimen matches when any of
its profile entries matches.


## Example 1
Profile entries of chromosome `7` where at least half of the arm is gained.
```json
{
    "chromosome": { "value": ["7"] },
    "gain": { "value": { "from": 0.5 } }
}
```

## Example 2
In a donor search: donors having a specimen whose chromosome `10` `Q` arm is mostly lost.
```json
{
    "cnvProfile": {
        "chromosome": { "value": ["10"] },
        "chromosomeArm": { "value": ["Q"] },
        "loss": { "value": { "from": 0.8 } }
    }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all `cnvProfile` filters are negated, its stage is an exclusion, see [Negative Filters](./search-criteria.md#negative-filters).
