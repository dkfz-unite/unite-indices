namespace Unite.Indices.Entities.Projects;

public class CnvStatsIndex
{
    // Cnvs // Gain, Neutral, Loss // LOH / DEL
    // DKFZ
    // Gain
    // Gain Loh
    // Neutral
    // Neutral Loh
    // Loss Del
    // Loss Loh

    // Other or all...
    // Gain
    // Gain Loh
    // Neutral
    // Neutral Loh
    // Loss
    // Loss Del
    // Loss Loh

    public int Total { get; set; } // Total number of donors having cnvs
    public Stat<int[]>[] GainByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] GainLohByRange { get; set; } // chromNum, start, end
    // public Stat<int[]>[] NeutralByRange { get; set; } // not needed
    public Stat<int[]>[] NeutralLohByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] LossByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] LossDelByRange { get; set; } // chromNum, start, end
    public Stat<int[]>[] LossLohByRange { get; set; } // chromNum, start, end
    // public int SomethingLikeSimilarAreaInGenomeNarrowItUpHowToDo { get; set; }
    // public Stat[] NumberOfCnvsByType { get; set; }
}
