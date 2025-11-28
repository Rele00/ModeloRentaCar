using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Data.Dtos
{
    public class RentaDevolucionDto
    {
        // Datos necesarios para la lógica
        [Required]
        public int RentaId { get; set; }

        [Required(ErrorMessage = "La fecha de devolución es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaDevolucionReal { get; set; } = DateTime.Today;

        [Range(0, 999999999, ErrorMessage = "Recargo inválido")]
        public decimal Recargo { get; set; }

        [Range(0, 999999999, ErrorMessage = "Descuento inválido")]
        public decimal Descuento { get; set; }

        [MaxLength(200)]
        public string? Notas { get; set; }

        // Datos solo de lectura para mostrar al usuario
        public int ClienteId { get; set; }
        public string? NombreCliente { get; set; }

        public int VehiculoId { get; set; }
        public string? DescripcionVehiculo { get; set; }

        public DateTime FechaSalida { get; set; }
        public DateTime FechaEstimadaDevolucion { get; set; }

        public decimal PrecioDiario { get; set; }
        public int CantidadDiasEstimada { get; set; }
        public decimal MontoEstimado { get; set; }

        public decimal? MontoFinalCalculado { get; set; }
        public decimal? TotalFacturaActual { get; set; }
    }
}
