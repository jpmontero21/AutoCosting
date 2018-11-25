using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.Tracking;

namespace AutoCosting.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class TrabajoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrabajoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trabajo
        public async Task<IActionResult> Index(string search = null)
        {
            if (search != null)
            {
                return View(await _context.Trabajos.Where(t=>t.Descripcion.ToLower().Contains(search)).ToListAsync());
            }
            return View(await _context.Trabajos.ToListAsync());
        }

        // GET: Trabajo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajo = await _context.Trabajos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trabajo == null)
            {
                return NotFound();
            }

            return View(trabajo);
        }

        // GET: Trabajo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trabajo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Trabajo trabajo)
        {
            if (ModelState.IsValid)
            {
                Trabajo trab = await this._context.Trabajos.FirstOrDefaultAsync(t => t.Descripcion == trabajo.Descripcion);
                if (trab != null)
                {
                    ModelState.AddModelError("Descripcion", "Ya existe un trabajo con esta descripción");
                    return View(trabajo);
                }
                _context.Add(trabajo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trabajo);
        }

        // GET: Trabajo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajo = await _context.Trabajos.FindAsync(id);
            if (trabajo == null)
            {
                return NotFound();
            }
            return View(trabajo);
        }

        // POST: Trabajo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trabajo trabajo)
        {
            if (id != trabajo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Trabajo trab = await this._context.Trabajos.FirstOrDefaultAsync(t => t.Descripcion == trabajo.Descripcion && t.ID != trabajo.ID);
                    if (trab != null)
                    {
                        ModelState.AddModelError("Descripcion", "Ya existe un trabajo con esta descripción");
                        return View(trabajo);
                    }
                    _context.Update(trabajo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrabajoExists(trabajo.ID))
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
            return View(trabajo);
        }

        // GET: Trabajo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajo = await _context.Trabajos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trabajo == null)
            {
                return NotFound();
            }

            return View(trabajo);
        }

        // POST: Trabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TrackingDetail track = this._context.TrackinDetails.FirstOrDefault(t => t.TrabajoId == id);
            
            var trabajo = await _context.Trabajos.FindAsync(id);
            if (track != null)
            {
                ModelState.AddModelError("ID", "No se puede eliminar, este trabajo está en uso.");
                return View(trabajo);
            }
            _context.Trabajos.Remove(trabajo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrabajoExists(int id)
        {
            return _context.Trabajos.Any(e => e.ID == id);
        }
    }
}
