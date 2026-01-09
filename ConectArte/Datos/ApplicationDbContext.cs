using ConectArte.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ConectArte.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DEFINIR CLAVE PRIMARIA COMPUESTA
            modelBuilder.Entity<AsistenteTaller>()
                .HasKey(at => new { at.AsistenteId, at.TallerId });

            modelBuilder.Entity<AsistenteTaller>()
                .HasOne(at => at.AsistenteAsignado)
                .WithMany(a => a.AsistentesTalleres)
                .HasForeignKey(pt => pt.AsistenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsistenteTaller>()
                .HasOne(at => at.TallerAsignado)
                .WithMany(t => t.AsistentesTalleres)
                .HasForeignKey(at => at.TallerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Asistente> Asistentes { get; set; }
        public DbSet<CentroCultural> CentrosCulturales { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Taller> Talleres { get; set; }
    }
}
