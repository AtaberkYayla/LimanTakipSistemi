using Microsoft.EntityFrameworkCore;
using ShipManagement.Common;
using ShipManagement.Data;
using ShipManagement.DTOs;
using ShipManagement.Models;
using ShipManagement.Repositories;

namespace ShipManagement.Services;

public class CrewMemberService : ICrewMemberService
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public CrewMemberService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<Result<IEnumerable<CrewMemberDto>>> GetAllAsync()
    {
        var crew = await _context.CrewMembers.ToListAsync();
        return Result<IEnumerable<CrewMemberDto>>.Success(crew.Select(ToDto));
    }

    public async Task<Result<CrewMemberDto>> GetByIdAsync(int id)
    {
        var crew = await _uow.Repository<CrewMember>().GetByIdAsync(id);
        if (crew is null) return Result<CrewMemberDto>.Fail("Mürettebat bulunamadı.");
        return Result<CrewMemberDto>.Success(ToDto(crew));
    }

    public async Task<Result<CrewMemberDto>> AddAsync(CreateCrewMemberDto dto)
    {
        if (!IsValidEmail(dto.Email))
            return Result<CrewMemberDto>.Fail("Geçersiz e-posta formatı.");

        if (!IsValidPhone(dto.PhoneNumber))
            return Result<CrewMemberDto>.Fail("Geçersiz telefon formatı. Örnek: +90 5XX XXX XX XX");

        var crew = new CrewMember
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role
        };

        await _uow.Repository<CrewMember>().AddAsync(crew);
        await _uow.SaveChangesAsync();
        return Result<CrewMemberDto>.Success(ToDto(crew));
    }

    public async Task<Result<CrewMemberDto>> UpdateAsync(int id, UpdateCrewMemberDto dto)
    {
        if (!IsValidEmail(dto.Email))
            return Result<CrewMemberDto>.Fail("Geçersiz e-posta formatı.");

        if (!IsValidPhone(dto.PhoneNumber))
            return Result<CrewMemberDto>.Fail("Geçersiz telefon formatı. Örnek: +90 5XX XXX XX XX");

        var crew = await _uow.Repository<CrewMember>().GetByIdAsync(id);
        if (crew is null) return Result<CrewMemberDto>.Fail("Mürettebat bulunamadı.");

        crew.FirstName = dto.FirstName;
        crew.LastName = dto.LastName;
        crew.Email = dto.Email;
        crew.PhoneNumber = dto.PhoneNumber;
        crew.Role = dto.Role;

        _uow.Repository<CrewMember>().Update(crew);
        await _uow.SaveChangesAsync();
        return Result<CrewMemberDto>.Success(ToDto(crew));
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var crew = await _uow.Repository<CrewMember>().GetByIdAsync(id);
        if (crew is null) return Result<bool>.Fail("Mürettebat bulunamadı.");
        _uow.Repository<CrewMember>().Delete(crew);
        await _uow.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private static bool IsValidEmail(string email) =>
        System.Text.RegularExpressions.Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private static bool IsValidPhone(string phone) =>
        System.Text.RegularExpressions.Regex.IsMatch(phone,
            @"^\+90\s5\d{2}\s\d{3}\s\d{2}\s\d{2}$");

    private static CrewMemberDto ToDto(CrewMember c) => new()
    {
        CrewId = c.CrewId,
        FirstName = c.FirstName,
        LastName = c.LastName,
        Email = c.Email,
        PhoneNumber = c.PhoneNumber,
        Role = c.Role
    };
}