namespace Unite.Indices.Entities.Basic.Specimens;

public class OrganoidIndex
{
    public string ReferenceId { get; set; }

    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }
    public string Medium { get; set; }

    public OrganoidInterventionIndex[] Interventions { get; set; }
}
