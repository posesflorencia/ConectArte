using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class FiltrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FiltrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Filtro 1:

        [HttpGet]
        public IActionResult AsistentesSinTallerConRecurso()
        {
            ViewData["TiposRecurso"] = _context.Recursos
                .Select(r => r.Tipo)
                .Distinct()
                .ToList();

            return View(new List<Asistente>());
        }

        [HttpPost]
        public IActionResult AsistentesSinTallerConRecurso(string tipoRecurso)
        {
            ViewData["TiposRecurso"] = _context.Recursos
                .Select(r => r.Tipo)
                .Distinct()
                .ToList();

            // Obtener talleres que usen ese recurso (a través de la Sala)
            var talleresConRecurso = _context.Talleres
                .Include(t => t.SalaAsignada)
                    .ThenInclude(s => s.Recursos)
                .Where(t => t.SalaAsignada.Recursos.Any(r => r.Tipo == tipoRecurso))
                .Select(t => t.Id)
                .ToList();

            // Obtener asistentes que NO participaron en esos talleres
            var asistentes = _context.Asistentes
                .Include(a => a.AsistentesTalleres)
                .Where(a => !a.AsistentesTalleres.Any(at => talleresConRecurso.Contains(at.TallerId)))
                .ToList();

            if (asistentes.Count == 0)
                ViewData["Mensaje"] = $"No se encontraron asistentes que no hayan participado en talleres con el recurso '{tipoRecurso}'.";
            else
                ViewData["Mensaje"] = null;

            return View(asistentes);
        }

        //Filtro 2:

        [HttpGet]
        public IActionResult Top5AsistentesConMasTalleres()
        {
            var top5 = _context.Asistentes
                .Include(a => a.AsistentesTalleres)
                .OrderByDescending(a => a.AsistentesTalleres.Count)
                .Take(5)
                .ToList();

            return View(top5);
        }
    }
}
