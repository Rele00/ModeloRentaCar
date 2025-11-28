namespace RentCar.Data.Services
{
    // Interfaz que define el contrato para manejar Vehículos (trabaja con DTOs para la UI)
    public interface IVehiculoService
    {
        Task<List<VehiculoDto>> GetAllAsync();
        Task<VehiculoDto?> GetByIdAsync(int id);

        // Crear un nuevo vehículo a partir de un DTO
        Task<VehiculoDto> CreateAsync(VehiculoDto vehiculoDto);

        // Actualizar un vehículo existente (id + DTO)
        Task<bool> UpdateAsync(int id, VehiculoDto vehiculoDto);

        // Desactivar (baja lógica)
        Task<bool> DeactivateAsync(int id);

        // Reactivar
        Task<bool> ActivateAsync(int id);

        // Eliminar físico
        Task<bool> EliminateAsync(int id);
    }
}
