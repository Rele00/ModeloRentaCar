using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public string Telefono { get; set; } = string.Empty;

        [Required, MaxLength(13)]
        public string Cedula { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(11)]
        public string Licencia { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de vencimiento de la licencia. Se recomienda guardar solo la fecha (sin hora).
        /// </summary>
        [Required, Column(TypeName = "date")]
        public DateTime VigenciaDeLicencia { get; set; }

        [Required]
        public bool EsActivo { get; set; } = true;

        // Relaciones
        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}