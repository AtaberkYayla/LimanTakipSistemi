namespace ShipManagement.DTOs;

public class ShipCrewAssignmentDto
{
    public int AssignmentId { get; set; }
    public int ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public int CrewId { get; set; }
    public string CrewFullName { get; set; } = string.Empty;
    public DateTime AssignmentDate { get; set; }
}

public class CreateShipCrewAssignmentDto
{
    public int ShipId { get; set; }
    public int CrewId { get; set; }
    public DateTime AssignmentDate { get; set; }
}

public class UpdateShipCrewAssignmentDto
{
    public int ShipId { get; set; }
    public int CrewId { get; set; }
    public DateTime AssignmentDate { get; set; }
}