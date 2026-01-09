using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ConectArte.Models
{
    public class Recurso
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo.")]
        public string Tipo { get; set; }
        public bool Disponible { get; set; }

        [ValidateNever]
        public List<Sala> Salas { get; set; } = new List<Sala>();
    }
}
