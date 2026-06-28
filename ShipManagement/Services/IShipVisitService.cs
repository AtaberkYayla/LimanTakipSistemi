using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface IShipVisitService
{
    Task<Result<IEnumerable<ShipVisitDto>>> GetAllAsync();
    Task<Result<ShipVisitDto>> GetByIdAsync(int id);
    Task<Result<ShipVisitDto>> AddAsync(CreateShipVisitDto dto);
    Task<Result<ShipVisitDto>> UpdateAsync(int id, UpdateShipVisitDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}