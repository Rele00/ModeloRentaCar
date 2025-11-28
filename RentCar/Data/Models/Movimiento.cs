using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Movimiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RentaId { get; set; }

        [Required]
        [MaxLength(30)]
        public string TipoMovimiento { get; set; } = "RentaCreada";
        // Ejemplos: "RentaCreada", "Devolucion", "Pago", "Ajuste"

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        // Navegación
        [ForeignKey(nameof(RentaId))]
        public virtual Renta? Renta { get; set; }
    }
}
