using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.FacturacionElectronica;

namespace AutoCosting.Controllers
{
    public class EmisorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmisorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emisors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Emisor.ToListAsync());
        }

        // GET: Emisors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emisor = await _context.Emisor
                .FirstOrDefaultAsync(m => m.ID == id);
            if (emisor == null)
            {
                return NotFound();
            }

            return View(emisor);
        }

        // GET: Emisors/Create
        public IActionResult Create()
        {
            ViewData["Cantones"] = new SelectList(_context.CodificacionMH.Where(c => c.idProvincia == "1").Select(i => new { i.idCanton, i.nombreCanton }).Distinct(), "idCanton", "nombreCanton");
            ViewData["Distritos"] = new SelectList(_context.CodificacionMH.Where(c => c.idProvincia == "1" && c.idCanton == "00").Select(i => new { i.idDistrito, i.nombreDistrito }).Distinct(), "idDistrito", "nombreDistrito");
            ViewData["Barrios"] = new SelectList(_context.CodificacionMH.Where(c => c.idProvincia == "1" && c.idCanton == "00" && c.idDistrito == "00" ).Select(i => new { i.idBarrio, i.nombreBarrio }).Distinct(), "idBarrio", "nombreBarrio");
            return View();
        }

        // POST: Emisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Emisor emisor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emisor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emisor);
        }

        // GET: Emisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emisor = await _context.Emisor.FindAsync(id);
            if (emisor == null)
            {
                return NotFound();
            }
            return View(emisor);
        }

        // POST: Emisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Emisor emisor)
        {
            if (id != emisor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmisorExists(emisor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(emisor);
        }

        // GET: Emisors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emisor = await _context.Emisor
                .FirstOrDefaultAsync(m => m.ID == id);
            if (emisor == null)
            {
                return NotFound();
            }

            return View(emisor);
        }

        // POST: Emisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emisor = await _context.Emisor.FindAsync(id);
            _context.Emisor.Remove(emisor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmisorExists(int id)
        {
            return _context.Emisor.Any(e => e.ID == id);
        }

        [HttpPost]
        public ActionResult LoadCanton(string provinciaId)
        {
            var list = _context.CodificacionMH.Where(c => c.idProvincia == provinciaId).Select(i => new { i.idCanton, i.nombreCanton }).Distinct();
            ViewData["Cantones"] = list;
            return Json(list.ToList());
        }

        [HttpPost]
        public ActionResult LoadDistritos(string provinciaId, string cantonId)
        {
            var list = _context.CodificacionMH.Where(c => c.idProvincia == provinciaId && c.idCanton == cantonId).Select(i => new { i.idDistrito, i.nombreDistrito }).Distinct();
            ViewData["Distritos"] = list;
            return Json(list.ToList());
        }
        [HttpPost]
        public ActionResult LoadBarrios(string provinciaId, string cantonId, string distritoId)
        {
            var list = _context.CodificacionMH.Where(c => c.idProvincia == provinciaId && c.idCanton == cantonId && c.idDistrito == distritoId).Select(i => new { i.idBarrio, i.nombreBarrio}).Distinct();
            ViewData["Distritos"] = list;
            return Json(list.ToList());
        }
    }
}
