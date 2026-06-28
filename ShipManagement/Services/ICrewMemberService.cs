using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface ICrewMemberService
{
    Task<Result<IEnumerable<CrewMemberDto>>> GetAllAsync();
    Task<Result<CrewMemberDto>> GetByIdAsync(int id);
    Task<Result<CrewMemberDto>> AddAsync(CreateCrewMemberDto dto);
    Task<Result<CrewMemberDto>> UpdateAsync(int id, UpdateCrewMemberDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}