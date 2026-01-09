using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectArte.Models
{
    public class Sala
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 50, ErrorMessage = "La capacidad debe estar entre 1 y 50.")]
        public int CapacidadMaxima { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un centro cultural.")]
        public int CentroCulturalId { get; set; }
        [ForeignKey("CentroCulturalId")]
        [ValidateNever]
        public CentroCultural CentroCulturalAsignado { get; set; }

        [ValidateNever]
        public List<Recurso> Recursos { get; set; } = new List<Recurso>();
        [NotMapped]
        public List<int> RecursosIds { get; set; } = new List<int>();

        [ValidateNever]
        public List<Taller> Talleres { get; set; } = new List<Taller>();

    }
}
