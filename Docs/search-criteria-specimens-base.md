# Base Specimen Filters Criteria
Base specimen filters criteria. These criteria are part of all specimen type specific criteria: [materials](search-criteria-materials.md) (`material`), [cell lines](search-criteria-lines.md) (`line`), [organoids](search-criteria-organoids.md) (`organoid`) and [xenografts](search-criteria-xenografts.md) (`xenograft`). Actual criteria can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/SpecimenCriteria.cs), filters [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/SpecimenFilters.cs).

The filters apply to the type specific part of a specimen document (for
example `material` criteria to the material data of a specimen), so any
positive criterion of a type specific group selects specimens of that type only.

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["S01", "S02", "S03"] },
    "category": { "value": ["Normal", "Tumor"] },
    "tumorType": { "value": ["Primary", "Metastasis", "Recurrent"] },
    "tumorGrade": { "value": { "from": 1, "to": 4 } },

    // Tumor classification filters
    "tumorSuperfamily": { "value": ["Superfamily 1"] },
    "tumorFamily": { "value": ["Family 1"] },
    "tumorClass": { "value": ["Class 1"] },
    "tumorSubClass": { "value": ["Subclass 1"] },

    // Molecular data filters
    "mgmtStatus": { "value": true },
    "idhStatus": { "value": false },
    "idhMutation": { "value": ["IDH1 R132H"] },
    "tertStatus": { "value": true },
    "tertMutation": { "value": ["C228T", "C250T"] },
    "expressionSubtype": { "value": ["Classical", "Mesenchymal", "Proneural"] },
    "methylationSubtype": { "value": ["RTKI", "RTKII", "Mesenchymal"] },
    "gcimpMethylation": { "value": true },
    "geneKnockout": { "value": ["TP53"] },

    // Intervention filters (not for materials)
    "intervention": { "value": ["Temozolomide"] },

    // Drug screening filters (not for materials)
    "drug": { "value": ["Temozolomide", "Lomustine"] },
    "dss": { "value": { "from": 10, "to": 50 } },
    "dssS": { "value": { "from": 10, "to": 50 } }
}
```


## General Fields
**`id`** - Internal specimen identifier.
- Filter: [Values](./search-criteria.md#values-criteria), integers.
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": [1, 2, 3] }`.

**`referenceId`** - External specimen identifier (provided during data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["S01", "S02"] }`.

**`category`** - Specimen category.
- Options: `Normal`, `Tumor`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Tumor"] }`.

**`tumorType`** - Tumor type of the specimen.
- Options: `Primary`, `Metastasis`, `Recurrent`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Primary"] }`.

**`tumorGrade`** - Tumor grade of the specimen.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 3, "to": 4 } }`.


## Tumor Classification Fields
**`tumorSuperfamily`** - Tumor superfamily.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Superfamily 1"] }`.

**`tumorFamily`** - Tumor family.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Family 1"] }`.

**`tumorClass`** - Tumor class.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Class 1"] }`.

**`tumorSubClass`** - Tumor subclass.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text (keyword)](./search-criteria.md#value-matching) - whole value, case-sensitive.
- Example: `{ "value": ["Subclass 1"] }`.


## Molecular Data Fields
Value meanings follow the
[specimens feed molecular data model](https://github.com/dkfz-unite/unite-feed-specimens/blob/main/Docs/api-models-base-molecular.md).

**`mgmtStatus`** - Whether the MGMT promoter is methylated.
- Values: `true` - methylated, `false` - unmethylated.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`idhStatus`** - Whether IDH is mutated.
- Values: `true` - mutant, `false` - wild type.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": false }`.

**`idhMutation`** - IDH mutation.
- Options: `IDH1 R132H`, `IDH1 R132C`, `IDH1 R132G`, `IDH1 R132L`, `IDH1 R132S`, `IDH2 R172G`, `IDH2 R172W`, `IDH2 R172K`, `IDH2 R172T`, `IDH2 R172M`, `IDH2 R172S`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["IDH1 R132H"] }`.

**`tertStatus`** - Whether TERT is mutated.
- Values: `true` - mutant, `false` - wild type.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`tertMutation`** - TERT mutation.
- Options: `C228T`, `C250T`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["C228T"] }`.

**`expressionSubtype`** - Gene expression subtype.
- Options: `Classical`, `Mesenchymal`, `Proneural`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["Mesenchymal"] }`.

**`methylationSubtype`** - Methylation subtype.
- Options: `H3-K27`, `H3-G34`, `RTKI`, `RTKII`, `Mesenchymal`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["RTKI"] }`.

**`gcimpMethylation`** - Whether the specimen has G-CIMP methylation.
- Values: `true` - yes, `false` - no.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`geneKnockout`** - Gene knocked out in the specimen (any of the specimen's knockouts).
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Exact](./search-criteria.md#value-matching).
- Example: `{ "value": ["TP53"] }`.


## Intervention Fields
Not applied to materials.

**`intervention`** - Type of any of the specimen's interventions.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Temozolomide"] }`.


## Drug Screening Fields
Not applied to materials.

**`drug`** - Name of a drug tested in the specimen's drug screening.
- Filter: [Values](./search-criteria.md#values-criteria).
- Matching: [Text](./search-criteria.md#value-matching).
- Example: `{ "value": ["Temozolomide"] }`.

**`dss`** - Drug sensitivity score (DSS) of any drug screening entry.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 10, "to": 50 } }`.

**`dssS`** - Selective drug sensitivity score (difference between the DSS of samples and controls) of any drug screening entry.
- Filter: [Range](./search-criteria.md#range-criteria), decimals.
- Example: `{ "value": { "from": 10, "to": 50 } }`.

Drug screening entries are not indexed as nested objects: `drug`, `dss` and
`dssS` can be satisfied by different entries of the same specimen.


## Not Applied
The type specific criteria classes inherit `specimenType` and the
[data availability criteria](./search-criteria.md#data-availability-criteria)
from the common specimen criteria, but the search engine does not apply them in
these groups. Use the [`specimen`](search-criteria-specimens.md) group instead.


## Example 1
Specimens with a methylated MGMT promoter **and** IDH wild type.
```json
{
    "mgmtStatus": { "value": true },
    "idhStatus": { "value": false }
}
```

## Example 2
Specimens with ID `1` **or** `2`.
```json
{
    "id": { "value": [1, 2] }
}
```


##
- All filters are optional and empty by default.
- Values of one filter are combined with logical `OR` operator.
- Different filters are combined with logical `AND` operator.
- Any filter can be negated with `"not": true`. When all filters of a type specific group are negated, the specimen stage is an exclusion, see [Negative Filters](./search-criteria.md#negative-filters).
