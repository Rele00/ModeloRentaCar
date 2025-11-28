using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        public int Año { get; set; }

        [Required, MaxLength(15)]
        public string Placa { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Color { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TipoVehiculo { get; set; } = string.Empty;

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public virtual Categoria? Categoria { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; } = "Disponible"; //Disponible, Mantenimiento, Renta

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PrecioPorDia { get; set; } //Disponible, Mantenimiento, Renta

        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}