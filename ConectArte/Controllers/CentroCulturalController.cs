using ConectArte.Datos;
using ConectArte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectArte.Controllers
{
    public class CentroCulturalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CentroCulturalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddCentroCultural()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCentroCultural(CentroCultural c)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(c);
                }

                _context.CentrosCulturales.Add(c);
                _context.SaveChanges();
                return RedirectToAction("ListCentroCultural");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al guardar el centro cultural.");
                return View(c);
            }
        }

        public IActionResult ListCentroCultural()
        {
            List<CentroCultural> centros = _context.CentrosCulturales.ToList();
            return View(centros);
        }

        public IActionResult UpdateCentroCultural(int id)
        {
            var centro = _context.CentrosCulturales.Find(id);
            if (centro == null)
            {
                return NotFound();
            }
            return View(centro);
        }

        [HttpPost]
        public IActionResult UpdateCentroCultural(CentroCultural centro)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(centro);
                }

                _context.CentrosCulturales.Update(centro);
                _context.SaveChanges();
                return RedirectToAction("ListCentroCultural");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error interno al actualizar el centro cultural.");
                return View(centro);
            }
        }

        [HttpPost]
        public IActionResult DeleteCentroCultural(int id)
        {
            var centro = _context.CentrosCulturales.Find(id);
            if (centro != null)
            {
                _context.CentrosCulturales.Remove(centro);
                _context.SaveChanges();
            }

            return RedirectToAction("ListCentroCultural");
        }

        public IActionResult DetailsCentroCultural(int id)
        {
            var centro = _context.CentrosCulturales
                                 .Include(c => c.Salas)
                                 .Include(c => c.Asistentes)
                                 .FirstOrDefault(c => c.Id == id);

            if (centro == null)
            {
                return NotFound();
            }

            return View(centro);
        }
    }
}
