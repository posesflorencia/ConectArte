using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class RecursoController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> tiposRecurso = new List<string>
        {
            "Proyector", "Parlantes", "Piano", "Computadoras", "Micrófono", "Tablets", "Vestuario", "Maquillaje"
        };

        public RecursoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ListRecurso()
        {
            var recursos = _context.Recursos
                            .Include(r => r.Salas)
                            .ToList();
            return View(recursos);
        }

        public IActionResult AddRecurso()
        {
            ViewData["TiposRecurso"] = tiposRecurso;
            return View();
        }

        [HttpPost]
        public IActionResult AddRecurso(Recurso recurso)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["TiposRecurso"] = tiposRecurso;
                    return View(recurso);
                }

                _context.Recursos.Add(recurso);
                _context.SaveChanges();
                return RedirectToAction("ListRecurso");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar el recurso.");
                ViewData["TiposRecurso"] = tiposRecurso;
                return View(recurso);
            }
        }

        public IActionResult UpdateRecurso(int id)
        {
            var recurso = _context.Recursos.Find(id);
            if (recurso == null)
            {
                return NotFound();
            }
            ViewData["TiposRecurso"] = tiposRecurso;
            return View(recurso);
        }

        [HttpPost]
        public IActionResult UpdateRecurso(Recurso recurso)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["TiposRecurso"] = tiposRecurso;
                    return View(recurso);
                }

                _context.Recursos.Update(recurso);
                _context.SaveChanges();
                return RedirectToAction("ListRecurso");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar el recurso.");
                ViewData["TiposRecurso"] = tiposRecurso;
                return View(recurso);
            }
        }

        [HttpPost]
        public IActionResult DeleteRecurso(int id)
        {
            var recurso = _context.Recursos.Find(id);
            if (recurso != null)
            {
                _context.Recursos.Remove(recurso);
                _context.SaveChanges();
            }
            return RedirectToAction("ListRecurso");
        }
    }
}
