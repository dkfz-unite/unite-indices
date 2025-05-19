# Search Engine
Search engine allows to search for data indexed by the system. It is based on [Elasticsearch](https://www.elastic.co/).

There are different types of indices available, each has own structure most suitable for fast and convenient search:
- [**Donors**](#donors-index) - donors centric index.
    - Allows to search for donors and their related information.
    - Index is populated by [Donors Feed](https://github.com/dkfz-unite/unite-donors-feed) service.
- [**Images**](#images-index) - images centric index.
    - Allows to search for images and their related information.
    - All images share the same index and can be of one of the following types: `MR` or `CT`.
    - Index is populated by [Images Feed](https://github.com/dkfz-unite/unite-images-feed) service.
- [**Specimens**](#specimens-index) - specimens centric index.
    - Allows to search for specimens and their related information.
    - All specimen share the same index and can be of one of the following types: `Material`, `Line`, `Organoid` or `Xenograft`.
    - Index is populated by [Specimens Feed](https://github.com/dkfz-unite/unite-specimens-feed) service.
- [**Genes**](#genes-index) - genes centric index.
    - Allows to search for genes and their related information.
    - Index is populated by [Omics Feed](https://github.com/dkfz-unite/unite-omics-feed) service.
- [**Variants**](#variants-index) - variants centric index.
    - Allows to search for variants and their related information.
    - All variants share the same index and can be of one of the following types: `SM`, `CNV`, or `SV`.
    - Index is populated by [Omics Feed](https://github.com/dkfz-unite/unite-omics-feed) service.

All the data in different indices can be filtered by cross reference search [criteria](./search-criteria.md).


## Donors Index
Donors index does not iclude information about genes and variants. All gene and variant filters lead to subsequent aggregation query in corresponding indices, results are then merged with donors search results.

Index has the following structure:
- [Donor](../Unite.Indices/Entities/Donors/DonorIndex.cs): [DonorBase](../Unite.Indices/Entities/Basic/Donors/DonorIndex.cs)
    - Images - [ImageBase](../Unite.Indices/Entities/Basic/Images/ImageIndex.cs)[]
    - Specimens - [SpecimenBase](../Unite.Indices/Entities/Basic/Specimens/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs) availability fields
    - Statistics - Statistics fields

It means that a sigle donor can have multiple images and specimens associated with it. Data availability and statistics fields are pre-calculated during indexing process and depend on which data is assosiated with current donor. Donors can also have genes (expression data) and variants related to them, but they are in separate indices.


## Images Index
Images index does not iclude information about genes and variants. All gene and variant filters lead to subsequent aggregation query in corresponding indices, results are then merged with images search results.

Index has the following structure:
- [Image](../Unite.Indices/Entities/Images/ImageIndex.cs): [ImageBase](../Unite.Indices/Entities/Basic/Images/ImageIndex.cs)
    - Donor - [DonorBase](../Unite.Indices/Entities/Basic/Donors/DonorIndex.cs)
    - Specimens - [SpecimenBase](../Unite.Indices/Entities/Basic/Specimens/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs) availability fields
    - Statistics - Statistics fields

It means that a sigle image can have single donor and multiple specimens associated with it. Data availability and statistics fields are pre-calculated during indexing process and depend on which data is assosiated with current image. Images can also have genes (expression data) and variants related to them, but they are in separate indices.

## Specimens Index
Specimens index does not iclude information about genes and variants. All gene and variant filters lead to subsequent aggregation query in corresponding indices, results are then merged with specimens search results.

Index has the following structure:
- [Specimen](../Unite.Indices/Entities/Specimens/SpecimenIndex.cs): [SpecimenBase](../Unite.Indices/Entities/Basic/Specimens/SpecimenIndex.cs)
    - Donor - [DonorBase](../Unite.Indices/Entities/Basic/Donors/DonorIndex.cs)
    - Images - [ImageBase](../Unite.Indices/Entities/Basic/Images/ImageIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs) availability fields
    - Statistics - Statistics fields

It means that a sigle specimen can have single donor and multiple images associated with it. Data availability and statistics fields are pre-calculated during indexing process and depend on which data is assosiated with current specimen. Specimens can also have genes (expression data) and variants related to them, but they are in separate indices.

## Genes Index
Genes index is self-contained and includes all information about associated entities. This is the most advanced index.

Index has the following structure:
- [Gene](../Unite.Indices/Entities/Genes/GeneIndex.cs): [GeneBase](../Unite.Indices/Entities/Basic/Omics/GeneIndex.cs)
    - Specimens - [SpecimenBase](../Unite.Indices/Entities/Basic/Specimens/SpecimenIndex.cs)[]
        - Expression - [Expression](../Unite.Indices/Entities/Genes/BulkExpressionIndex.cs) data
        - Donor - [DonorBase](../Unite.Indices/Entities/Basic/Donors/DonorIndex.cs)
        - Images - [ImageBase](../Unite.Indices/Entities/Basic/Images/ImageIndex.cs)[]
        - Variants - [VariantBase](../Unite.Indices/Entities/Basic/Omics/Variants/VariantIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs) availability fields
    - Statistics - Statistics fields

It means that a sigle gene can be affected by multiple variants and have expression data in multiple specimens, where each specimen is associated with single donor and multiple images. Variants are associated with the gene during Ensembl VEP annotation process. Data availability and statistics fields are pre-calculated during indexing process and depend on which data is assosiated with current gene.

## Variants Index
Variants index is self-contained and includes information about associated entities (except gene expression data).

Index has the following structure:
- [Variant](../Unite.Indices/Entities/Variants/VariantIndex.cs): [VariantBase](../Unite.Indices/Entities/Basic/Omics/Variants/VariantIndex.cs)
    - Genes - [GeneBase](../Unite.Indices/Entities/Basic/Omics/GeneIndex.cs)[]
    - Specimens - [SpecimenBase](../Unite.Indices/Entities/Basic/Specimens/SpecimenIndex.cs)[]
        - Donor - [DonorBase](../Unite.Indices/Entities/Basic/Donors/DonorIndex.cs)
        - Images - [ImageBase](../Unite.Indices/Entities/Basic/Images/ImageIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs) availability fields
    - Statistics - Statistics fields

It means that a sigle variant can affect multiple genes and be present in multiple specimens, where each specimen is associated with single donor and multiple images. Genes are associated with the variant during Ensembl VEP annotation process as affected transcripts. Data availability and statistics fields are pre-calculated during indexing process and depend on which data is assosiated with current variant. 
