using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RentaId { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroFactura { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Impuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [MaxLength(20)]
        public string MetodoPago { get; set; } = "Efectivo"; // Efectivo, Tarjeta, Transferencia...

        [MaxLength(200)]
        public string? Notas { get; set; }

        // Navegación
        [ForeignKey(nameof(RentaId))]
        public virtual Renta? Renta { get; set; }
    }
}
