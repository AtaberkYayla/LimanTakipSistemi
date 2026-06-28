using ShipManagement.Common;
using ShipManagement.DTOs;

namespace ShipManagement.Services;

public interface ICargoService
{
    Task<Result<IEnumerable<CargoDto>>> GetAllAsync();
    Task<Result<CargoDto>> GetByIdAsync(int id);
    Task<Result<CargoDto>> AddAsync(CreateCargoDto dto);
    Task<Result<CargoDto>> UpdateAsync(int id, UpdateCargoDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}