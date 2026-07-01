using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipManagement.Data;
using ShipManagement.DTOs;

namespace ShipManagement.IntegrationTests;

public class ShipsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ShipsApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var toRemove = services.Where(d =>
                    d.ServiceType.Namespace != null &&
                    d.ServiceType.Namespace.Contains("EntityFrameworkCore")).ToList();

                foreach (var descriptor in toRemove)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("SharedTestDb"));
            });
        });
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    [Fact]
    public async Task GetAll_Ships_ReturnsOk()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/v1/ships");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_Ship_ReturnsCreated()
    {
        var client = CreateClient();
        var ship = new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "1234567",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        };

        var response = await client.PostAsJsonAsync("/api/v1/ships", ship);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_Ship_WithDuplicateIMO_ReturnsBadRequest()
    {
        var client = CreateClient();
        var ship = new CreateShipDto
        {
            Name = "Test Ship",
            IMO = "9999991",
            Type = "Konteyner",
            Flag = "Turkey",
            YearBuilt = 2020
        };

        await client.PostAsJsonAsync("/api/v1/ships", ship);
        var response = await client.PostAsJsonAsync("/api/v1/ships", ship);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_Ship_ById_ReturnsOk()
    {
        var client = CreateClient();
        var ship = new CreateShipDto
        {
            Name = "Test Ship GetById",
            IMO = "7654321",
            Type = "Tanker",
            Flag = "Turkey",
            YearBuilt = 2015
        };

        var postResponse = await client.PostAsJsonAsync("/api/v1/ships", ship);
        var created = await postResponse.Content.ReadFromJsonAsync<ShipDto>();

        var response = await client.GetAsync($"/api/v1/ships/{created!.ShipId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Ship_ReturnsNoContent()
    {
        var client = CreateClient();
        var ship = new CreateShipDto
        {
            Name = "Test Ship Delete",
            IMO = "1112223",
            Type = "Kargo",
            Flag = "Turkey",
            YearBuilt = 2010
        };

        var postResponse = await client.PostAsJsonAsync("/api/v1/ships", ship);
        var created = await postResponse.Content.ReadFromJsonAsync<ShipDto>();

        var response = await client.DeleteAsync($"/api/v1/ships/{created!.ShipId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Get_NonExistent_Ship_ReturnsNotFound()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/v1/ships/99999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}