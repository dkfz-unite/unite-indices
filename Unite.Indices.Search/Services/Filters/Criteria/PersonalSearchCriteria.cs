namespace Unite.Indices.Search.Services.Filters.Criteria;

public class PersonalSearchCriteria
{
    public int? UserId { get; }
    public bool? IsRoot { get; }
    public SearchCriteria SearchCriteria { get; set; }

    public PersonalSearchCriteria(int? userId, bool? isRoot, SearchCriteria searchCriteria)
    {
        UserId = userId;
        IsRoot = isRoot;
        SearchCriteria = searchCriteria;
    }
}