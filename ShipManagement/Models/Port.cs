namespace ShipManagement.Models;

public class Port
{
    public int PortId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public ICollection<ShipVisit> ShipVisits { get; set; } = new List<ShipVisit>();
}