using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Transaction;
using AutoCosting.HelpersAndValidations;

namespace AutoCosting.Controllers.Transaccion
{
    public class ComisionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComisionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comision
        public IActionResult Index(string option=null,string search=null)
        {
            var comisions = _context.Comisiones.Include(c => c.Parent).ToList();
            if (option == "Tipo" && search != null)
            {
                comisions = _context.Comisiones.Where(c => c.TipoComision.ToString().ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "Nombre" && search != null)
            {
                comisions = _context.Comisiones.Where(c => c.Nombre.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(comisions);
        }

        // GET: Comision/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comision = await _context.Comisiones
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comision == null)
            {
                return NotFound();
            }
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            return View(comision);
        }

        // GET: Comision/Create
        public IActionResult Create(TipoComision commType)
        {
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            Comision comision = new Comision()
            {
                TipoComision = commType
            };
            return View(comision);
        }

        // POST: Comision/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Comision comision)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comision);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            return View(comision);
        }

        // GET: Comision/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comision = await _context.Comisiones.FindAsync(id);
            if (comision == null)
            {
                return NotFound();
            }
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            return View(comision);
        }

        // POST: Comision/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Comision comision)
        {
            if (id != comision.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comision);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComisionExists(comision.ID))
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
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            return View(comision);
        }

        // GET: Comision/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comision = await _context.Comisiones
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comision == null)
            {
                return NotFound();
            }
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "NombreCompleto", "NombreCompleto");
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransIdStr");
            return View(comision);
        }

        // POST: Comision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comision = await _context.Comisiones.FindAsync(id);
            _context.Comisiones.Remove(comision);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComisionExists(int id)
        {
            return _context.Comisiones.Any(e => e.ID == id);
        }
    }
}
