namespace Unite.Indices.Entities.Basic.Epigenetics
{
    public class EpigeneticsDataIndex
    {
        public string GeneExpressionSubtype { get; set; }
        public string IdhStatus { get; set; }
        public string IdhMutation { get; set; }
        public string MethylationStatus { get; set; }
        public string MethylationSubtype { get; set; }
        public bool? GcimpMethylation { get; set; }
    }
}
