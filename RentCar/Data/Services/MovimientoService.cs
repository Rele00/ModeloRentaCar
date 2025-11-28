using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentCar.Data.Context;
using RentCar.Data.Dtos;
using RentCar.Data.Models;

namespace RentCar.Data.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly ApplicationDbContext _context;

        public MovimientoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los movimientos (devuelve DTOs)
        public async Task<IEnumerable<MovimientoDto>> GetTodosAsync()
        {
            // Reutilizamos la lógica de GetMovimientosAsync()
            return await GetMovimientosAsync();
        }

        // --------------------------------------------------------------------
        // 1. Obtener movimientos con filtros opcionales
        // --------------------------------------------------------------------
        public async Task<IEnumerable<MovimientoDto>> GetMovimientosAsync(
            DateTime? desde = null,
            DateTime? hasta = null,
            string? tipoMovimiento = null)
        {
            var query = _context.Movimientos
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Cliente)
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Vehiculo)
                .AsQueryable();

            if (desde.HasValue)
            {
                query = query.Where(m => m.FechaMovimiento.Date >= desde.Value.Date);
            }

            if (hasta.HasValue)
            {
                query = query.Where(m => m.FechaMovimiento.Date <= hasta.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(tipoMovimiento))
            {
                query = query.Where(m => m.TipoMovimiento == tipoMovimiento);
            }

            var lista = await query
                .OrderByDescending(m => m.FechaMovimiento)
                .ToListAsync();

            return lista.Select(MapearMovimiento);
        }

        // --------------------------------------------------------------------
        // 2. Movimientos por Renta
        // --------------------------------------------------------------------
        public async Task<IEnumerable<MovimientoDto>> GetMovimientosPorRentaAsync(int rentaId)
        {
            var lista = await _context.Movimientos
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Cliente)
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Vehiculo)
                .Where(m => m.RentaId == rentaId)
                .OrderBy(m => m.FechaMovimiento)
                .ToListAsync();

            return lista.Select(MapearMovimiento);
        }

        // --------------------------------------------------------------------
        // 3. Movimiento por Id
        // --------------------------------------------------------------------
        public async Task<MovimientoDto?> GetByIdAsync(int id)
        {
            var movimiento = await _context.Movimientos
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Cliente)
                .Include(m => m.Renta)
                    .ThenInclude(r => r.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimiento == null)
            {
                return null;
            }

            return MapearMovimiento(movimiento);
        }

        // --------------------------------------------------------------------
        // Método privado de mapeo entidad -> DTO
        // --------------------------------------------------------------------
        private static MovimientoDto MapearMovimiento(Movimiento m)
        {
            var clienteId = 0;
            var nombreCliente = "N/D";
            var vehiculoId = 0;
            var vehiculoDescripcion = "N/D";

            if (m.Renta != null)
            {
                if (m.Renta.Cliente != null)
                {
                    clienteId = m.Renta.ClienteId;
                    nombreCliente = m.Renta.Cliente.Nombre; // AJUSTA si el campo se llama distinto
                }

                if (m.Renta.Vehiculo != null)
                {
                    vehiculoId = m.Renta.VehiculoId;
                    vehiculoDescripcion = $"{m.Renta.Vehiculo.Marca} {m.Renta.Vehiculo.Modelo} - {m.Renta.Vehiculo.Placa}";
                    // Ajusta los nombres Marca/Modelo/Placa si tus propiedades se llaman diferente
                }
            }

            return new MovimientoDto
            {
                Id = m.Id,
                RentaId = m.RentaId,
                TipoMovimiento = m.TipoMovimiento,
                FechaMovimiento = m.FechaMovimiento,
                Monto = m.Monto,
                Descripcion = m.Descripcion,

                ClienteId = clienteId,
                NombreCliente = nombreCliente,
                VehiculoId = vehiculoId,
                VehiculoDescripcion = vehiculoDescripcion
            };
        }
    }
}
