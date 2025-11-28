using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    public interface IMovimientoService
    {
        /// <summary>
        /// Devuelve todos los movimientos, opcionalmente filtrados por fecha y/o tipo.
        /// Si se llama sin parámetros, devuelve todos los movimientos.
        /// </summary>
        Task<IEnumerable<MovimientoDto>> GetMovimientosAsync(
            DateTime? desde = null,
            DateTime? hasta = null,
            string? tipoMovimiento = null);

        /// <summary>
        /// Devuelve todos los movimientos (sin filtro).
        /// Método de conveniencia.
        /// </summary>
        Task<IEnumerable<MovimientoDto>> GetTodosAsync();

        /// <summary>
        /// Devuelve todos los movimientos de una renta específica.
        /// </summary>
        Task<IEnumerable<MovimientoDto>> GetMovimientosPorRentaAsync(int rentaId);

        /// <summary>
        /// Devuelve un movimiento por Id (o null si no existe).
        /// </summary>
        Task<MovimientoDto?> GetByIdAsync(int id);
    }
}
