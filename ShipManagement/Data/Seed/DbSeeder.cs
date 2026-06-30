using ShipManagement.Data;
using ShipManagement.Models;

namespace ShipManagement.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Ships.Any()) return; // Zaten veri varsa seed etme

        // Gemiler
        var ships = new List<Ship>
        {
            new() { Name = "Ever Given", IMO = "9811000", Type = "Konteyner", Flag = "Panama", YearBuilt = 2018 },
            new() { Name = "MSC Oscar", IMO = "9703291", Type = "Konteyner", Flag = "Panama", YearBuilt = 2014 },
            new() { Name = "Maersk Alabama", IMO = "9164263", Type = "Kargo", Flag = "ABD", YearBuilt = 1998 },
            new() { Name = "Türkiye", IMO = "9321483", Type = "Tanker", Flag = "Türkiye", YearBuilt = 2006 },
        };
        await context.Ships.AddRangeAsync(ships);

        // Limanlar
        var ports = new List<Port>
        {
            new() { Name = "İzmir Alsancak Limanı", Country = "Türkiye", City = "İzmir" },
            new() { Name = "İstanbul Ambarlı Limanı", Country = "Türkiye", City = "İstanbul" },
            new() { Name = "Mersin Uluslararası Limanı", Country = "Türkiye", City = "Mersin" },
            new() { Name = "Port Said", Country = "Mısır", City = "Port Said" },
        };
        await context.Ports.AddRangeAsync(ports);

        // Mürettebat
        var crew = new List<CrewMember>
        {
            new() { FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet.yilmaz@example.com", PhoneNumber = "+90 532 123 45 67", Role = "Kaptan" },
            new() { FirstName = "Mehmet", LastName = "Demir", Email = "mehmet.demir@example.com", PhoneNumber = "+90 533 234 56 78", Role = "Güverte Subayı" },
            new() { FirstName = "Ali", LastName = "Kaya", Email = "ali.kaya@example.com", PhoneNumber = "+90 534 345 67 89", Role = "Baş Mühendis" },
            new() { FirstName = "Ayşe", LastName = "Çelik", Email = "ayse.celik@example.com", PhoneNumber = "+90 535 456 78 90", Role = "Seyir Subayı" },
        };
        await context.CrewMembers.AddRangeAsync(crew);

        await context.SaveChangesAsync();

        // Ziyaretler
        var visits = new List<ShipVisit>
        {
            new() { ShipId = ships[0].ShipId, PortId = ports[0].PortId, ArrivalDate = DateTime.Now.AddDays(-10), DepartureDate = DateTime.Now.AddDays(-7), Purpose = "Yükleme" },
            new() { ShipId = ships[1].ShipId, PortId = ports[1].PortId, ArrivalDate = DateTime.Now.AddDays(-5), DepartureDate = DateTime.Now.AddDays(-2), Purpose = "Boşaltma" },
            new() { ShipId = ships[2].ShipId, PortId = ports[2].PortId, ArrivalDate = DateTime.Now.AddDays(-3), Purpose = "Bakım" },
            new() { ShipId = ships[3].ShipId, PortId = ports[3].PortId, ArrivalDate = DateTime.Now.AddDays(-1), Purpose = "Yükleme" },
        };
        await context.ShipVisits.AddRangeAsync(visits);

        // Yükler
        var cargoes = new List<Cargo>
        {
            new() { ShipId = ships[0].ShipId, Description = "Elektronik Eşya", WeightTon = 15000, CargoType = "Genel" },
            new() { ShipId = ships[1].ShipId, Description = "Ham Petrol", WeightTon = 80000, CargoType = "Tehlikeli" },
            new() { ShipId = ships[2].ShipId, Description = "Gıda Maddeleri", WeightTon = 5000, CargoType = "Gıda" },
            new() { ShipId = ships[3].ShipId, Description = "Kimyasal Madde", WeightTon = 12000, CargoType = "Tehlikeli" },
        };
        await context.Cargoes.AddRangeAsync(cargoes);

        // Atamalar
        var assignments = new List<ShipCrewAssignment>
        {
            new() { ShipId = ships[0].ShipId, CrewId = crew[0].CrewId, AssignmentDate = DateTime.Now.AddDays(-30) },
            new() { ShipId = ships[1].ShipId, CrewId = crew[1].CrewId, AssignmentDate = DateTime.Now.AddDays(-20) },
            new() { ShipId = ships[2].ShipId, CrewId = crew[2].CrewId, AssignmentDate = DateTime.Now.AddDays(-15) },
            new() { ShipId = ships[3].ShipId, CrewId = crew[3].CrewId, AssignmentDate = DateTime.Now.AddDays(-10) },
        };
        await context.ShipCrewAssignments.AddRangeAsync(assignments);

        await context.SaveChangesAsync();
    }
}