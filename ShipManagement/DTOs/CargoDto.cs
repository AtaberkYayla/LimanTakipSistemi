namespace ShipManagement.DTOs;

public class CargoDto
{
    public int CargoId { get; set; }
    public int ShipId { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal WeightTon { get; set; }
    public string CargoType { get; set; } = string.Empty;
}

public class CreateCargoDto
{
    public int ShipId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal WeightTon { get; set; }
    public string CargoType { get; set; } = string.Empty;
}

public class UpdateCargoDto
{
    public int ShipId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal WeightTon { get; set; }
    public string CargoType { get; set; } = string.Empty;
}