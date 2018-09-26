using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Maintenance;

namespace AutoCosting.Controllers
{
    public class SedeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SedeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sede
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sede.Include(s => s.Empresa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sede/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sede = await _context.Sede
                .Include(s => s.Empresa)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sede == null)
            {
                return NotFound();
            }

            return View(sede);
        }

        // GET: Sede/Create
        public IActionResult Create()
        {
            if (_context.Empresa.Count() <= 0)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "Nombre");
            return View();
        }

        // POST: Sede/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EmpresaID,Nombre,Direccion,Telefono,UsarCierreCajaYN,ImprimirLogoYN")] Sede sede)
        {
            Empresa empresa = await _context.Empresa.FirstOrDefaultAsync();
            if (empresa == null)
            {
                return NotFound();
            }
            else
            {
                sede.EmpresaID = empresa.ID;
            }
            if (ModelState.IsValid)
            {
                _context.Add(sede);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "Nombre", sede.EmpresaID);
            return View(sede);
        }

        // GET: Sede/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Empresa empresa = await _context.Empresa.FirstOrDefaultAsync();
            if (empresa == null)
            {
                return NotFound();
            }

            var sede = await _context.Sede.FindAsync(new object[] { id, empresa.ID });
            if (sede == null)
            {
                return NotFound();
            }
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "Nombre", sede.EmpresaID);
            return View(sede);
        }

        // POST: Sede/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EmpresaID,Nombre,Direccion,Telefono,UsarCierreCajaYN,ImprimirLogoYN")] Sede sede)
        {
            if (id != sede.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sede);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SedeExists(sede.ID))
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
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "Nombre", sede.EmpresaID);
            return View(sede);
        }

        // GET: Sede/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sede = await _context.Sede
                .Include(s => s.Empresa)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sede == null)
            {
                return NotFound();
            }

            return View(sede);
        }

        // POST: Sede/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Empresa empresa = await _context.Empresa.FirstOrDefaultAsync();
            if (empresa == null)
            {
                return NotFound();
            }
            var sede = await _context.Sede.FindAsync(new object[] { id, empresa.ID });
            _context.Sede.Remove(sede);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SedeExists(int id)
        {
            return _context.Sede.Any(e => e.ID == id);
        }
    }
}
