using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class AsistenteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsistenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddAsistente()
        {
            List<CentroCultural> centros = _context.CentrosCulturales.ToList();
            ViewData["CentrosCulturales"] = centros;

            List<string> tipos = new List<string> { "Niño", "Adolescente", "Adulto" };
            ViewData["Tipos"] = tipos;

            return View();
        }

        [HttpPost]
        public IActionResult AddAsistente(Asistente a)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                    ViewData["Tipos"] = new List<string> { "Niño", "Adolescente", "Adulto" };
                    return View(a);
                }

                _context.Asistentes.Add(a);
                _context.SaveChanges();
                return RedirectToAction("ListAsistente");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar el asistente.");
                ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                ViewData["Tipos"] = new List<string> { "Niño", "Adolescente", "Adulto" };
                return View(a);
            }
        }

        public IActionResult ListAsistente()
        {
            List<Asistente> asistentes = _context.Asistentes.Include(a => a.CentroCulturalAsignado).ToList();
            return View(asistentes);
        }

        public IActionResult UpdateAsistente(int id)
        {
            Asistente asistente = _context.Asistentes.FirstOrDefault(a => a.Id == id);
            List<CentroCultural> centros = _context.CentrosCulturales.ToList();
            ViewData["CentrosCulturales"] = centros;

            List<string> tipos = new List<string> { "Niño", "Adolescente", "Adulto" };
            ViewData["Tipos"] = tipos;

            return View(asistente);
        }

        [HttpPost]
        public IActionResult UpdateAsistente(Asistente a)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                    ViewData["Tipos"] = new List<string> { "Niño", "Adolescente", "Adulto" };
                    return View(a);
                }

                _context.Asistentes.Update(a);
                _context.SaveChanges();

                return RedirectToAction("ListAsistente");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar el asistente.");
                ViewData["CentrosCulturales"] = _context.CentrosCulturales.ToList();
                ViewData["Tipos"] = new List<string> { "Niño", "Adolescente", "Adulto" };
                return View(a);
            }
        }

        [HttpPost]
        public IActionResult DeleteAsistente(int id)
        {
            var asistente = _context.Asistentes.Find(id);

            if (asistente != null)
            {
                var relaciones = _context.Set<AsistenteTaller>().Where(at => at.AsistenteId == id).ToList();
                _context.Set<AsistenteTaller>().RemoveRange(relaciones);

                _context.Asistentes.Remove(asistente);
                _context.SaveChanges();
            }

            return RedirectToAction("ListAsistente");
        }

        public IActionResult DetailsAsistente(int id)
        {
            var asistente = _context.Asistentes
                .Include(a => a.CentroCulturalAsignado)
                .Include(a => a.AsistentesTalleres)        
                    .ThenInclude(at => at.TallerAsignado)   
                .FirstOrDefault(a => a.Id == id);

            if (asistente == null)
                return NotFound();

            return View(asistente);
        }
    }
}
