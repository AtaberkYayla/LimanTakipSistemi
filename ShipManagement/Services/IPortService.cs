using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface IPortService
{
    Task<Result<IEnumerable<PortDto>>> GetAllAsync();
    Task<Result<PortDto>> GetByIdAsync(int id);
    Task<Result<PortDto>> AddAsync(CreatePortDto dto);
    Task<Result<PortDto>> UpdateAsync(int id, UpdatePortDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}