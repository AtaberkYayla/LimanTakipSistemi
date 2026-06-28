using Microsoft.EntityFrameworkCore;
using ShipManagement.Models;

namespace ShipManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ship> Ships => Set<Ship>();
    public DbSet<Port> Ports => Set<Port>();
    public DbSet<ShipVisit> ShipVisits => Set<ShipVisit>();
    public DbSet<Cargo> Cargoes => Set<Cargo>();
    public DbSet<CrewMember> CrewMembers => Set<CrewMember>();
    public DbSet<ShipCrewAssignment> ShipCrewAssignments => Set<ShipCrewAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CrewMember>()
            .HasKey(c => c.CrewId);

        modelBuilder.Entity<Ship>()
            .HasKey(s => s.ShipId);

        modelBuilder.Entity<Port>()
            .HasKey(p => p.PortId);

        modelBuilder.Entity<ShipVisit>()
            .HasKey(v => v.VisitId);

        modelBuilder.Entity<Cargo>()
            .HasKey(c => c.CargoId);

        modelBuilder.Entity<ShipCrewAssignment>()
            .HasKey(a => a.AssignmentId);

        modelBuilder.Entity<Ship>()
            .HasIndex(s => s.IMO)
            .IsUnique();

        modelBuilder.Entity<ShipCrewAssignment>()
            .HasIndex(a => new { a.ShipId, a.CrewId, a.AssignmentDate })
            .IsUnique();

        modelBuilder.Entity<Cargo>()
            .Property(c => c.WeightTon)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ShipCrewAssignment>()
            .HasOne(a => a.CrewMember)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CrewId);

        modelBuilder.Entity<ShipCrewAssignment>()
            .HasOne(a => a.Ship)
            .WithMany(s => s.CrewAssignments)
            .HasForeignKey(a => a.ShipId);
    }
}