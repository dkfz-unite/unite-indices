using System;

namespace Unite.Indices.Entities.Basic.Samples
{
    public class CellLineIndex
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Species { get; set; }

        public string DepositorName { get; set; }
        public string DepositorEstablishment { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public string PublicationId { get; set; }
        public string AtccId { get; set; }
        public string ExPasyId { get; set; }
    }
}
