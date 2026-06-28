namespace ShipManagement.Models;

public class ShipVisit
{
    public int VisitId { get; set; }
    public int ShipId { get; set; }
    public int PortId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string Purpose { get; set; } = string.Empty;

    public Ship Ship { get; set; } = null!;
    public Port Port { get; set; } = null!;
}