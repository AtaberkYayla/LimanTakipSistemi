using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class ShipService : IShipService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public ShipService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<ShipDto>>> GetAllAsync()
    {
        var ships = await _context.Ships.ToListAsync();
        var dto = ships.Select(s => ToDto(s));
        return Result<IEnumerable<ShipDto>>.Success(dto);
    }

    public async Task<Result<ShipDto>> GetByIdAsync(int id)
    {
        var ship = await _uow.Repository<Ship>().GetByIdAsync(id);
        if (ship is null)
            return Result<ShipDto>.Fail("Gemi bulunamadı.");
        return Result<ShipDto>.Success(ToDto(ship));
    }

    public async Task<Result<ShipDto>> AddAsync(CreateShipDto dto)
    {
        var imoExists = await _context.Ships.AnyAsync(s => s.IMO == dto.IMO);
        if (imoExists)
            return Result<ShipDto>.Fail("Bu IMO numarası zaten kayıtlı.");

        var ship = new Ship
        {
            Name = dto.Name,
            IMO = dto.IMO,
            Type = dto.Type,
            Flag = dto.Flag,
            YearBuilt = dto.YearBuilt
        };

        await _uow.Repository<Ship>().AddAsync(ship);
        await _uow.SaveChangesAsync();
        return Result<ShipDto>.Success(ToDto(ship));
    }

    public async Task<Result<ShipDto>> UpdateAsync(int id, UpdateShipDto dto)
    {
        var ship = await _uow.Repository<Ship>().GetByIdAsync(id);
        if (ship is null)
            return Result<ShipDto>.Fail("Gemi bulunamadı.");

        var imoExists = await _context.Ships.AnyAsync(s => s.IMO == dto.IMO && s.ShipId != id);
        if (imoExists)
            return Result<ShipDto>.Fail("Bu IMO numarası başka bir gemiye ait.");

        ship.Name = dto.Name;
        ship.IMO = dto.IMO;
        ship.Type = dto.Type;
        ship.Flag = dto.Flag;
        ship.YearBuilt = dto.YearBuilt;

        _uow.Repository<Ship>().Update(ship);
        await _uow.SaveChangesAsync();
        return Result<ShipDto>.Success(ToDto(ship));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var ship = await _uow.Repository<Ship>().GetByIdAsync(id);
        if (ship is null)
            return Result<bool>.Fail("Gemi bulunamadı.");

        _uow.Repository<Ship>().Delete(ship);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static ShipDto ToDto(Ship s) => new()
    {
        ShipId = s.ShipId,
        Name = s.Name,
        IMO = s.IMO,
        Type = s.Type,
        Flag = s.Flag,
        YearBuilt = s.YearBuilt
    };

    public async Task<Result<PaginatedResult<ShipDto>>> GetFilteredAsync(ShipFilterDto filter)
    {
        var query = _context.Ships.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(s => s.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Type))
            query = query.Where(s => s.Type.Contains(filter.Type));

        if (!string.IsNullOrWhiteSpace(filter.Flag))
            query = query.Where(s => s.Flag.Contains(filter.Flag));

        if (filter.YearBuiltMin.HasValue)
            query = query.Where(s => s.YearBuilt >= filter.YearBuiltMin.Value);

        if (filter.YearBuiltMax.HasValue)
            query = query.Where(s => s.YearBuilt <= filter.YearBuiltMax.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var result = new PaginatedResult<ShipDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        return Result<PaginatedResult<ShipDto>>.Success(result);
    }
}