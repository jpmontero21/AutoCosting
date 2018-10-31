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
        public async Task<IActionResult> Index()
        {
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
        public async Task<IActionResult> Create([Bind("ID,Descripcion,PrecioPromedio")] Trabajo trabajo)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Descripcion,PrecioPromedio")] Trabajo trabajo)
        {
            if (id != trabajo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            var trabajo = await _context.Trabajos.FindAsync(id);
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
