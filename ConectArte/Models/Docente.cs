using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ConectArte.Models
{
    public class Docente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El nombre completo debe tener entre 5 y 25 caracteres.")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        public string Especialidad { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public int Telefono { get; set; }
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        [ValidateNever]
        public List<Taller> Talleres { get; set; } = new List<Taller>();
    }
}
