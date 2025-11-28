using System.ComponentModel.DataAnnotations;

public class CategoriaDto
{
    public int Id { get; set; }

    [Display(Name = "Nombre de la categoría")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
    public string NombreCategoria { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [StringLength(200, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
    public string? Descripcion { get; set; }

    // Opcional: para eliminación lógica
    public bool Activo { get; set; } = true;
}
