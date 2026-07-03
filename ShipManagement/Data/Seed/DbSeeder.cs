using ShipManagement.Data;
using ShipManagement.Models;

namespace ShipManagement.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Ships.Any()) return;

        var ships = new List<Ship>
        {
            new() { Name = "Ever Given", IMO = "9811000", Type = "Konteyner", Flag = "Panama", YearBuilt = 2018 },
            new() { Name = "MSC Oscar", IMO = "9703291", Type = "Konteyner", Flag = "Panama", YearBuilt = 2014 },
            new() { Name = "Maersk Alabama", IMO = "9164263", Type = "Kargo", Flag = "ABD", YearBuilt = 1998 },
            new() { Name = "Türkiye", IMO = "9321483", Type = "Tanker", Flag = "Türkiye", YearBuilt = 2006 },
            new() { Name = "CMA CGM Marco Polo", IMO = "9454450", Type = "Konteyner", Flag = "Fransa", YearBuilt = 2012 },
            new() { Name = "Berge Stahl", IMO = "8606145", Type = "Kuru Dökme", Flag = "Norveç", YearBuilt = 1986 },
            new() { Name = "Seawise Giant", IMO = "7381154", Type = "Tanker", Flag = "Singapur", YearBuilt = 1979 },
            new() { Name = "Emma Mærsk", IMO = "9321484", Type = "Konteyner", Flag = "Danimarka", YearBuilt = 2006 },
        };

        ships[7].IMO = "9321484";
        await context.Ships.AddRangeAsync(ships);

        var ports = new List<Port>
        {
            new() { Name = "İzmir Alsancak Limanı", Country = "Türkiye", City = "İzmir" },
            new() { Name = "İstanbul Ambarlı Limanı", Country = "Türkiye", City = "İstanbul" },
            new() { Name = "Mersin Uluslararası Limanı", Country = "Türkiye", City = "Mersin" },
            new() { Name = "Port Said", Country = "Mısır", City = "Port Said" },
            new() { Name = "Rotterdam Limanı", Country = "Hollanda", City = "Rotterdam" },
            new() { Name = "Singapur Limanı", Country = "Singapur", City = "Singapur" },
            new() { Name = "Hamburg Limanı", Country = "Almanya", City = "Hamburg" },
            new() { Name = "Şanghay Limanı", Country = "Çin", City = "Şanghay" },
        };
        await context.Ports.AddRangeAsync(ports);

        // Mürettebat
        var crew = new List<CrewMember>
        {
            new() { FirstName = "Ahmet", LastName = "Yılmaz", Email = "ahmet.yilmaz@example.com", PhoneNumber = "+90 532 123 45 67", Role = "Kaptan" },
            new() { FirstName = "Mehmet", LastName = "Demir", Email = "mehmet.demir@example.com", PhoneNumber = "+90 533 234 56 78", Role = "Güverte Subayı" },
            new() { FirstName = "Ali", LastName = "Kaya", Email = "ali.kaya@example.com", PhoneNumber = "+90 534 345 67 89", Role = "Baş Mühendis" },
            new() { FirstName = "Ayşe", LastName = "Çelik", Email = "ayse.celik@example.com", PhoneNumber = "+90 535 456 78 90", Role = "Seyir Subayı" },
            new() { FirstName = "Fatma", LastName = "Şahin", Email = "fatma.sahin@example.com", PhoneNumber = "+90 536 567 89 01", Role = "Kaptan" },
            new() { FirstName = "Mustafa", LastName = "Arslan", Email = "mustafa.arslan@example.com", PhoneNumber = "+90 537 678 90 12", Role = "Makine Subayı" },
            new() { FirstName = "Zeynep", LastName = "Koç", Email = "zeynep.koc@example.com", PhoneNumber = "+90 538 789 01 23", Role = "Baş Mühendis" },
            new() { FirstName = "Hasan", LastName = "Yıldız", Email = "hasan.yildiz@example.com", PhoneNumber = "+90 539 890 12 34", Role = "Güverte Subayı" },
            new() { FirstName = "Elif", LastName = "Öztürk", Email = "elif.ozturk@example.com", PhoneNumber = "+90 530 901 23 45", Role = "Seyir Subayı" },
            new() { FirstName = "İbrahim", LastName = "Çetin", Email = "ibrahim.cetin@example.com", PhoneNumber = "+90 531 012 34 56", Role = "Makine Subayı" },
        };
        await context.CrewMembers.AddRangeAsync(crew);

        await context.SaveChangesAsync();

        // Ziyaretler
        var visits = new List<ShipVisit>
        {
            new() { ShipId = ships[0].ShipId, PortId = ports[0].PortId, ArrivalDate = DateTime.Now.AddDays(-30), DepartureDate = DateTime.Now.AddDays(-27), Purpose = "Yükleme" },
            new() { ShipId = ships[1].ShipId, PortId = ports[1].PortId, ArrivalDate = DateTime.Now.AddDays(-25), DepartureDate = DateTime.Now.AddDays(-22), Purpose = "Boşaltma" },
            new() { ShipId = ships[2].ShipId, PortId = ports[2].PortId, ArrivalDate = DateTime.Now.AddDays(-20), DepartureDate = DateTime.Now.AddDays(-18), Purpose = "Bakım" },
            new() { ShipId = ships[3].ShipId, PortId = ports[3].PortId, ArrivalDate = DateTime.Now.AddDays(-15), DepartureDate = DateTime.Now.AddDays(-12), Purpose = "Yükleme" },
            new() { ShipId = ships[4].ShipId, PortId = ports[4].PortId, ArrivalDate = DateTime.Now.AddDays(-10), DepartureDate = DateTime.Now.AddDays(-7), Purpose = "Boşaltma" },
            new() { ShipId = ships[5].ShipId, PortId = ports[5].PortId, ArrivalDate = DateTime.Now.AddDays(-8), DepartureDate = DateTime.Now.AddDays(-5), Purpose = "Yakıt İkmali" },
            new() { ShipId = ships[6].ShipId, PortId = ports[6].PortId, ArrivalDate = DateTime.Now.AddDays(-5), DepartureDate = DateTime.Now.AddDays(-3), Purpose = "Yükleme" },
            new() { ShipId = ships[7].ShipId, PortId = ports[7].PortId, ArrivalDate = DateTime.Now.AddDays(-3), Purpose = "Boşaltma" },
            new() { ShipId = ships[0].ShipId, PortId = ports[4].PortId, ArrivalDate = DateTime.Now.AddDays(-60), DepartureDate = DateTime.Now.AddDays(-57), Purpose = "Bakım" },
            new() { ShipId = ships[1].ShipId, PortId = ports[5].PortId, ArrivalDate = DateTime.Now.AddDays(-45), DepartureDate = DateTime.Now.AddDays(-42), Purpose = "Yükleme" },
            new() { ShipId = ships[2].ShipId, PortId = ports[0].PortId, ArrivalDate = DateTime.Now.AddDays(-40), DepartureDate = DateTime.Now.AddDays(-37), Purpose = "Boşaltma" },
            new() { ShipId = ships[3].ShipId, PortId = ports[1].PortId, ArrivalDate = DateTime.Now.AddDays(-35), DepartureDate = DateTime.Now.AddDays(-32), Purpose = "Yakıt İkmali" },
        };
        await context.ShipVisits.AddRangeAsync(visits);

        // Yükler
        var cargoes = new List<Cargo>
        {
            new() { ShipId = ships[0].ShipId, Description = "Elektronik Eşya", WeightTon = 15000, CargoType = "Genel" },
            new() { ShipId = ships[1].ShipId, Description = "Ham Petrol", WeightTon = 80000, CargoType = "Tehlikeli" },
            new() { ShipId = ships[2].ShipId, Description = "Gıda Maddeleri", WeightTon = 5000, CargoType = "Gıda" },
            new() { ShipId = ships[3].ShipId, Description = "Kimyasal Madde", WeightTon = 12000, CargoType = "Tehlikeli" },
            new() { ShipId = ships[4].ShipId, Description = "Tekstil Ürünleri", WeightTon = 8000, CargoType = "Genel" },
            new() { ShipId = ships[5].ShipId, Description = "Demir Cevheri", WeightTon = 200000, CargoType = "Kuru Dökme" },
            new() { ShipId = ships[6].ShipId, Description = "Ham Petrol", WeightTon = 500000, CargoType = "Tehlikeli" },
            new() { ShipId = ships[7].ShipId, Description = "Otomobil", WeightTon = 25000, CargoType = "Genel" },
            new() { ShipId = ships[0].ShipId, Description = "Makine Ekipmanı", WeightTon = 3000, CargoType = "Genel" },
            new() { ShipId = ships[1].ShipId, Description = "Plastik Granül", WeightTon = 9000, CargoType = "Genel" },
            new() { ShipId = ships[2].ShipId, Description = "Tahıl", WeightTon = 45000, CargoType = "Gıda" },
            new() { ShipId = ships[3].ShipId, Description = "LPG", WeightTon = 30000, CargoType = "Tehlikeli" },
        };
        await context.Cargoes.AddRangeAsync(cargoes);

        // Atamalar
        var assignments = new List<ShipCrewAssignment>
        {
            new() { ShipId = ships[0].ShipId, CrewId = crew[0].CrewId, AssignmentDate = DateTime.Now.AddDays(-90) },
            new() { ShipId = ships[1].ShipId, CrewId = crew[1].CrewId, AssignmentDate = DateTime.Now.AddDays(-80) },
            new() { ShipId = ships[2].ShipId, CrewId = crew[2].CrewId, AssignmentDate = DateTime.Now.AddDays(-70) },
            new() { ShipId = ships[3].ShipId, CrewId = crew[3].CrewId, AssignmentDate = DateTime.Now.AddDays(-60) },
            new() { ShipId = ships[4].ShipId, CrewId = crew[4].CrewId, AssignmentDate = DateTime.Now.AddDays(-50) },
            new() { ShipId = ships[5].ShipId, CrewId = crew[5].CrewId, AssignmentDate = DateTime.Now.AddDays(-40) },
            new() { ShipId = ships[6].ShipId, CrewId = crew[6].CrewId, AssignmentDate = DateTime.Now.AddDays(-30) },
            new() { ShipId = ships[7].ShipId, CrewId = crew[7].CrewId, AssignmentDate = DateTime.Now.AddDays(-20) },
            new() { ShipId = ships[0].ShipId, CrewId = crew[8].CrewId, AssignmentDate = DateTime.Now.AddDays(-15) },
            new() { ShipId = ships[1].ShipId, CrewId = crew[9].CrewId, AssignmentDate = DateTime.Now.AddDays(-10) },
        };
        await context.ShipCrewAssignments.AddRangeAsync(assignments);

        await context.SaveChangesAsync();
    }
}