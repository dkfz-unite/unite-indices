namespace Unite.Indices.Entities.Basic.Images;

public class MrImageIndex : ImageBaseIndex
{
    [Searchable]
    public double? WholeTumor { get; set; }
    [Searchable]
    public double? ContrastEnhancing { get; set; }
    [Searchable]
    public double? NonContrastEnhancing { get; set; }

    [Searchable]
    public double? MedianAdcTumor { get; set; }
    [Searchable]
    public double? MedianAdcCe { get; set; }
    [Searchable]
    public double? MedianAdcEdema { get; set; }

    [Searchable]
    public double? MedianCbfTumor { get; set; }
    [Searchable]
    public double? MedianCbfCe { get; set; }
    [Searchable]
    public double? MedianCbfEdema { get; set; }

    [Searchable]
    public double? MedianCbvTumor { get; set; }
    [Searchable]
    public double? MedianCbvCe { get; set; }
    [Searchable]
    public double? MedianCbvEdema { get; set; }

    [Searchable]
    public double? MedianMttTumor { get; set; }
    [Searchable]
    public double? MedianMttCe { get; set; }
    [Searchable]
    public double? MedianMttEdema { get; set; }
}
