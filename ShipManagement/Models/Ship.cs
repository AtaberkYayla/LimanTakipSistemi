namespace ShipManagement.Models;

public class Ship
{
    public int ShipId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IMO { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }

    public ICollection<ShipVisit> ShipVisits { get; set; } = new List<ShipVisit>();
    public ICollection<Cargo> Cargoes { get; set; } = new List<Cargo>();
    public ICollection<ShipCrewAssignment> CrewAssignments { get; set; } = new List<ShipCrewAssignment>();
}