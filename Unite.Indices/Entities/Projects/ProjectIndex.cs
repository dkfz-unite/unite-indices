namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Donors.ProjectIndex
{
    public DonorStatsIndex DonorStats { get; set; }
    public ImageStatsIndex ImageStats { get; set; }
    public SpecimenStatsIndex MaterialStats { get; set; }
    public VariantStatsIndex VariantStats { get; set; }
    public TranscriptomicStatsIndex TranscriptomicStats { get; set; }
}
