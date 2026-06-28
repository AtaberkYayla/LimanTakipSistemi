using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class ShipVisitService : IShipVisitService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public ShipVisitService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<ShipVisitDto>>> GetAllAsync()
    {
        var visits = await _context.ShipVisits
            .Include(v => v.Ship)
            .Include(v => v.Port)
            .ToListAsync();
        return Result<IEnumerable<ShipVisitDto>>.Success(visits.Select(ToDto));
    }

    public async Task<Result<ShipVisitDto>> GetByIdAsync(int id)
    {
        var visit = await _context.ShipVisits
            .Include(v => v.Ship)
            .Include(v => v.Port)
            .FirstOrDefaultAsync(v => v.VisitId == id);
        if (visit is null) return Result<ShipVisitDto>.Fail("Ziyaret bulunamadı.");
        return Result<ShipVisitDto>.Success(ToDto(visit));
    }

    public async Task<Result<ShipVisitDto>> AddAsync(CreateShipVisitDto dto)
    {
        if (dto.DepartureDate.HasValue && dto.DepartureDate < dto.ArrivalDate)
            return Result<ShipVisitDto>.Fail("Ayrılış tarihi geliş tarihinden önce olamaz.");

        var ship = await _uow.Repository<Ship>().GetByIdAsync(dto.ShipId);
        if (ship is null) return Result<ShipVisitDto>.Fail("Gemi bulunamadı.");

        var port = await _uow.Repository<Port>().GetByIdAsync(dto.PortId);
        if (port is null) return Result<ShipVisitDto>.Fail("Liman bulunamadı.");

        var visit = new ShipVisit
        {
            ShipId = dto.ShipId,
            PortId = dto.PortId,
            ArrivalDate = dto.ArrivalDate,
            DepartureDate = dto.DepartureDate,
            Purpose = dto.Purpose
        };

        await _uow.Repository<ShipVisit>().AddAsync(visit);
        await _uow.SaveChangesAsync();

        visit.Ship = ship;
        visit.Port = port;

        return Result<ShipVisitDto>.Success(ToDto(visit));
    }

    public async Task<Result<ShipVisitDto>> UpdateAsync(int id, UpdateShipVisitDto dto)
    {
        var visit = await _context.ShipVisits
            .Include(v => v.Ship)
            .Include(v => v.Port)
            .FirstOrDefaultAsync(v => v.VisitId == id);
        if (visit is null) return Result<ShipVisitDto>.Fail("Ziyaret bulunamadı.");

        if (dto.DepartureDate.HasValue && dto.DepartureDate < dto.ArrivalDate)
            return Result<ShipVisitDto>.Fail("Ayrılış tarihi geliş tarihinden önce olamaz.");

        visit.ShipId = dto.ShipId;
        visit.PortId = dto.PortId;
        visit.ArrivalDate = dto.ArrivalDate;
        visit.DepartureDate = dto.DepartureDate;
        visit.Purpose = dto.Purpose;

        _uow.Repository<ShipVisit>().Update(visit);
        await _uow.SaveChangesAsync();
        return Result<ShipVisitDto>.Success(ToDto(visit));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var visit = await _uow.Repository<ShipVisit>().GetByIdAsync(id);
        if (visit is null) return Result<bool>.Fail("Ziyaret bulunamadı.");
        _uow.Repository<ShipVisit>().Delete(visit);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static ShipVisitDto ToDto(ShipVisit v) => new()
    {
        VisitId = v.VisitId,
        ShipId = v.ShipId,
        ShipName = v.Ship?.Name ?? string.Empty,
        PortId = v.PortId,
        PortName = v.Port?.Name ?? string.Empty,
        ArrivalDate = v.ArrivalDate,
        DepartureDate = v.DepartureDate,
        Purpose = v.Purpose
    };
}