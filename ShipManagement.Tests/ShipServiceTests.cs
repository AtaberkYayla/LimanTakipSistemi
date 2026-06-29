using Microsoft.EntityFrameworkCore;
using Moq;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;
using ShipManagement.Services;

namespace ShipManagement.Tests;

public class ShipServiceTests
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
        var service = new ShipService(uow, context);

        var dto = new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "1234567",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Ship", result.Value.Name);
        Assert.Equal("1234567", result.Value.IMO);
    }

    [Fact]
    public async Task AddAsync_WithDuplicateIMO_ReturnsFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new ShipService(uow, context);

        var dto = new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "1234567",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        };

        // Act
        await service.AddAsync(dto); // İlk ekleme
        var result = await service.AddAsync(dto); // Aynı IMO ile tekrar

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Bu IMO numarası zaten kayıtlı.", result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsShip()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new ShipService(uow, context);

        var dto = new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "1234567",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        };

        var added = await service.AddAsync(dto);

        // Act
        var result = await service.GetByIdAsync(added.Value!.ShipId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Ship", result.Value!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new ShipService(uow, context);

        // Act
        var result = await service.GetByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Gemi bulunamadı.", result.Error);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var uow = new UnitOfWork(context);
        var service = new ShipService(uow, context);

        var added = await service.AddAsync(new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "1234567",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        });

        // Act
        var result = await service.DeleteAsync(added.Value!.ShipId);

        // Assert
        Assert.True(result.IsSuccess);
    }
}