namespace Unite.Indices.Search.Services.Filters.Criteria;

public class PersonalSearchCriteria
{
    public UserClaims UserClaims { get; set; }
    public SearchCriteria SearchCriteria { get; set; }

    public PersonalSearchCriteria(int? userId, bool? isRoot, SearchCriteria searchCriteria)
    {
        UserClaims = new UserClaims(userId, isRoot);
        SearchCriteria = searchCriteria;
    }
}