using Microsoft.EntityFrameworkCore;
using RentCar.Data.Context;
using RentCar.Data.Dtos;
using RentCar.Data.Models;

namespace RentCar.Data.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Entidad -> DTO
        private static ClienteDto ToDto(Cliente c) =>
            new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Telefono = c.Telefono,
                Cedula = c.Cedula,
                Email = c.Email,
                Licencia = c.Licencia,
                VigenciaDeLicencia = c.VigenciaDeLicencia,
                EsActivo = c.EsActivo
            };

        // DTO -> Entidad (para crear)
        private static Cliente ToEntity(ClienteDto dto) =>
            new Cliente
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono,
                Cedula = dto.Cedula,
                Email = dto.Email,
                Licencia = dto.Licencia,
                VigenciaDeLicencia = dto.VigenciaDeLicencia,
                EsActivo = dto.EsActivo
            };

        // Obtener solo activos
        public async Task<List<ClienteDto>> GetAll()
        {
            var clientes = await _context.Clientes
                                         .Where(c => c.EsActivo)
                                         .ToListAsync();

            return clientes.Select(ToDto).ToList();
        }

        // Obtener todos (activos e inactivos)
        public async Task<List<ClienteDto>> GetAllAsync()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes.Select(ToDto).ToList();
        }
        // Obtener todos (inactivos)
        public async Task<List<ClienteDto>> GetAllInactivesAsync()
        {
            var clientes = await _context.Clientes.Where(c => c.EsActivo == false).ToListAsync();
            return clientes.Select(ToDto).ToList();
        }

        // Obtener por Id
        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes
                                        .FirstOrDefaultAsync(c => c.Id == id);

            return cliente is null ? null : ToDto(cliente);
        }

        // Crear (recibe DTO, devuelve DTO)
        public async Task<ClienteDto> CreateAsync(ClienteDto clienteDto)
        {
            var entity = ToEntity(clienteDto);

            // Aseguramos que entre como activo por defecto
            if (!entity.EsActivo)
                entity.EsActivo = true;

            _context.Clientes.Add(entity);
            await _context.SaveChangesAsync();

            return ToDto(entity);
        }

        // Actualizar (recibe DTO) usando comparaciones propiedad a propiedad
        public async Task<bool> UpdateAsync(ClienteDto clienteDto)
        {
            var existingCliente = await _context.Clientes.FindAsync(clienteDto.Id);
            if (existingCliente == null) return false;

            if (existingCliente.Nombre != clienteDto.Nombre)
                existingCliente.Nombre = clienteDto.Nombre;

            if (existingCliente.Apellido != clienteDto.Apellido)
                existingCliente.Apellido = clienteDto.Apellido;

            if (existingCliente.Telefono != clienteDto.Telefono)
                existingCliente.Telefono = clienteDto.Telefono;

            if (existingCliente.Cedula != clienteDto.Cedula)
                existingCliente.Cedula = clienteDto.Cedula;

            if (existingCliente.Email != clienteDto.Email)
                existingCliente.Email = clienteDto.Email;

            if (existingCliente.Licencia != clienteDto.Licencia)
                existingCliente.Licencia = clienteDto.Licencia;

            if (existingCliente.VigenciaDeLicencia != clienteDto.VigenciaDeLicencia)
                existingCliente.VigenciaDeLicencia = clienteDto.VigenciaDeLicencia;

            if (existingCliente.EsActivo != clienteDto.EsActivo)
                existingCliente.EsActivo = clienteDto.EsActivo;

            _context.Clientes.Update(existingCliente);
            await _context.SaveChangesAsync();
            return true;
        }

        // Desactivar (eliminación lógica)
        public async Task<bool> DeactivateAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.EsActivo = false;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
        // Eliminación física
        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }


        // Reactivar
        public async Task<bool> ActivateAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.EsActivo = true;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
