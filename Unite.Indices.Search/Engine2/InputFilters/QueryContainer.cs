using System.Reflection;
using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using CnvProfileIndex = Unite.Indices.Entities.CnvProfiles.CnvProfileIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneExpressionIndex = Unite.Indices.Entities.Genes.GeneExpressionIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using ProteinExpressionIndex = Unite.Indices.Entities.Proteins.ProteinExpressionIndex;
using ProteinIndex = Unite.Indices.Entities.Proteins.ProteinIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;

namespace Unite.Indices.Search.Engine2.InputFilters;

public interface IDocumentQuery
{
    public Type DocumentType { get; }
    public FilterGroup FilterGroup { get; }
}

public class DocumentQuery<TDocument> : IDocumentQuery
{
    public FilterGroup FilterGroup { get; set; }

    public Type DocumentType => typeof(TDocument);
}

public class QueryContainer
{
    private static readonly Dictionary<Type, PropertyInfo> _documentQueryProperties = typeof(QueryContainer)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(DocumentQuery<>))
        .ToDictionary(property => property.PropertyType.GetGenericArguments()[0]);

    public DocumentQuery<ProjectIndex> ProjectQuery { get; set; }
    public DocumentQuery<DonorIndex> DonorQuery { get; set; }
    public DocumentQuery<ImageIndex> ImageQuery { get; set; }
    public DocumentQuery<SpecimenIndex> SpecimenQuery { get; set; }
    public DocumentQuery<GeneIndex> GeneQuery { get; set; }
    public DocumentQuery<SmIndex> SmQuery { get; set; }
    public DocumentQuery<CnvIndex> CnvQuery { get; set; }
    public DocumentQuery<SvIndex> SvQuery { get; set; }
    public DocumentQuery<CnvProfileIndex> CnvProfileQuery { get; set; }
    public DocumentQuery<GeneExpressionIndex> GeneExpressionQuery { get; set; }
    public DocumentQuery<ProteinIndex> ProteinQuery { get; set; }
    public DocumentQuery<ProteinExpressionIndex> ProteinExpressionQuery { get; set; }


    public IDocumentQuery Get(Type documentType)
    {
        return _documentQueryProperties.TryGetValue(documentType, out var property)
            ? (IDocumentQuery)property.GetValue(this)
            : null;
    }

    public DocumentQuery<TDocument> Get<TDocument>()
    {
        return (DocumentQuery<TDocument>)Get(typeof(TDocument));
    }

    public void Set<TDocument>(DocumentQuery<TDocument> documentQuery)
    {
        if (!_documentQueryProperties.TryGetValue(typeof(TDocument), out var property))
            throw new NotSupportedException($"Document type '{typeof(TDocument).Name}' is not supported.");

        property.SetValue(this, documentQuery);
    }

    /// <summary>
    /// Creates a shallow copy: document queries are shared, replace them with <see cref="Set{TDocument}"/> instead of modifying.
    /// </summary>
    public QueryContainer Copy()
    {
        return (QueryContainer)MemberwiseClone();
    }
}
