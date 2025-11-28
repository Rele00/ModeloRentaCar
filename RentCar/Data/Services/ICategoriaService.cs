using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(int id);
        Task<string> GetNombreCategoriaAsync(int id);

        Task<CategoriaDto> CreateAsync(CategoriaDto categoriaDto);
        Task<bool> UpdateAsync(CategoriaDto categoriaDto);
        Task<bool> DeactivateAsync(int id);
        Task<bool> ActivateAsync(int id);
    }
}
