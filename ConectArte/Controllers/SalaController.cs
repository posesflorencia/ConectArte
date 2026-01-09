using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class SalaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddSala()
        {
            ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
            ViewData["Recursos"] = _context.Recursos.Where(r => r.Disponible).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddSala(Sala s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                    ViewData["Recursos"] = _context.Recursos.ToList();
                    return View(s);
                }

                _context.Salas.Add(s);
                _context.SaveChanges();

                if (s.RecursosIds != null && s.RecursosIds.Any())
                {
                    var salaGuardada = _context.Salas.Include(sa => sa.Recursos).FirstOrDefault(sa => sa.Id == s.Id);
                    if (salaGuardada != null)
                    {
                        var recursos = _context.Recursos.Where(r => s.RecursosIds.Contains(r.Id)).ToList();
                        salaGuardada.Recursos = recursos;
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction("ListSala");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar la sala.");
                ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                ViewData["Recursos"] = _context.Recursos.ToList();
                return View(s);
            }
        }

        public IActionResult ListSala()
        {
            var salas = _context.Salas
                .Include(s => s.CentroCulturalAsignado)
                .Include(s => s.Recursos)
                .ToList();

            return View(salas);
        }

        public IActionResult UpdateSala(int id)
        {
            var sala = _context.Salas
                .Include(s => s.Recursos)
                .FirstOrDefault(s => s.Id == id);

            if (sala == null)
                return NotFound();

            ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();

            var recursosAsignadosIds = sala.Recursos.Select(r => r.Id).ToList();
            ViewData["Recursos"] = _context.Recursos
                .Where(r => r.Disponible || recursosAsignadosIds.Contains(r.Id))
                .ToList();

            sala.RecursosIds = recursosAsignadosIds;

            return View(sala);
        }

        [HttpPost]
        public IActionResult UpdateSala(Sala s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                    ViewData["Recursos"] = _context.Recursos.ToList();
                    return View(s);
                }

                var salaExistente = _context.Salas
                    .Include(sa => sa.Recursos)
                    .FirstOrDefault(sa => sa.Id == s.Id);

                if (salaExistente == null)
                    return NotFound();

                salaExistente.CapacidadMaxima = s.CapacidadMaxima;
                salaExistente.CentroCulturalId = s.CentroCulturalId;

                salaExistente.Recursos.Clear();

                if (s.RecursosIds != null && s.RecursosIds.Any())
                {
                    var recursosSeleccionados = _context.Recursos
                                                .Where(r => s.RecursosIds.Contains(r.Id))
                                                .ToList();

                    foreach (var recurso in recursosSeleccionados)
                    {
                        salaExistente.Recursos.Add(recurso);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("ListSala");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar la sala.");
                ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                ViewData["Recursos"] = _context.Recursos.ToList();
                return View(s);
            }
        }

        [HttpPost]
        public IActionResult DeleteSala(int id)
        {
            var sala = _context.Salas.Find(id);

            if (sala != null)
            {
                _context.Salas.Remove(sala);
                _context.SaveChanges();
            }

            return RedirectToAction("ListSala");
        }

        public IActionResult DetailsSala(int id)
        {
            var sala = _context.Salas
                .Include(s => s.CentroCulturalAsignado)
                .Include(s => s.Recursos)
                .Include(s => s.Talleres)
                .FirstOrDefault(s => s.Id == id);

            if (sala == null)
                return NotFound();

            return View(sala);
        }

        [HttpPost]
        public IActionResult DeleteAssignedRecurso(int salaId, int recursoId)
        {
            var sala = _context.Salas
                .Include(s => s.Recursos)
                .FirstOrDefault(s => s.Id == salaId);

            if (sala == null)
                return NotFound();

            var recurso = sala.Recursos.FirstOrDefault(r => r.Id == recursoId);
            if (recurso != null)
            {
                sala.Recursos.Remove(recurso);
                _context.SaveChanges();
            }

            return RedirectToAction("DetailsSala", new { id = salaId });
        }
    }
}
