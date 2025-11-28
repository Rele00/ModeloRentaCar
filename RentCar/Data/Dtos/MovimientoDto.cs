using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Data.Dtos
{
    /// <summary>
    /// DTO para mostrar los movimientos de una renta (historial)
    /// y, si se usa en formularios, validar los campos básicos.
    /// </summary>
    public class MovimientoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La renta es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Renta inválida")]
        public int RentaId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string TipoMovimiento { get; set; } = string.Empty;
        // Ej: "RentaCreada", "Devolucion", "Pago", "Ajuste"

        [Required(ErrorMessage = "La fecha del movimiento es obligatoria")]
        public DateTime FechaMovimiento { get; set; }

        [Range(0, 999999999, ErrorMessage = "Monto inválido")]
        public decimal Monto { get; set; }

        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string? Descripcion { get; set; }

        // --- Datos de contexto (normalmente solo lectura) ---
        [Range(0, int.MaxValue, ErrorMessage = "Cliente inválido")]
        public int ClienteId { get; set; }

        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string NombreCliente { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Vehículo inválido")]
        public int VehiculoId { get; set; }

        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string VehiculoDescripcion { get; set; } = string.Empty;
    }
}
