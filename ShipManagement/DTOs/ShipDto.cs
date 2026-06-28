namespace ShipManagement.DTOs;

public class ShipDto
{
    public int ShipId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IMO { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
}

public class CreateShipDto
{
    public string Name { get; set; } = string.Empty;
    public string IMO { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
}

public class UpdateShipDto
{
    public string Name { get; set; } = string.Empty;
    public string IMO { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public int YearBuilt { get; set; }
}