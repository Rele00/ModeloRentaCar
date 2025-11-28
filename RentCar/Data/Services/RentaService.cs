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
    public class RentaService : IRentaService
    {
        private readonly ApplicationDbContext _context;

        public RentaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------------
        // 1. CREAR RENTA
        // -------------------------------
        public async Task<(bool ok, string? error)> CrearRentaAsync(RentaCreateDto dto)
        {
            // 1. Validar vehículo
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == dto.VehiculoId);

            if (vehiculo == null)
            {
                return (false, "El vehículo seleccionado no existe.");
            }

            // Ajusta estos nombres según tu modelo
            if (!vehiculo.EsActivo || vehiculo.Estado != "Disponible")
            {
                return (false, "El vehículo no está disponible para renta.");
            }

            // 2. Validar cliente
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

            if (cliente == null || !cliente.EsActivo)
            {
                return (false, "El cliente seleccionado no es válido.");
            }

            // 3. Validar fechas
            if (dto.FechaEstimadaDevolucion <= dto.FechaSalida)
            {
                return (false, "La fecha estimada de devolución debe ser mayor que la fecha de salida.");
            }

            // 4. Calcular días
            var dias = (dto.FechaEstimadaDevolucion.Date - dto.FechaSalida.Date).Days;
            if (dias <= 0)
            {
                dias = 1;
            }
            dto.CantidadDias = dias;

            // 5. Precio diario: si no viene, tomamos del vehículo
            if (dto.PrecioDiario <= 0)
            {
                dto.PrecioDiario = vehiculo.PrecioPorDia; // AJUSTA al nombre de tu propiedad
            }

            // 6. Monto estimado
            dto.MontoEstimado = dto.PrecioDiario * dto.CantidadDias;

            // 7. Crear entidad Renta
            var renta = new Renta
            {
                VehiculoId = dto.VehiculoId,
                ClienteId = dto.ClienteId,
                FechaSalida = dto.FechaSalida,
                FechaEstimadaDevolucion = dto.FechaEstimadaDevolucion,
                PrecioDiario = dto.PrecioDiario,
                CantidadDias = dto.CantidadDias,
                MontoEstimado = dto.MontoEstimado,
                EstadoRenta = "Activa",
                Notas = dto.Notas,
                Activo = true
            };

            _context.Rentas.Add(renta);

            // 8. Cambiar estado del vehículo
            vehiculo.Estado = "Rentado"; // AJUSTA según tus estados
            _context.Vehiculos.Update(vehiculo);

            // 9. Registrar movimiento inicial
            var movimiento = new Movimiento
            {
                Renta = renta,
                TipoMovimiento = "RentaCreada",
                FechaMovimiento = DateTime.Now,
                Monto = dto.MontoEstimado,
                Descripcion = "Renta creada desde el sistema."
            };
            _context.Movimientos.Add(movimiento);

            // 10. Crear factura (ejemplo simple con ITBIS 18%)
            var subtotal = dto.MontoEstimado;
            var impuestos = subtotal * 0.18m; // ITBIS 18%
            var total = subtotal + impuestos;

            var factura = new Factura
            {
                Renta = renta,
                NumeroFactura = GenerarNumeroFactura(),
                Fecha = DateTime.Now,
                Subtotal = subtotal,
                Impuestos = impuestos,
                Descuento = 0m,
                Total = total,
                MetodoPago = dto.MetodoPago,
                Notas = dto.Notas
            };
            _context.Facturas.Add(factura);

            // 11. Guardar todo
            try
            {
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar la renta: {ex.Message}");
            }
        }

        // -------------------------------
        // 2. LISTAR RENTAS ACTIVAS
        // -------------------------------
        public async Task<IEnumerable<RentaResumenDto>> GetRentasActivasAsync()
        {
            return await _context.Rentas
                .Include(r => r.Cliente)
                .Include(r => r.Vehiculo)
                .Where(r => r.EstadoRenta == "Activa" && r.Activo)
                .OrderByDescending(r => r.FechaSalida)
                .Select(r => new RentaResumenDto
                {
                    Id = r.Id,
                    NombreCliente = r.Cliente != null ? r.Cliente.Nombre : "N/D",
                    VehiculoDescripcion = r.Vehiculo != null
                        ? (r.Vehiculo.Marca + " " + r.Vehiculo.Modelo + " - " + r.Vehiculo.Placa)
                        : "N/D",
                    FechaSalida = r.FechaSalida,
                    FechaEstimadaDevolucion = r.FechaEstimadaDevolucion,
                    MontoEstimado = r.MontoEstimado,
                    EstadoRenta = r.EstadoRenta
                })
                .ToListAsync();
        }

        // -------------------------------
        // 3. OBTENER RENTA PARA DEVOLUCIÓN
        // -------------------------------
        public async Task<RentaDevolucionDto?> GetRentaParaDevolucionAsync(int rentaId)
        {
            var renta = await _context.Rentas
                .Include(r => r.Cliente)
                .Include(r => r.Vehiculo)
                .Include(r => r.Factura)
                .FirstOrDefaultAsync(r => r.Id == rentaId);

            if (renta == null)
            {
                return null;
            }

            if (renta.EstadoRenta != "Activa" || !renta.Activo)
            {
                // No se puede devolver algo que ya está cerrado/cancelado
                return null;
            }

            var dto = new RentaDevolucionDto
            {
                RentaId = renta.Id,
                FechaDevolucionReal = DateTime.Today,

                ClienteId = renta.ClienteId,
                NombreCliente = renta.Cliente?.Nombre ?? "N/D",

                VehiculoId = renta.VehiculoId,
                DescripcionVehiculo = renta.Vehiculo != null
                    ? (renta.Vehiculo.Marca + " " + renta.Vehiculo.Modelo + " - " + renta.Vehiculo.Placa)
                    : "N/D",

                FechaSalida = renta.FechaSalida,
                FechaEstimadaDevolucion = renta.FechaEstimadaDevolucion,
                PrecioDiario = renta.PrecioDiario,
                CantidadDiasEstimada = renta.CantidadDias,
                MontoEstimado = renta.MontoEstimado,

                TotalFacturaActual = renta.Factura?.Total
            };

            return dto;
        }

        // -------------------------------
        // 4. DEVOLVER RENTA
        // -------------------------------
        public async Task<(bool ok, string? error)> DevolverRentaAsync(RentaDevolucionDto dto)
        {
            var renta = await _context.Rentas
                .Include(r => r.Vehiculo)
                .Include(r => r.Factura)
                .FirstOrDefaultAsync(r => r.Id == dto.RentaId);

            if (renta == null)
            {
                return (false, "La renta no existe.");
            }

            if (renta.EstadoRenta != "Activa" || !renta.Activo)
            {
                return (false, "La renta no está activa o ya fue cerrada.");
            }

            if (dto.FechaDevolucionReal.Date < renta.FechaSalida.Date)
            {
                return (false, "La fecha de devolución no puede ser menor a la fecha de salida.");
            }

            // Calcular días reales
            var diasReales = (dto.FechaDevolucionReal.Date - renta.FechaSalida.Date).Days;
            if (diasReales <= 0)
            {
                diasReales = 1;
            }

            // Subtotal base
            var subtotalBase = renta.PrecioDiario * diasReales;

            // Aplicar recargos y descuentos
            var subtotal = subtotalBase + dto.Recargo - dto.Descuento;
            if (subtotal < 0)
            {
                subtotal = 0;
            }

            // Impuestos (ej. ITBIS 18%)
            var impuestos = subtotal * 0.18m;
            var total = subtotal + impuestos;

            // Actualizar Renta
            renta.FechaDevolucionReal = dto.FechaDevolucionReal;
            renta.CantidadDias = diasReales;
            renta.MontoFinal = total;
            renta.EstadoRenta = "Cerrada";
            renta.Activo = false;
            renta.Notas = dto.Notas ?? renta.Notas;

            // Actualizar vehículo a disponible
            if (renta.Vehiculo != null)
            {
                renta.Vehiculo.Estado = "Disponible";
                _context.Vehiculos.Update(renta.Vehiculo);
            }

            // Actualizar o crear factura
            if (renta.Factura == null)
            {
                renta.Factura = new Factura
                {
                    RentaId = renta.Id,
                    NumeroFactura = GenerarNumeroFactura(),
                    Fecha = DateTime.Now
                };
                _context.Facturas.Add(renta.Factura);
            }

            renta.Factura.Subtotal = subtotal;
            renta.Factura.Impuestos = impuestos;
            renta.Factura.Descuento = dto.Descuento;
            renta.Factura.Total = total;
            renta.Factura.Notas = dto.Notas ?? renta.Factura.Notas;
            renta.Factura.Fecha = DateTime.Now;

            // Registrar movimiento de devolución
            var movimiento = new Movimiento
            {
                RentaId = renta.Id,
                TipoMovimiento = "Devolucion",
                FechaMovimiento = DateTime.Now,
                Monto = total,
                Descripcion = $"Devolución del vehículo. Días reales: {diasReales}. Total: {total:C2}"
            };
            _context.Movimientos.Add(movimiento);

            try
            {
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar la devolución: {ex.Message}");
            }
        }

        // -------------------------------
        // Helper para número de factura
        // -------------------------------
        private string GenerarNumeroFactura()
        {
            return $"F-{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}
