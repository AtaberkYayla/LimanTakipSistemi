using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface IShipService
{
    Task<Result<IEnumerable<ShipDto>>> GetAllAsync();
    Task<Result<ShipDto>> GetByIdAsync(int id);
    Task<Result<ShipDto>> AddAsync(CreateShipDto dto);
    Task<Result<ShipDto>> UpdateAsync(int id, UpdateShipDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<PaginatedResult<ShipDto>>> GetFilteredAsync(ShipFilterDto filter);
}