using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class TallerController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> tiposTaller = new List<string>
        {
            "Arte", "Música", "Teatro", "Tecnología", "Danza", "Literatura"
        };

        public TallerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddTaller()
        {
            ViewData["Salas"] = _context.Salas.ToList();
            ViewData["Docentes"] = _context.Docentes.ToList();
            ViewData["Asistentes"] = _context.Asistentes.ToList();
            ViewData["TiposTaller"] = tiposTaller; 
            return View();
        }

        [HttpPost]
        public IActionResult AddTaller(Taller t)
        {
            try
            {
                if (t.Fecha.Date < DateTime.Today.Date)
                {
                    ModelState.AddModelError("Fecha", "La fecha debe ser mayor o igual a la de hoy");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["Salas"] = _context.Salas.ToList();
                    ViewData["Docentes"] = _context.Docentes.ToList();
                    ViewData["Asistentes"] = _context.Asistentes.ToList();
                    ViewData["TiposTaller"] = tiposTaller;
                    return View(t);
                }

                _context.Talleres.Add(t);
                _context.SaveChanges();

                if (t.AsistentesIds != null)
                {
                    foreach (int id in t.AsistentesIds)
                    {
                        AsistenteTaller at = new AsistenteTaller
                        {
                            AsistenteId = id,
                            TallerId = t.Id,
                            Calificacion = null
                        };
                        _context.Add(at);
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("ListTaller");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar el taller.");
                ViewData["Salas"] = _context.Salas.ToList();
                ViewData["Docentes"] = _context.Docentes.ToList();
                ViewData["Asistentes"] = _context.Asistentes.ToList();
                return View(t);
            }
        }

        public IActionResult ListTaller()
        {
            var talleres = _context.Talleres
                .Include(t => t.SalaAsignada)
                .Include(t => t.DocenteAsignado)
                .Include(t => t.AsistentesTalleres)
                .ToList();

            return View(talleres);
        }

        public IActionResult UpdateTaller(int id)
        {
            Taller taller = _context.Talleres
                .Include(t => t.AsistentesTalleres)
                .FirstOrDefault(t => t.Id == id);

            ViewData["Salas"] = _context.Salas.ToList();
            ViewData["Docentes"] = _context.Docentes.ToList();
            ViewData["Asistentes"] = _context.Asistentes.ToList();
            ViewData["TiposTaller"] = tiposTaller; 

            taller.AsistentesIds = taller.AsistentesTalleres.Select(at => at.AsistenteId).ToList();

            return View(taller);
        }

        [HttpPost]
        public IActionResult UpdateTaller(Taller t)
        {
            if (t.Fecha.Date < DateTime.Today.Date)
            {
                ModelState.AddModelError("Fecha", "La fecha debe ser mayor o igual a la de hoy");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Salas"] = _context.Salas.ToList();
                ViewData["Docentes"] = _context.Docentes.ToList();
                ViewData["TiposTaller"] = tiposTaller;
                ViewData["Asistentes"] = _context.Asistentes.ToList();

                return View(t);
            }

            try
            {
                _context.Talleres.Update(t);
                _context.SaveChanges();

                var relacionesExistentes = _context.Set<AsistenteTaller>()
                    .Where(at => at.TallerId == t.Id)
                    .ToList();

                var asistentesNuevos = t.AsistentesIds
                    .Where(id => !relacionesExistentes.Any(at => at.AsistenteId == id))
                    .ToList();

                var relacionesAEliminar = relacionesExistentes
                    .Where(at => !t.AsistentesIds.Contains(at.AsistenteId))
                    .ToList();

                if (relacionesAEliminar.Any())
                {
                    _context.Set<AsistenteTaller>().RemoveRange(relacionesAEliminar);
                }

                foreach (var idNuevo in asistentesNuevos)
                {
                    var nuevaRelacion = new AsistenteTaller
                    {
                        AsistenteId = idNuevo,
                        TallerId = t.Id,
                        Calificacion = null
                    };
                    _context.Set<AsistenteTaller>().Add(nuevaRelacion);
                }

                _context.SaveChanges();
                return RedirectToAction("ListTaller");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar el taller.");

                ViewData["Salas"] = _context.Salas.ToList();
                ViewData["Docentes"] = _context.Docentes.ToList();
                ViewData["TiposTaller"] = tiposTaller;
                ViewData["Asistentes"] = _context.Asistentes.ToList();

                return View(t);
            }
        }

        public IActionResult DetailsTaller(int id)
        {
            Taller taller = _context.Talleres
                .Include(t => t.SalaAsignada)
                .Include(t => t.DocenteAsignado)
                .Include(t => t.AsistentesTalleres)
                    .ThenInclude(at => at.AsistenteAsignado)
                .FirstOrDefault(t => t.Id == id);

            return View(taller);
        }

        [HttpPost]
        public IActionResult DeleteTaller(int id)
        {
            Taller t = _context.Talleres.Find(id);

            if (t != null)
            {
                _context.Talleres.Remove(t);
                _context.SaveChanges();
            }

            return RedirectToAction("ListTaller");
        }

        [HttpPost]
        public IActionResult DeleteAssignedAsistente(int tallerId, int asistenteId)
        {
            var relacion = _context.Set<AsistenteTaller>()
                .FirstOrDefault(at => at.TallerId == tallerId && at.AsistenteId == asistenteId);

            if (relacion != null)
            {
                _context.Set<AsistenteTaller>().Remove(relacion);
                _context.SaveChanges();
            }

            return RedirectToAction("DetailsTaller", new { id = tallerId });
        }

        public IActionResult UpdateCalificacion(int tallerId, int asistenteId)
        {
            var relacion = _context.Set<AsistenteTaller>()
                .Include(at => at.AsistenteAsignado)
                .Include(at => at.TallerAsignado)
                .FirstOrDefault(at => at.TallerId == tallerId && at.AsistenteId == asistenteId);

            if (relacion == null)
                return NotFound();

            return View(relacion);
        }

        [HttpPost]
        public IActionResult UpdateCalificacion(AsistenteTaller at)
        {
            var existente = _context.Set<AsistenteTaller>()
                .FirstOrDefault(rel => rel.TallerId == at.TallerId && rel.AsistenteId == at.AsistenteId);

            if (existente != null)
            {
                existente.Calificacion = at.Calificacion;
                _context.SaveChanges();
            }

            return RedirectToAction("DetailsTaller", new { id = at.TallerId });
        }
    }
}
