﻿namespace Unite.Indices.Entities.Basic.Specimens
{
    public class SpecimenIndex
    {
        public int Id { get; set; }
        
        public TissueIndex Tissue { get; set; }
        public CellLineIndex CellLine { get; set; }
        public OrganoidIndex Organoid { get; set; }
        public XenograftIndex Xenograft { get; set; }
    }
}
