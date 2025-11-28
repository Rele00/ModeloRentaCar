using System.ComponentModel.DataAnnotations;

namespace RentCar.Data.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(10, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "El {0} debe contener exactamente 10 dígitos (solo números).")]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(13, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$",
            ErrorMessage = "La {0} debe tener el formato 000-0000000-0.")]
        public string Cedula { get; set; } = string.Empty;

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(100, ErrorMessage = "El {0} no puede tener más de {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El {0} debe ser una dirección de correo válida.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Licencia")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [StringLength(11, ErrorMessage = "La {0} no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^\d{11}$",
            ErrorMessage = "La {0} debe contener exactamente 11 dígitos (solo números).")]
        public string Licencia { get; set; } = string.Empty;

        [Display(Name = "Vigencia de la licencia")]
        [Required(ErrorMessage = "La {0} es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime VigenciaDeLicencia { get; set; }

        [Display(Name = "Cliente activo")]
        public bool EsActivo { get; set; } = true;
    }
}
