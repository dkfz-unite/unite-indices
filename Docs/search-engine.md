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
- [**SMs**](#sms-index) - simple mutations centric index.
    - Allows to search for simple mutations and their related information.
    - Index is populated by [Omics Feed](https://github.com/dkfz-unite/unite-omics-feed) service.
- [**CNVs**](#cnvs-index) - copy number variants centric index.
    - Allows to search for copy number variants and their related information.
    - Index is populated by [Omics Feed](https://github.com/dkfz-unite/unite-omics-feed) service.
- [**SVs**](#svs-index) - structural variants centric index.
    - Allows to search for structural variants and their related information.
    - Index is populated by [Omics Feed](https://github.com/dkfz-unite/unite-omics-feed) service.

All the data in different indices can be filtered by cross reference search [criteria](./search-criteria.md).


## Donors Index
Donors index has denormalized structure containing donor related information.  
It also contains navigation fields to images and specimens associated with the donor, available data and statistics.  
Specimens index contains information about related samples, their resources and available data.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [Donor](../Unite.Indices/Entities/Donors/DonorIndex.cs)
    - Images - [Image](../Unite.Indices/Entities/Donors/ImageIndex.cs)[]
    - Specimens - [Specimen](../Unite.Indices/Entities/Donors/SpecimenIndex.cs)[]
        - Samples - [Sample](../Unite.Indices/Entities/Donors/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Donors/StatsIndex.cs)

## Images Index
Images index has denormalized structure containing image related information.  
It also contains navigation fields to donor and specimens associated with the image, available data and statistics.  
Specimens index contains information about related samples, their resources and available data.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [Image](../Unite.Indices/Entities/Images/ImageIndex.cs)
    - Donor - [Donor](../Unite.Indices/Entities/Images/DonorIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Images/SpecimenIndex.cs)[]
        - Samples - [Sample](../Unite.Indices/Entities/Images/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Images/StatsIndex.cs)

## Specimens Index
Specimens index has denormalized structure containing specimen related information.  
It also contains navigation fields to donor and images associated with the specimen, available data and statistics.  
Specimens index contains information about related samples, their resources and available data.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [Specimen](../Unite.Indices/Entities/Specimens/SpecimenIndex.cs)
    - Donor - [Donor](../Unite.Indices/Entities/Specimens/DonorIndex.cs)
    - Images - [Image](../Unite.Indices/Entities/Specimens/ImageIndex.cs)[]
    - Samples - [Sample](../Unite.Indices/Entities/Specimens/SampleIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Specimens/StatsIndex.cs)

## Genes Index
Genes index has denormalized structure containing gene related information.  
It also contains navigation fields to specimens having variants or expression data associated with the gene, available data and statistics.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [Gene](../Unite.Indices/Entities/Genes/GeneIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Genes/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Genes/StatisticsIndex.cs)

## SMs Index
SMs index has denormalized structure containing simple mutation related information.  
It also contains navigation fields to specimens having variants associated with the simple mutation, available data and statistics.  
SMs index includes information about affected genes.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [SM](../Unite.Indices/Entities/Variants/SmIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Variants/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Variants/StatsIndex.cs)

## CNVs Index
CNVs index has denormalized structure containing copy number variant related information.  
It also contains navigation fields to specimens having variants associated with the copy number variant, available data and statistics.  
CNVs index includes information about affected genes.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [CNV](../Unite.Indices/Entities/Variants/CnvIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Variants/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Variants/StatsIndex.cs)

## SVs Index
SVs index has denormalized structure containing structural variant related information.  
It also contains navigation fields to specimens having variants associated with the structural variant, available data and statistics.  
SVs index includes information about affected genes.  
Search requests containing other data types lead to subsequent aggregation queries in corresponding indices and merging of results.

Index has the following structure:
- [SV](../Unite.Indices/Entities/Variants/SvIndex.cs)
    - Specimens - [Specimen](../Unite.Indices/Entities/Variants/SpecimenIndex.cs)[]
    - Data - [Data](../Unite.Indices/Entities/DataIndex.cs)
    - Statistics - [Stats](../Unite.Indices/Entities/Variants/StatsIndex.cs)
