namespace Unite.Indices.Entities.Projects;

public class LineStatsIndex
{
    public int TotalLines { get; set; }
    public Stat<int>[] ByDrugDss { get; set; } // Top // drugsscreening dss-score = drugsensitivityscore (for tumor cells) the higher the better
    public Stat<int>[] ByDrugSdss { get; set; } // Top // drugsscreening sdss-score = sensitivity score for both tumor and healthy cells the higher the better 
    // public Stat[] ByInterventionsType { get; set; } // idea not yet baked
}
