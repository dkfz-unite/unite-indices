namespace Unite.Indices.Search.Services.Filters.Base.Projects.Constants;

public class ProjectsFilterNames : DataFilterNames
{
    protected override string Prefix => "Projects";

    public string Id => $"{Prefix}.Id";
    public string Name => $"{Prefix}.Name";
}
