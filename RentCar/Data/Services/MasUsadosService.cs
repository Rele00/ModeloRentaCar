using Microsoft.EntityFrameworkCore;
using RentCar.Data.Context;
using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    public class MasUsadosService : IMasUsadosService
    {
        private readonly ApplicationDbContext _context;

        public MasUsadosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MasUsadosDto>> ObtenerMasUsadosAsync(int top = 5, bool soloRentas = false)
        {
            // Query equivalente al SQL proporcionado: join Movimientos -> Vehiculos, agrupar y contar
            var query =
                from v in _context.Vehiculos
                join m in _context.Movimientos on v.Id equals m.Id
                where !soloRentas || m.TipoMovimiento == "Renta"
                group m by new { v.Id, v.Marca, v.Modelo, v.Placa, v.ImageUrl, v.Color, v.Año, v.Estado } into g
                select new MasUsadosDto
                {
                    Id = g.Key.Id,
                    Marca = g.Key.Marca,
                    Modelo = g.Key.Modelo,
                    Placa = g.Key.Placa,
                    ImageUrl = g.Key.ImageUrl,
                    Color = g.Key.Color,
                    Año = g.Key.Año,
                    Estado = g.Key.Estado,
                    VecesUsado = g.Count()
                };

            var result = await query
                .OrderByDescending(x => x.VecesUsado)
                .ThenBy(x => x.Marca)
                .ThenBy(x => x.Modelo)
                .Take(top)
                .ToListAsync();

            return result;
        }


    }
}