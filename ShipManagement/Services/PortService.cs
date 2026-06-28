using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class PortService : IPortService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public PortService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<PortDto>>> GetAllAsync()
    {
        var ports = await _context.Ports.ToListAsync();
        return Result<IEnumerable<PortDto>>.Success(ports.Select(ToDto));
    }

    public async Task<Result<PortDto>> GetByIdAsync(int id)
    {
        var port = await _uow.Repository<Port>().GetByIdAsync(id);
        if (port is null) return Result<PortDto>.Fail("Liman bulunamadı.");
        return Result<PortDto>.Success(ToDto(port));
    }

    public async Task<Result<PortDto>> AddAsync(CreatePortDto dto)
    {
        var port = new Port { Name = dto.Name, Country = dto.Country, City = dto.City };
        await _uow.Repository<Port>().AddAsync(port);
        await _uow.SaveChangesAsync();
        return Result<PortDto>.Success(ToDto(port));
    }

    public async Task<Result<PortDto>> UpdateAsync(int id, UpdatePortDto dto)
    {
        var port = await _uow.Repository<Port>().GetByIdAsync(id);
        if (port is null) return Result<PortDto>.Fail("Liman bulunamadı.");
        port.Name = dto.Name;
        port.Country = dto.Country;
        port.City = dto.City;
        _uow.Repository<Port>().Update(port);
        await _uow.SaveChangesAsync();
        return Result<PortDto>.Success(ToDto(port));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var port = await _uow.Repository<Port>().GetByIdAsync(id);
        if (port is null) return Result<bool>.Fail("Liman bulunamadı.");
        _uow.Repository<Port>().Delete(port);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static PortDto ToDto(Port p) => new()
    {
        PortId = p.PortId,
        Name = p.Name,
        Country = p.Country,
        City = p.City
    };
}