using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectArte.Models
{
    public class Asistente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El nombre completo debe tener entre 5 y 25 caracteres.")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo.")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un centro cultural.")]
        public int CentroCulturalId { get; set; }
        [ForeignKey("CentroCulturalId")]
        [ValidateNever]
        public CentroCultural CentroCulturalAsignado { get; set; }

        [ValidateNever]
        public List<AsistenteTaller> AsistentesTalleres { get; set; } = new List<AsistenteTaller>();
    }
}
