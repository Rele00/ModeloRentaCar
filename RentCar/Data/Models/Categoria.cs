using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Data.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string NombreCategoria { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; } = string.Empty;

        // Para la eliminación lógica:
        public bool Activo { get; set; } = true;

        // Relación inversa correcta: colecciones no deben llevar atributos como MaxLength
        // y deben inicializarse para evitar problemas con EF Core y nulls.
        [InverseProperty(nameof(Vehiculo.Categoria))]
        public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }
}
