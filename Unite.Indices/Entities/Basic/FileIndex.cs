using System;

namespace Unite.Indices.Entities.Basic
{
    public class FileIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}

