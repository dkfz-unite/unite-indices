namespace Unite.Indices.Entities.Basic.Specimens.Drugs;

public class DrugScreeningIndex
{
    [Searchable]
    public string Drug { get; set; }
    public double? Gof { get; set; }
    [Searchable]
    public double? Dss { get; set; }
    [Searchable]
    public double? DssS { get; set; }
    public double? DoseMin { get; set; }
    public double? DoseMax { get; set; }
    public double? Dose25 { get; set; }
    public double? Dose50 { get; set; }
    public double? Dose75 { get; set; }
}
