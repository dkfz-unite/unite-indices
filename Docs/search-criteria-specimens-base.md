# Base Specimen Filters Criteria
Base specimen filters criteria. These criteria are part of all specimen specific filters criteria. Actual filters can be found [here](../Unite.Indices.Search/Services/Filters/Base/Specimens/Criteria/SpecimenBaseCriteria.cs).

```jsonc
{
    // General filters
    "id": { "value": [1, 2, 3] },
    "referenceId": { "value": ["T01", "T02", "T03"] },

    // Molecular data filters
    "mgmtStatus": { "value": ["Methylated", "Unmethylated"] },
    "idhStatus": { "value": ["Wildtype", "Mutant"] },
    "idhMutation": { "value": ["IDH2 R172S", "IDH1 R132L"] },
    "geneExpressionSubtype": { "value": ["Mesenchymal", "Proneural", "Classical"] },
    "methylationSubtype": { "value": ["RTKI", "Mesenchymal", "H3-G34"] },
    "gcimpMethylation": { "value": true },

    // Drug screening data filters
    "drug": { "value": ["Temozolomide", "Lomustine"] },
    "dss": { "from": 0.5, "to": 1.0 },
    "dssSelective": { "from": 0.5, "to": 1.0 }
}
```


## General Fields
General specimen filters applicable to any type of the index.

**`id`** - Internal specimen identifier.
- Description: Allows to filter specimens by internal identifiers.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": [1, 2, 3] }`.

**`referenceId`** - External specimen identifier.
- Description: Allows to filter specimens by external identifiers (Which were provided during the data submission).
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["T01", "T02", "T03"] }`.

**`mgmtStatus`** - MGMT promoter methylation status.
- Options: `Methylated`, `Unmethylated`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Methylated"] }`.

**`idhStatus`** - IDH status.
- Options: `Wildtype`, `Mutant`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Wildtype"] }`.

**`idhMutation`** - IDH mutation.
- Notes: Can be set only if `idhStatus` is set to `Mutant`.
- Options: `IDH2 R172S`, `IDH2 R172M`, `IDH2 R172T`, `IDH2 R172W`, `IDH2 R172G`, `IDH1 R132G`, `IDH2 R172K`, `IDH1 R132C`, `IDH1 R132H`, `IDH1 R132L`, `IDH1 R132S`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["IDH2 R172S"] }`.

**`geneExpressionSubtype`** - Gene expression subtype.
- Note: Can be set only if `idhStatus` is set to `WildType`.
- Options: `Mesenchymal`, `Proneural`, `Classical`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Mesenchymal"] }`.

**`methylationSubtype`** - Methylation subtype.
- Note: Can be set only if `idhStatus` is set to `WildType`.
- Options: `RTKI`, `RTKII`, `Mesenchymal`, `H3-K27`, `H3-G34`.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["RTKI"] }`.

**`gcimpMethylation`** - GCIMP methylation status.
- Values: `true` - methylated, `false` - unmethylated.
- Filter: [Boolean](./search-criteria.md#boolean-criteria).
- Example: `{ "value": true }`.

**`drug`** - Drug name tested against the specimen during the drug screening process.
- Filter: [Values](./search-criteria.md#values-criteria).
- Example: `{ "value": ["Temozolomide"] }`.

**`dss`** - Drug sensitivity score (%) (How much the specimen responds to a drug of different concentration levels).
- Notes: Can be set only if `drug` is set.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 50, "to": 100 } }`.

**`dssSelective`** - Drug selective sensitivity score (%) (How much the specimen responds to a drug of different concentration levels compared to other drugs).
- Notes: Can be set only if `drug` is set.
- Filter: [Range](./search-criteria.md#range-criteria).
- Example: `{ "value": { "from": 50, "to": 100 } }`.


## Example 1
Data, where `MGMT` promoter is methylated **and** `IDH` status is `Wildtype`.
```json
{
    "mgmtStatus": { "value": ["Methylated"] },
    "idhStatus": { "value": ["Wildtype"] }
}
```

## Example 2
Data, where specimens have ID `1` **or** `2`.
```json
{
    "id": { "value": [1, 2] }
}
```


##
- All filters are optional and empty by default.
- Filters applied to the same field are combined with logical `OR` operator.
- Filters applied to different fields are combined with logical `AND` operator.
