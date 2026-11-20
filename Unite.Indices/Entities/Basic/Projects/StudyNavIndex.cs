namespace Unite.Indices.Entities.Basic.Projects;

public class StudyNavIndex
{
    public int Id { get; set; }
    [Searchable]
    public string Name { get; set; }
}
