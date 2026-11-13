using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria.Properties;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Searchable property of an index document. In JSON it is given by its name (e.g. "specimens.id").
/// </summary>
[JsonConverter(typeof(SearchFieldJsonConverter))]
public abstract class SearchField
{
    public string Name { get; }

    protected SearchField(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Field reference by name only, resolved to a typed field of the document later.
    /// </summary>
    public static implicit operator SearchField(string name) => name == null ? null : new UnresolvedSearchField(name);

    public override string ToString() => Name;
}

/// <summary>
/// Searchable property of documents of type <typeparamref name="TDocument"/>.
/// </summary>
public abstract class SearchField<TDocument> : SearchField where TDocument : class
{
    /// <summary>
    /// Type of the property value (element type for collections).
    /// </summary>
    public abstract Type ValueType { get; }

    protected SearchField(string name) : base(name)
    {
    }

    public abstract IFilter<TDocument> CreateFilter(PropertyCriteria criteria);
}

/// <summary>
/// Searchable property of documents of type <typeparamref name="TDocument"/> with value type <typeparamref name="TProp"/>.
/// </summary>
public sealed class SearchField<TDocument, TProp> : SearchField<TDocument> where TDocument : class
{
    public Expression<Func<TDocument, TProp>> Property { get; }

    /// <summary>
    /// Property for exact (terms) matching: strings are matched on their "keyword" sub-field.
    /// </summary>
    public Expression<Func<TDocument, object>> TermProperty { get; }

    public override Type ValueType => typeof(TProp);


    public SearchField(string name, Expression<Func<TDocument, TProp>> property) : base(name)
    {
        Property = property;

        var termProperty = Expression.Lambda<Func<TDocument, object>>(Expression.Convert(property.Body, typeof(object)), property.Parameters);

        TermProperty = typeof(TProp) == typeof(string) ? termProperty.AppendSuffix("keyword") : termProperty;
    }


    public override IFilter<TDocument> CreateFilter(PropertyCriteria criteria)
    {
        return criteria.Accept(new FilterBuilder<TDocument, TProp>(this));
    }
}

/// <summary>
/// Field given by name only (e.g. read from JSON), not yet resolved to a document property.
/// </summary>
internal sealed class UnresolvedSearchField : SearchField
{
    public UnresolvedSearchField(string name) : base(name)
    {
    }
}
