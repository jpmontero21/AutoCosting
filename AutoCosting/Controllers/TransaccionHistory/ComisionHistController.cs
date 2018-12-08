using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.TransactionHist;

namespace AutoCosting.Controllers.TransaccionHistory
{
    public class ComisionHistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComisionHistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComisionHist
        public async Task<IActionResult> Index(string option = null, string search = null)
        {
            var comisions = await _context.ComisionHistory.Include(c => c.Parent).ToListAsync();
            if (option == "Tipo" && search != null)
            {
                comisions = await _context.ComisionHistory.Where(c => c.TipoComision.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Nombre" && search != null)
            {
                comisions = await _context.ComisionHistory.Where(c => c.Nombre.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            return View(comisions);
        }

        // GET: ComisionHist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comisionHist = await _context.ComisionHistory
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comisionHist == null)
            {
                return NotFound();
            }

            return View(comisionHist);
        }

        // GET: ComisionHist/Create
        public IActionResult Create()
        {
            ViewData["TransID"] = new SelectList(_context.TransHistoryHeader, "TransID", "VendedorID");
            return View();
        }

        // POST: ComisionHist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComisionHist comisionHist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comisionHist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransID"] = new SelectList(_context.TransHistoryHeader, "TransID", "VendedorID", comisionHist.TransID);
            return View(comisionHist);
        }

        // GET: ComisionHist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comisionHist = await _context.ComisionHistory.FindAsync(id);
            if (comisionHist == null)
            {
                return NotFound();
            }
            ViewData["TransID"] = new SelectList(_context.TransHistoryHeader, "TransID", "VendedorID", comisionHist.TransID);
            return View(comisionHist);
        }

        // POST: ComisionHist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComisionHist comisionHist)
        {
            if (id != comisionHist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comisionHist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComisionHistExists(comisionHist.ID))
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
            ViewData["TransID"] = new SelectList(_context.TransHistoryHeader, "TransID", "VendedorID", comisionHist.TransID);
            return View(comisionHist);
        }

        // GET: ComisionHist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comisionHist = await _context.ComisionHistory
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (comisionHist == null)
            {
                return NotFound();
            }

            return View(comisionHist);
        }

        // POST: ComisionHist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comisionHist = await _context.ComisionHistory.FindAsync(id);
            _context.ComisionHistory.Remove(comisionHist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComisionHistExists(int id)
        {
            return _context.ComisionHistory.Any(e => e.ID == id);
        }
    }
}
