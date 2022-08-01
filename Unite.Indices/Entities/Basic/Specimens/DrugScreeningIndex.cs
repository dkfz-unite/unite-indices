namespace Unite.Indices.Entities.Basic.Specimens;

public class DrugScreeningIndex
{
    public string Drug { get; set; }
    public double? Dss { get; set; }
    public double? DssSelective { get; set; }
    public double? Gof { get; set; }
    public double? MinConcentration { get; set; }
    public double? MaxConcentration { get; set; }
    public double? AbsIC25 { get; set; }
    public double? AbsIC50 { get; set; }
    public double? AbsIC75 { get; set; }
}
