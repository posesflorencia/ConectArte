using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ConectArte.Models
{
    public class CentroCultural
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del centro cultural es obligatorio.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 25 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(30, ErrorMessage = "La ciudad no debe exceder los 30 caracteres.")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public int Telefono { get; set; }

        //Coordinador
        [Required(ErrorMessage = "El nombre del coordinador es obligatorio.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El nombre del coordinador debe tener entre 5 y 25 caracteres.")]
        public string NombreCoordinador { get; set; }
        [Required(ErrorMessage = "El teléfono del coordinador es obligatorio.")]
        public int TelefonoCoordinador { get; set; }

        [ValidateNever]
        public List<Sala> Salas { get; set; } = new List<Sala>();
        [ValidateNever]
        public List<Asistente> Asistentes { get; set; } = new List<Asistente>();
    }
}
