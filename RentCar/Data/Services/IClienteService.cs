using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    // Interfaz que define el contrato para manejar Clientes usando DTOs
    public interface IClienteService
    {
        // Listar todos los clientes (activos e inactivos)
        Task<List<ClienteDto>> GetAllAsync();
        //Obtener todos los clientes inactivos
        Task<List<ClienteDto>> GetAllInactivesAsync();
        
        // Listar solo clientes activos
        Task<List<ClienteDto>> GetAll();

        // Obtener un cliente por Id
        Task<ClienteDto?> GetByIdAsync(int id);

        // Crear un nuevo cliente (recibe DTO, devuelve DTO)
        Task<ClienteDto> CreateAsync(ClienteDto clienteDto);

        // Actualizar un cliente existente (recibe DTO)
        Task<bool> UpdateAsync(ClienteDto clienteDto);

        // Desactivar (eliminación lógica)
        Task<bool> DeactivateAsync(int id);
        //Eliminacion Fisica total
        Task<bool> DeleteAsync(int id);

        // Reactivar
        Task<bool> ActivateAsync(int id);
    }
}
