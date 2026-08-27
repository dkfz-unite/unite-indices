namespace Unite.Indices.Search.Services.Filters.Criteria;

public class PersonalGetCriteria
{
    public string Key { get; set; }
    public UserClaims UserClaims { get; set; }
    
    public PersonalGetCriteria(string key, UserClaims userClaims)
    {
        Key = key;
        UserClaims = userClaims;
    }
}