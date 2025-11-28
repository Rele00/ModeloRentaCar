using Microsoft.EntityFrameworkCore;
using RentCar.Data.Context;
using RentCar.Data.Dtos;
using RentCar.Data.Models;

namespace RentCar.Data.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------- MAPEOS ENTRE ENTIDAD Y DTO ----------

        private static CategoriaDto ToDto(Categoria c) =>
            new CategoriaDto
            {
                Id = c.Id,
                NombreCategoria = c.NombreCategoria,
                Descripcion = c.Descripcion,
                // Si tu entidad tiene Activo:
                Activo = c.Activo
            };

        private static void UpdateEntityFromDto(Categoria entity, CategoriaDto dto)
        {
            entity.NombreCategoria = dto.NombreCategoria;
            entity.Descripcion = dto.Descripcion;

            // Si manejas Activo desde la UI:
            entity.Activo = dto.Activo;
        }

        // ---------- LECTURAS ----------

        public async Task<List<CategoriaDto>> GetAllAsync()
        {
            // Trae todas las categorías (puedes filtrar por Activo si quieres solo las activas)
            var categorias = await _context.Set<Categoria>()
                                           .OrderBy(c => c.NombreCategoria)
                                           .ToListAsync();

            return categorias.Select(ToDto).ToList();
        }

        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            var categoria = await _context.Set<Categoria>().FindAsync(id);
            return categoria is null ? null : ToDto(categoria);
        }

        public async Task<string> GetNombreCategoriaAsync(int id)
        {
            var dto = await GetByIdAsync(id);
            return dto?.NombreCategoria ?? string.Empty;
        }

        // ---------- CREAR ----------

        public async Task<CategoriaDto> CreateAsync(CategoriaDto categoriaDto)
        {
            var entity = new Categoria
            {
                NombreCategoria = categoriaDto.NombreCategoria,
                Descripcion = categoriaDto.Descripcion,
                // Si tu modelo tiene Activo:
                Activo = true
            };

            _context.Set<Categoria>().Add(entity);
            await _context.SaveChangesAsync();

            // Devuelvo el DTO actualizado con el Id generado
            return ToDto(entity);
        }

        // ---------- ACTUALIZAR ----------

        public async Task<bool> UpdateAsync(CategoriaDto categoriaDto)
        {
            var entity = await _context.Set<Categoria>().FindAsync(categoriaDto.Id);
            if (entity is null)
                return false;

            UpdateEntityFromDto(entity, categoriaDto);
            await _context.SaveChangesAsync();
            return true;
        }

        // ---------- DESACTIVAR (ELIMINACIÓN LÓGICA) ----------

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Set<Categoria>().FindAsync(id);
            if (entity is null)
                return false;

            // Eliminación lógica
            entity.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // ---------- REACTIVAR ----------

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _context.Set<Categoria>().FindAsync(id);
            if (entity is null)
                return false;

            entity.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
