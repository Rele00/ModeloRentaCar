using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Renta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VehiculoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Required]
        public DateTime FechaEstimadaDevolucion { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioDiario { get; set; }

        [Required]
        public int CantidadDias { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoEstimado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontoFinal { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoRenta { get; set; } = "Activa"; // Activa, Cerrada, Cancelada

        [MaxLength(200)]
        public string? Notas { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        [ForeignKey(nameof(VehiculoId))]
        public virtual Vehiculo? Vehiculo { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public virtual Cliente? Cliente { get; set; }

        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

        public virtual Factura? Factura { get; set; }
    }
}
