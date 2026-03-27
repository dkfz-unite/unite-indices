namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class CnvProfileFilterNames
{
    private static string Prefix => "CNV";

    public static string Specimen => $"{Prefix}.{nameof(Specimen)}";
    public static string Chromosome => $"{Prefix}.{nameof(Chromosome)}";
    public static string ChromosomeArm => $"{Prefix}.{nameof(ChromosomeArm)}";
    public static string Gain => $"{Prefix}.{nameof(Gain)}";
    public static string Loss => $"{Prefix}.{nameof(Loss)}";
    public static string Neutral => $"{Prefix}.{nameof(Neutral)}";
}