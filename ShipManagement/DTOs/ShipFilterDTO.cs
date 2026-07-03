namespace ShipManagement.DTOs;

public class ShipFilterDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Flag { get; set; }
    public int? YearBuiltMin { get; set; }
    public int? YearBuiltMax { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}