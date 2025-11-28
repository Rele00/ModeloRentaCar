using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentCar.Data.Models;

namespace RentCar.Data.Context
{
    public interface IApplicationDbContext
    {
        DbSet<Cliente> Clientes { get; set; }
        DbSet<Vehiculo> Vehiculos { get; set; }
        DbSet<Movimiento> Movimientos { get; set; }
        DbSet<Renta> Rentas { get; set; }
        DbSet<Factura> Facturas { get; set; }
        DbSet<IdentityRole> Roles { get; set; }
        DbSet<Categoria> Categorias { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehiculo> Vehiculos { get; set; } = default!;
        public DbSet<Cliente> Clientes { get; set; } = default!;
        public DbSet<Movimiento> Movimientos { get; set; } = default!;
        public DbSet<Renta> Rentas { get; set; } = default!;
        public DbSet<Factura> Facturas { get; set; } = default!;
        public DbSet<Categoria> Categorias { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Renta>()
                .HasOne(r => r.Factura)
                .WithOne(f => f.Renta!)
                .HasForeignKey<Factura>(f => f.RentaId);

            builder.Entity<Renta>()
                .HasMany(r => r.Movimientos)
                .WithOne(m => m.Renta!)
                .HasForeignKey(m => m.RentaId);

            builder.Entity<Factura>()
                .HasIndex(f => f.NumeroFactura)
                .IsUnique();
        }
    }
}
