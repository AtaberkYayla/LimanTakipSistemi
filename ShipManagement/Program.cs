using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.Repositories;
using ShipManagement.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using ShipManagement.Validators;
using ShipManagement.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IShipService, ShipService>();
builder.Services.AddScoped<IPortService, PortService>();
builder.Services.AddScoped<IShipVisitService, ShipVisitService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<ICrewMemberService, CrewMemberService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateShipValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();