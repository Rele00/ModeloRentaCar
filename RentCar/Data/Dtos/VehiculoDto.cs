using System.ComponentModel.DataAnnotations;

public class VehiculoDto
{
    public int Id { get; set; }

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "La {0} es obligatoria.")]
    [StringLength(50, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
    public string Marca { get; set; } = string.Empty;

    [Display(Name = "Modelo")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Display(Name = "Año")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    [Range(1900, 2100, ErrorMessage = "El {0} debe estar entre {1} y {2}.")]
    public int Año { get; set; }

    [Display(Name = "Placa")]
    [Required(ErrorMessage = "La {0} es obligatoria.")]
    [StringLength(15, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
    public string Placa { get; set; } = string.Empty;

    [Display(Name = "Color")]
    [StringLength(30, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
    public string Color { get; set; } = string.Empty;

    [Display(Name = "Tipo de vehículo")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
    public string TipoVehiculo { get; set; } = string.Empty;

    [Display(Name = "Categoría")]
    [Required(ErrorMessage = "La {0} es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una {0} válida.")]
    public int CategoriaId { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    [StringLength(20, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
    public string Estado { get; set; } = "Disponible";

    [Display(Name = "PrecioPorDia")]
    [Required(ErrorMessage = "El {0} es obligatorio.")]
    public decimal PrecioPorDia { get; set; }

    [Display(Name = "URL de imagen")]
    [Required(ErrorMessage = "La {0} es obligatoria.")]
    [Url(ErrorMessage = "La {0} debe ser una dirección URL válida.")]
    [StringLength(255, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "Fecha de registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Display(Name = "Vehículo activo")]
    public bool EsActivo { get; set; } = true;
}