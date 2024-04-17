namespace Unite.Indices.Entities.Projects;

public class SvStatsIndex
{
    // Svs // SV types(DUP, TDUP, INS, DEL, INV. ITX, CTX)
    public int Total { get; set; } // Total number of donors having svs
    // DupByRange (top like 3 ranges) about genomes
    public Stat<int[]>[] DupByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] TDupByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] InsByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] DelByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] InvByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] ItxByGene { get; set; } // chromNum, start, end
    public Stat<int[]>[] CtxByGene { get; set; } // chromNum, start, end
    // public Stat[] GenesAffectedBySvs { get; set; }
}
