using Microsoft.EntityFrameworkCore;
using RentCar.Data.Context;
using RentCar.Data.Models;

namespace RentCar.Data.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly ApplicationDbContext _context;

        public VehiculoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------ MAPEOS ------------------

        private static VehiculoDto ToDto(Vehiculo v) =>
            new VehiculoDto
            {
                Id = v.Id,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Año = v.Año,
                Placa = v.Placa,
                Color = v.Color,
                TipoVehiculo = v.TipoVehiculo,
                CategoriaId = v.CategoriaId,
                Estado = v.Estado,
                ImageUrl = v.ImageUrl,
                FechaRegistro = v.FechaRegistro,
                EsActivo = v.EsActivo,
                PrecioPorDia = v.PrecioPorDia
            };

        private static Vehiculo ToEntity(VehiculoDto dto) =>
            new Vehiculo
            {
                Id = dto.Id,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Año = dto.Año,
                Placa = dto.Placa,
                Color = dto.Color,
                TipoVehiculo = dto.TipoVehiculo,
                CategoriaId = dto.CategoriaId,
                Estado = dto.Estado,
                ImageUrl = dto.ImageUrl,
                EsActivo = dto.EsActivo,
                PrecioPorDia = dto.PrecioPorDia,
                // Si viene con fecha por defecto, asumimos ahora
                FechaRegistro = dto.FechaRegistro == default
                    ? DateTime.UtcNow
                    : dto.FechaRegistro
            };

        // ------------------ QUERIES ------------------

        // Obtener todos los vehículos activos (devuelve DTOs)
        public async Task<List<VehiculoDto>> GetAllAsync()
        {
            var vehiculos = await _context.Vehiculos
                                          .Where(v => v.EsActivo)
                                          .ToListAsync();

            return vehiculos.Select(ToDto).ToList();
        }

        // Obtener un vehículo por Id (devuelve DTO)
        public async Task<VehiculoDto?> GetByIdAsync(int id)
        {
            var v = await _context.Vehiculos
                                  .FirstOrDefaultAsync(x => x.Id == id);

            return v is null ? null : ToDto(v);
        }

        // ------------------ COMANDOS (CRUD) ------------------

        // Crear un nuevo vehículo (recibe DTO y devuelve DTO)
        public async Task<VehiculoDto> CreateAsync(VehiculoDto vehiculoDto)
        {
            var entity = ToEntity(vehiculoDto);

            _context.Vehiculos.Add(entity);
            await _context.SaveChangesAsync();

            // entity.Id se llena después del SaveChanges
            return ToDto(entity);
        }

        // Actualizar un vehículo existente (recibe id + DTO)
        public async Task<bool> UpdateAsync(int id, VehiculoDto vehiculoDto)
        {
            var existing = await _context.Vehiculos
                                         .FirstOrDefaultAsync(v => v.Id == id)
                                         .ConfigureAwait(false);

            if (existing == null) return false;

            // Asignar valores a las variables para solo los campos que cambian
            /*
            Cada Campo ordenado

            existing.Marca = vehiculoDto.Marca;
            existing.Modelo = vehiculoDto.Modelo;
            existing.Año = vehiculoDto.Año;
            existing.Placa = vehiculoDto.Placa;
            existing.Color = vehiculoDto.Color;
            existing.TipoVehiculo = vehiculoDto.TipoVehiculo;
            existing.CategoriaId = vehiculoDto.CategoriaId;
            existing.Estado = vehiculoDto.Estado;
            existing.ImageUrl = vehiculoDto.ImageUrl;
            existing.EsActivo = vehiculoDto.EsActivo;
            */
            if (existing.Marca != vehiculoDto.Marca)
            {
                existing.Marca = vehiculoDto.Marca;
            }
            if (existing.Modelo != vehiculoDto.Modelo)
            {
                existing.Modelo = vehiculoDto.Modelo;
            }
            if (existing.Año != vehiculoDto.Año)
            {
                existing.Año = vehiculoDto.Año;
            }
            if (existing.Placa != vehiculoDto.Placa)
            {
                existing.Placa = vehiculoDto.Placa;
            }
            if (existing.Color != vehiculoDto.Color)
            {
                existing.Color = vehiculoDto.Color;
            }
            if (existing.TipoVehiculo != vehiculoDto.TipoVehiculo)
            {
                existing.TipoVehiculo = vehiculoDto.TipoVehiculo;
            }
            if (existing.CategoriaId != vehiculoDto.CategoriaId)
            {
                existing.CategoriaId = vehiculoDto.CategoriaId;
            }
            if (existing.Estado != vehiculoDto.Estado)
            {
                existing.Estado = vehiculoDto.Estado;
            }
            if (existing.ImageUrl != vehiculoDto.ImageUrl)
            {
                existing.ImageUrl = vehiculoDto.ImageUrl;
            }
            if (existing.EsActivo != vehiculoDto.EsActivo)
            {
                existing.EsActivo = vehiculoDto.EsActivo;
            }
            if (existing.PrecioPorDia!= vehiculoDto.PrecioPorDia)
            {
                existing.PrecioPorDia = vehiculoDto.PrecioPorDia;
            }

            // Para respetar la fecha de registro original, NO tocar
            // Si permites editarla desde el DTO:
            if (vehiculoDto.FechaRegistro != default)
            {
                existing.FechaRegistro = vehiculoDto.FechaRegistro;
            }

            _context.Vehiculos.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        // ------------------ ESTADO / ELIMINACIÓN ------------------

        // Desactivar un vehículo (eliminación lógica)
        public async Task<bool> DeactivateAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return false;

            vehiculo.EsActivo = false;
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();

            return true;
        }

        // Eliminar un vehículo (eliminación física)
        public async Task<bool> EliminateAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null || vehiculo.EsActivo != true) return false;

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return true;
        }

        // Reactivar un vehículo
        public async Task<bool> ActivateAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return false;

            vehiculo.EsActivo = true;
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
