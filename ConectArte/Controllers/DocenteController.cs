using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class DocenteController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> especialidades = new List<string>
        {
            "Arte", "Música", "Teatro", "Tecnología", "Danza", "Literatura"
        };

        public DocenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddDocente()
        {
            ViewData["Especialidades"] = especialidades;
            return View();
        }

        [HttpPost]
        public IActionResult AddDocente(Docente docente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Especialidades"] = especialidades; 
                    return View(docente);
                }

                _context.Docentes.Add(docente);
                _context.SaveChanges();
                return RedirectToAction("ListDocente");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar el docente.");
                ViewData["Especialidades"] = especialidades;
                return View(docente);
            }
        }

        public IActionResult ListDocente()
        {
            List<Docente> docentes = _context.Docentes.ToList();
            return View(docentes);
        }

        [HttpPost]
        public IActionResult DeleteDocente(int id)
        {
            Docente docente = _context.Docentes.Find(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
                _context.SaveChanges();
            }
            return RedirectToAction("ListDocente");
        }

        public IActionResult UpdateDocente(int id)
        {
            Docente docente = _context.Docentes.Find(id);
            if (docente == null)
            {
                return NotFound();
            }
            ViewData["Especialidades"] = especialidades;
            return View(docente);
        }

        [HttpPost]
        public IActionResult UpdateDocente(Docente docente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Especialidades"] = especialidades;
                    return View(docente);
                }

                _context.Docentes.Update(docente);
                _context.SaveChanges();

                return RedirectToAction("ListDocente");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar el docente.");
                ViewData["Especialidades"] = especialidades;
                return View(docente);
            }
        }
    }
}
