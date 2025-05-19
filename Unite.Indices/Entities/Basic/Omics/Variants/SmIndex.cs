namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class SmIndex : VariantBaseIndex
{
    private string _change;

    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }
    public string Change { get => _change ?? GetDnaChange(); set => _change = value; }


    private string GetDnaChange()
    {
        var none = "-";
        return $"{Ref ?? none}>{Alt ?? none}";
    }
}
