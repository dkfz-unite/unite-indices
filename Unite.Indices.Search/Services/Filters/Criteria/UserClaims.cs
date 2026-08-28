namespace Unite.Indices.Search.Services.Filters.Criteria;

public class UserClaims
{
    public int? UserId { get; }
    public bool? IsRoot { get; }
    
    public UserClaims(int? userId, bool? isRoot)
    {
        UserId = userId;
        IsRoot = isRoot;
    }
}