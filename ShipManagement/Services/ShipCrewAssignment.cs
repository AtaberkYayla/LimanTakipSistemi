using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class ShipCrewAssignmentService : IShipCrewAssignmentService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public ShipCrewAssignmentService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<ShipCrewAssignmentDto>>> GetAllAsync()
    {
        var assignments = await _context.ShipCrewAssignments
            .Include(a => a.Ship)
            .Include(a => a.CrewMember)
            .ToListAsync();
        return Result<IEnumerable<ShipCrewAssignmentDto>>.Success(assignments.Select(ToDto));
    }

    public async Task<Result<ShipCrewAssignmentDto>> GetByIdAsync(int id)
    {
        var assignment = await _context.ShipCrewAssignments
            .Include(a => a.Ship)
            .Include(a => a.CrewMember)
            .FirstOrDefaultAsync(a => a.AssignmentId == id);
        if (assignment is null) return Result<ShipCrewAssignmentDto>.Fail("Atama bulunamadı.");
        return Result<ShipCrewAssignmentDto>.Success(ToDto(assignment));
    }

    public async Task<Result<ShipCrewAssignmentDto>> AddAsync(CreateShipCrewAssignmentDto dto)
    {
        var ship = await _uow.Repository<Ship>().GetByIdAsync(dto.ShipId);
        if (ship is null) return Result<ShipCrewAssignmentDto>.Fail("Gemi bulunamadı.");

        var crew = await _uow.Repository<CrewMember>().GetByIdAsync(dto.CrewId);
        if (crew is null) return Result<ShipCrewAssignmentDto>.Fail("Mürettebat bulunamadı.");

        var duplicate = await _context.ShipCrewAssignments.AnyAsync(a =>
            a.ShipId == dto.ShipId &&
            a.CrewId == dto.CrewId &&
            a.AssignmentDate.Date == dto.AssignmentDate.Date);
        if (duplicate)
            return Result<ShipCrewAssignmentDto>.Fail("Bu mürettebat aynı gemiye aynı tarihte zaten atanmış.");

        var assignment = new ShipCrewAssignment
        {
            ShipId = dto.ShipId,
            CrewId = dto.CrewId,
            AssignmentDate = dto.AssignmentDate
        };

        await _uow.Repository<ShipCrewAssignment>().AddAsync(assignment);
        await _uow.SaveChangesAsync();

        assignment.Ship = ship;
        assignment.CrewMember = crew;

        return Result<ShipCrewAssignmentDto>.Success(ToDto(assignment));
    }

    public async Task<Result<ShipCrewAssignmentDto>> UpdateAsync(int id, UpdateShipCrewAssignmentDto dto)
    {
        var assignment = await _context.ShipCrewAssignments
            .Include(a => a.Ship)
            .Include(a => a.CrewMember)
            .FirstOrDefaultAsync(a => a.AssignmentId == id);
        if (assignment is null) return Result<ShipCrewAssignmentDto>.Fail("Atama bulunamadı.");

        var ship = await _uow.Repository<Ship>().GetByIdAsync(dto.ShipId);
        if (ship is null) return Result<ShipCrewAssignmentDto>.Fail("Gemi bulunamadı.");

        var crew = await _uow.Repository<CrewMember>().GetByIdAsync(dto.CrewId);
        if (crew is null) return Result<ShipCrewAssignmentDto>.Fail("Mürettebat bulunamadı.");

        var duplicate = await _context.ShipCrewAssignments.AnyAsync(a =>
            a.ShipId == dto.ShipId &&
            a.CrewId == dto.CrewId &&
            a.AssignmentDate.Date == dto.AssignmentDate.Date &&
            a.AssignmentId != id);
        if (duplicate)
            return Result<ShipCrewAssignmentDto>.Fail("Bu mürettebat aynı gemiye aynı tarihte zaten atanmış.");

        assignment.ShipId = dto.ShipId;
        assignment.CrewId = dto.CrewId;
        assignment.AssignmentDate = dto.AssignmentDate;

        _uow.Repository<ShipCrewAssignment>().Update(assignment);
        await _uow.SaveChangesAsync();

        assignment.Ship = ship;
        assignment.CrewMember = crew;

        return Result<ShipCrewAssignmentDto>.Success(ToDto(assignment));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var assignment = await _uow.Repository<ShipCrewAssignment>().GetByIdAsync(id);
        if (assignment is null) return Result<bool>.Fail("Atama bulunamadı.");
        _uow.Repository<ShipCrewAssignment>().Delete(assignment);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static ShipCrewAssignmentDto ToDto(ShipCrewAssignment a) => new()
    {
        AssignmentId = a.AssignmentId,
        ShipId = a.ShipId,
        ShipName = a.Ship?.Name ?? string.Empty,
        CrewId = a.CrewId,
        CrewFullName = a.CrewMember is not null
            ? $"{a.CrewMember.FirstName} {a.CrewMember.LastName}"
            : string.Empty,
        AssignmentDate = a.AssignmentDate
    };
}