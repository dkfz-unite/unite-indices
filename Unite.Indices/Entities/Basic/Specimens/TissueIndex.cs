﻿using System;
namespace Unite.Indices.Entities.Basic.Specimens
{
    public class TissueIndex
    {
        public string ReferenceId { get; set; }
        public string Type { get; set; }
        public string TumourType { get; set; }
        public string Source { get; set; }
        public DateTime? ExtractionDate { get; set; }
    }
}
