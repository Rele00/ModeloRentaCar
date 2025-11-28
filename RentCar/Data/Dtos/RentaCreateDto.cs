using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Data.Dtos
{
    public class RentaCreateDto
    {
        [Required(ErrorMessage = "Seleccione un cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Cliente inválido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Seleccione un vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Vehículo inválido")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha estimada de devolución es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaEstimadaDevolucion { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "El precio diario es obligatorio")]
        [Range(0.01, 9999999, ErrorMessage = "Precio diario inválido")]
        public decimal PrecioDiario { get; set; }

        [Required]
        [Range(1, 3650, ErrorMessage = "Cantidad de días inválida")]
        public int CantidadDias { get; set; }

        [Required]
        [Range(0.01, 999999999, ErrorMessage = "Monto estimado inválido")]
        public decimal MontoEstimado { get; set; }

        [MaxLength(200)]
        public string? Notas { get; set; }

        [MaxLength(20)]
        public string MetodoPago { get; set; } = "Efectivo";
    }
}
