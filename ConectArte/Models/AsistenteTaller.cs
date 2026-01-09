using System.ComponentModel.DataAnnotations.Schema;

namespace ConectArte.Models
{
    public class AsistenteTaller
    {
        public int AsistenteId { get; set; }
        [ForeignKey("AsistenteId")]
        public Asistente AsistenteAsignado { get; set; }
        public int TallerId { get; set; }
        [ForeignKey("TallerId")]
        public Taller TallerAsignado { get; set; }
        public int? Calificacion {  get; set; }
    }
}
