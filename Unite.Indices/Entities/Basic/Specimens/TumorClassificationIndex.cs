namespace Unite.Indices.Entities.Basic.Specimens;

public class TumorClassificationIndex
{
    [Searchable]
    public string Superfamily { get; set; }
    public double? SuperfamilyScore { get; set; }
    [Searchable]
    public string Family { get; set; }
    public double? FamilyScore { get; set; }
    [Searchable]
    public string Class { get; set; }
    public double? ClassScore { get; set; }
    [Searchable]
    public string Subclass { get; set; }
    public double? SubclassScore { get; set; }
}
