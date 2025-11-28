using System.Collections.Generic;
using System.Threading.Tasks;
using RentCar.Data.Dtos;

namespace RentCar.Data.Services
{
    /// <summary>
    /// Servicio de dominio para gestionar el ciclo de vida de una renta:
    /// creación, consulta y devolución.
    /// </summary>
    public interface IRentaService
    {
        /// <summary>
        /// Crea una nueva renta, cambia el estado del vehículo a "Rentado",
        /// registra el movimiento inicial y genera la factura.
        /// </summary>
        /// <param name="dto">Datos de entrada para crear la renta.</param>
        /// <returns>
        /// ok = true si todo salió bien, error = mensaje en caso de fallo.
        /// </returns>
        Task<(bool ok, string? error)> CrearRentaAsync(RentaCreateDto dto);

        /// <summary>
        /// Devuelve un listado de las rentas activas (EstadoRenta = "Activa").
        /// </summary>
        Task<IEnumerable<RentaResumenDto>> GetRentasActivasAsync();

        /// <summary>
        /// Devuelve los datos necesarios para mostrar el formulario
        /// de devolución de una renta específica.
        /// </summary>
        /// <param name="rentaId">Id de la renta.</param>
        /// <returns>
        /// DTO con la información de la renta lista para devolver,
        /// o null si no existe o no está activa.
        /// </returns>
        Task<RentaDevolucionDto?> GetRentaParaDevolucionAsync(int rentaId);

        /// <summary>
        /// Registra la devolución de la renta:
        /// - Calcula días reales.
        /// - Recalcula importe, impuestos, recargos y descuentos.
        /// - Cierra la renta.
        /// - Actualiza la factura.
        /// - Cambia el vehículo a "Disponible".
        /// - Inserta un Movimiento tipo "Devolucion".
        /// </summary>
        /// <param name="dto">Datos de devolución proporcionados por el usuario.</param>
        /// <returns>
        /// ok = true si la devolución se registró correctamente, error = mensaje en caso contrario.
        /// </returns>
        Task<(bool ok, string? error)> DevolverRentaAsync(RentaDevolucionDto dto);
    }
}
