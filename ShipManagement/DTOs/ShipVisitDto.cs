namespace ShipManagement.DTOs;

public class ShipVisitDto
{
    public int VisitId { get; set; }
    public int ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public int PortId { get; set; }
    public string PortName { get; set; } = string.Empty;
    public DateTime ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
}

public class CreateShipVisitDto
{
    public int ShipId { get; set; }
    public int PortId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
}

public class UpdateShipVisitDto
{
    public int ShipId { get; set; }
    public int PortId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
}