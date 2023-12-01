using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Indices.Search.Services.Filters;

public class GeneIndexFiltersCollection : FiltersCollection<GeneIndex>
{
    public GeneIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<GeneIndex>(criteria.Donor, gene => gene.Specimens.First().Donor);
        var mriImageFilters = new MriImageFilters<GeneIndex>(criteria.Mri, gene => gene.Specimens.First().Images.First());
        var tissueFilters = new TissueFilters<GeneIndex>(criteria.Tissue, gene => gene.Specimens.First());
        var cellLineFilters = new CellLineFilters<GeneIndex>(criteria.Cell, gene => gene.Specimens.First());
        var organoidFilters = new OrganoidFilters<GeneIndex>(criteria.Organoid, gene => gene.Specimens.First());
        var xenograftFilters = new XenograftFilters<GeneIndex>(criteria.Xenograft, gene => gene.Specimens.First());
        var geneFilters = new GeneFilters<GeneIndex>(criteria.Gene, gene => gene);
        var mutationFilters = new MutationFilters<GeneIndex>(criteria.Ssm, gene => gene.Specimens.First().Variants.First());
        var copyNumberVariantFilters = new CopyNumberVariantFilters<GeneIndex>(criteria.Cnv, gene => gene.Specimens.First().Variants.First());
        var structuralVariantFilters = new StructuralVariantFilters<GeneIndex>(criteria.Sv, gene => gene.Specimens.First().Variants.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());
        _filters.AddRange(copyNumberVariantFilters.All());
        _filters.AddRange(structuralVariantFilters.All());

        if (IsNotEmpty(criteria.Image?.Id))
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
              ImageFilterNames.Id,
              gene => gene.Specimens.First().Images.First().Id,
              criteria.Image.Id
              ));
        }

        if (IsNotEmpty(criteria.Specimen?.Id))
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
                SpecimenFilterNames.Id,
                gene => gene.Specimens.First().Id,
                criteria.Specimen.Id
            ));
        }

        if (IsNotEmpty(criteria.Gene?.HasVariants))
        {
            _filters.Add(new GreaterThanFilter<GeneIndex, double?>(
                GeneFilterNames.HasVariants, 1,
                gene => gene.NumberOfSsms,
                gene => gene.NumberOfCnvs,
                gene => gene.NumberOfSvs
            ));
        }

        if (IsNotEmpty(criteria.Gene?.HasExpressions))
        {
            _filters.Add(new NotNullFilter<GeneIndex, object>(
                GeneFilterNames.HasExpressions,
                gene => gene.Specimens.First().Expression
            ));
        }
    }
}
