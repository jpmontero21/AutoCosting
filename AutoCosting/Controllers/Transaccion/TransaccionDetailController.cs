using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Transaction;

namespace AutoCosting.Controllers.Transaccion
{
    public class TransaccionDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaccionDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransaccionDetail
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransaccionDetails.Include(t => t.Vehiculo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransaccionDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionDetail = await _context.TransaccionDetails
                .Include(t => t.Vehiculo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaccionDetail == null)
            {
                return NotFound();
            }

            return View(transaccionDetail);
        }

        // GET: TransaccionDetail/Create
        public IActionResult Create()
        {
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "VIN");
            return View();
        }

        // POST: TransaccionDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TransID,VINVehiculo,PrecioAcordado")] TransaccionDetail transaccionDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "VIN", transaccionDetail.VINVehiculo);
            return View(transaccionDetail);
        }

        // GET: TransaccionDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionDetail = await _context.TransaccionDetails.FindAsync(id);
            if (transaccionDetail == null)
            {
                return NotFound();
            }
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "VIN", transaccionDetail.VINVehiculo);
            return View(transaccionDetail);
        }

        // POST: TransaccionDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TransID,VINVehiculo,PrecioAcordado")] TransaccionDetail transaccionDetail)
        {
            if (id != transaccionDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccionDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccionDetailExists(transaccionDetail.ID))
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
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "VIN", transaccionDetail.VINVehiculo);
            return View(transaccionDetail);
        }

        // GET: TransaccionDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionDetail = await _context.TransaccionDetails
                .Include(t => t.Vehiculo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaccionDetail == null)
            {
                return NotFound();
            }

            return View(transaccionDetail);
        }

        // POST: TransaccionDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccionDetail = await _context.TransaccionDetails.FindAsync(id);
            _context.TransaccionDetails.Remove(transaccionDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionDetailExists(int id)
        {
            return _context.TransaccionDetails.Any(e => e.ID == id);
        }
    }
}
