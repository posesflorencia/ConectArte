using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectArte.Models
{
    public class Taller
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 25 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El tipo es obligatorio.")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 50, ErrorMessage = "La capacidad debe estar entre 1 y 50.")]
        public int CapacidadMaxima { get; set; }
        [Required(ErrorMessage = "La duración en horas es obligatoria.")]
        [Range(1, 6, ErrorMessage = "La duración debe estar entre 1 y 6 horas.")]
        public int DuracionHoras { get; set; }
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una sala.")]
        public int SalaId { get; set; }
        [ForeignKey("SalaId")]
        [ValidateNever]
        public Sala SalaAsignada { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un docente.")]
        public int DocenteId { get; set; }
        [ForeignKey("DocenteId")]
        [ValidateNever]
        public Docente DocenteAsignado { get; set; }

        [ValidateNever]
        public List<AsistenteTaller> AsistentesTalleres {  get; set; } = new List<AsistenteTaller>();

        //Atributo calculado: CalificacionPromedio
        [NotMapped]
        public double CalificacionPromedio
        {
            get
            {
                var calificacionesValidas = AsistentesTalleres
                    .Where(at => at.Calificacion.HasValue)
                    .Select(at => at.Calificacion.Value)
                    .ToList();

                if (calificacionesValidas.Count == 0)
                {
                    return 0;
                }

                return calificacionesValidas.Average();
            }
        }

        [NotMapped]
        public List<int> AsistentesIds { get; set; } = new List<int>();
    }
}
