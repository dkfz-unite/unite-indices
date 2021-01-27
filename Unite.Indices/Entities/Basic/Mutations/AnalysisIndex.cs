using System;

namespace Unite.Indices.Entities.Basic.Mutations
{
    public class AnalysisIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }

        public FileIndex File { get; set; }
    }
}
