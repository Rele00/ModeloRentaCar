using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    public interface IMasUsadosService
    {
        Task<List<MasUsadosDto>> ObtenerMasUsadosAsync(int top = 5, bool soloRentas = false);
    }
}