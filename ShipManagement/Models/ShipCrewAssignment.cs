namespace ShipManagement.Models;

public class ShipCrewAssignment
{
    public int AssignmentId { get; set; }
    public int ShipId { get; set; }
    public int CrewId { get; set; }
    public DateTime AssignmentDate { get; set; }

    public Ship Ship { get; set; } = null!;
    public CrewMember CrewMember { get; set; } = null!;
}