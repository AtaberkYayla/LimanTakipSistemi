using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Repositories;
using ShipManagement.Services;

namespace ShipManagement.Tests;

public class CrewMemberServiceTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new CrewMemberService(uow, context);

        var dto = new CreateCrewMemberDto
        {
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+90 532 123 45 67",
            Role = "Kaptan"
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Ahmet", result.Value!.FirstName);
        Assert.Equal("Kaptan", result.Value.Role);
    }

    [Fact]
    public async Task AddAsync_WithInvalidEmail_ReturnsFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new CrewMemberService(uow, context);

        var dto = new CreateCrewMemberDto
        {
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            Email = "gecersiz-email",
            PhoneNumber = "+90 532 123 45 67",
            Role = "Kaptan"
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Geçersiz e-posta formatı.", result.Error);
    }

    [Fact]
    public async Task AddAsync_WithInvalidPhone_ReturnsFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new CrewMemberService(uow, context);

        var dto = new CreateCrewMemberDto
        {
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "05321234567",
            Role = "Kaptan"
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Geçersiz telefon formatı. Örnek: +90 5XX XXX XX XX", result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsCrewMember()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new CrewMemberService(uow, context);

        var added = await service.AddAsync(new CreateCrewMemberDto
        {
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            Email = "ahmet@example.com",
            PhoneNumber = "+90 532 123 45 67",
            Role = "Kaptan"
        });

        // Act
        var result = await service.GetByIdAsync(added.Value!.CrewId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Ahmet", result.Value!.FirstName);
    }
}