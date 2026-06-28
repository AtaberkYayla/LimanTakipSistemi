namespace ShipManagement.Models;

public class CrewMember
{
    public int CrewId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<ShipCrewAssignment> Assignments { get; set; } = new List<ShipCrewAssignment>();
}