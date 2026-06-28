using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface IShipCrewAssignmentService
{
    Task<Result<IEnumerable<ShipCrewAssignmentDto>>> GetAllAsync();
    Task<Result<ShipCrewAssignmentDto>> GetByIdAsync(int id);
    Task<Result<ShipCrewAssignmentDto>> AddAsync(CreateShipCrewAssignmentDto dto);
    Task<Result<ShipCrewAssignmentDto>> UpdateAsync(int id, UpdateShipCrewAssignmentDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}