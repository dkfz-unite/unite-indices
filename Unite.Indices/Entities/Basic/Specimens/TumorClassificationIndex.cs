namespace Unite.Indices.Entities.Basic.Specimens;

public class TumorClassificationIndex
{
    public string Superfamily { get; set; }
    public double? SuperfamilyScore { get; set; }
    public string Family { get; set; }
    public double? FamilyScore { get; set; }
    public string Class { get; set; }
    public double? ClassScore { get; set; }
    public string Subclass { get; set; }
    public double? SubclassScore { get; set; }
}
