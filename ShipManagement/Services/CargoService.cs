using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class CargoService : ICargoService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public CargoService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<CargoDto>>> GetAllAsync()
    {
        var cargoes = await _context.Cargoes
            .Include(c => c.Ship)
            .ToListAsync();
        return Result<IEnumerable<CargoDto>>.Success(cargoes.Select(ToDto));
    }

    public async Task<Result<CargoDto>> GetByIdAsync(int id)
    {
        var cargo = await _context.Cargoes
            .Include(c => c.Ship)
            .FirstOrDefaultAsync(c => c.CargoId == id);
        if (cargo is null) return Result<CargoDto>.Fail("Yük bulunamadı.");
        return Result<CargoDto>.Success(ToDto(cargo));
    }

    public async Task<Result<CargoDto>> AddAsync(CreateCargoDto dto)
    {
        if (dto.WeightTon <= 0)
            return Result<CargoDto>.Fail("Ağırlık 0'dan büyük olmalıdır.");

        var ship = await _uow.Repository<Ship>().GetByIdAsync(dto.ShipId);
        if (ship is null) return Result<CargoDto>.Fail("Gemi bulunamadı.");

        var cargo = new Cargo
        {
            ShipId = dto.ShipId,
            Description = dto.Description,
            WeightTon = dto.WeightTon,
            CargoType = dto.CargoType
        };

        await _uow.Repository<Cargo>().AddAsync(cargo);
        await _uow.SaveChangesAsync();

        cargo.Ship = ship;
        return Result<CargoDto>.Success(ToDto(cargo));
    }

    public async Task<Result<CargoDto>> UpdateAsync(int id, UpdateCargoDto dto)
    {
        if (dto.WeightTon <= 0)
            return Result<CargoDto>.Fail("Ağırlık 0'dan büyük olmalıdır.");

        var cargo = await _context.Cargoes
            .Include(c => c.Ship)
            .FirstOrDefaultAsync(c => c.CargoId == id);
        if (cargo is null) return Result<CargoDto>.Fail("Yük bulunamadı.");

        var ship = await _uow.Repository<Ship>().GetByIdAsync(dto.ShipId);
        if (ship is null) return Result<CargoDto>.Fail("Gemi bulunamadı.");

        cargo.ShipId = dto.ShipId;
        cargo.Description = dto.Description;
        cargo.WeightTon = dto.WeightTon;
        cargo.CargoType = dto.CargoType;

        _uow.Repository<Cargo>().Update(cargo);
        await _uow.SaveChangesAsync();

        cargo.Ship = ship;
        return Result<CargoDto>.Success(ToDto(cargo));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var cargo = await _uow.Repository<Cargo>().GetByIdAsync(id);
        if (cargo is null) return Result<bool>.Fail("Yük bulunamadı.");
        _uow.Repository<Cargo>().Delete(cargo);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static CargoDto ToDto(Cargo c) => new()
    {
        CargoId = c.CargoId,
        ShipId = c.ShipId,
        ShipName = c.Ship?.Name ?? string.Empty,
        Description = c.Description,
        WeightTon = c.WeightTon,
        CargoType = c.CargoType
    };
}