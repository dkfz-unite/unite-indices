namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Donors.ProjectIndex
{
    public DonorStatsIndex DonorStats { get; set; }
    public ImageStatsIndex MriStats { get; set; } // make different classes on need
    public ImageStatsIndex CtStats { get; set; } // make different classes on need
    public MaterialStatsIndex MaterialStats { get; set; }
    public LineStatsIndex LineStats { get; set; }
    public OrganoidStatsIndex OrganoidStats { get; set; }
    public XenograftStatsIndex XenograftStats { get; set; }
    public SsmStatsIndex SsmStats { get; set; }
    public CnvStatsIndex CnvStats { get; set; }
    public SvStatsIndex SvStats { get; set; }
    public TranscriptomicStatsIndex TranscriptomicStats { get; set; }
}
