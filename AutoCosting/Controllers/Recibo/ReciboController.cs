using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using R = AutoCosting.Models.Receipts;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.CierreAperturaCaja;

namespace AutoCosting.Controllers.Recibo
{
    [Authorize]
    public class ReciboController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReciboController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recibo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recibos.Include(r => r.Parent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recibo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recibo = await _context.Recibos
                .Include(r => r.Parent)
                .Include(r =>r.Parent.Sede )
                .Include(r => r.Parent.TransDetails)
                .Include(r => r.Parent.Cliente)
                .Include(r => r.Parent.Empleado)
                .Include(r => r.Parent.Sede.Empresa)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (recibo == null)
            {
                return NotFound();
            }

            return View(recibo);
        }

        // GET: Recibo/Create
        public async Task<IActionResult> Create(int transId)
        {
            R.Recibo recibo = new R.Recibo();
            recibo.TransID = transId;
            var transaccion = this._context.TransaccionHeaders.FirstOrDefault(t=>t.TransID == transId);
            if (transaccion == null)
            {
                return NotFound();
            }
            recibo.Parent = transaccion;
            recibo.Fecha = DateTime.Today;

            var caja = await this._context.AperturaCierreCaja.FirstOrDefaultAsync(c => c.Tipo == TipoCaja.Cierre && c.Fecha.Value.ToShortDateString() == DateTime.Today.ToShortDateString());
            if (caja != null)
            {
                this.ViewData["CajaMessage"] = "Ya cerró caja el día de hoy, si registra un nuevo recibo se actualizará el cierre.";
            }
            return View(recibo);
        }

        // POST: Recibo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(R.Recibo recibo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recibo);
                await _context.SaveChangesAsync();
                //Actualizar Caja
                Caja caja = await this._context.AperturaCierreCaja.AsNoTracking().FirstOrDefaultAsync(c => c.Tipo == TipoCaja.Cierre && c.Fecha.Value.ToShortDateString() == DateTime.Today.ToShortDateString());
                if (caja != null)
                {
                    caja.Monto += recibo.Abono;
                    _context.AperturaCierreCaja.Update(caja);
                    await _context.SaveChangesAsync();
                }
                

                return RedirectToAction(nameof(Edit), "TransaccionHeader", new { @id = recibo.TransID });
            }            
            return View(recibo);
        }

        // GET: Recibo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recibo = await _context.Recibos.FindAsync(id);
            if (recibo == null)
            {
                return NotFound();
            }
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransID", recibo.TransID);
            return View(recibo);
        }

        // POST: Recibo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, R.Recibo recibo)
        {
            if (id != recibo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recibo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReciboExists(recibo.ID))
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
            ViewData["TransID"] = new SelectList(_context.TransaccionHeaders, "TransID", "TransID", recibo.TransID);
            return View(recibo);
        }

        //[Authorize(Roles = "Dev")]
        // GET: Recibo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recibo = await _context.Recibos
                .Include(r => r.Parent)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (recibo == null)
            {
                return NotFound();
            }

            return View(recibo);
        }

        // POST: Recibo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recibo = await _context.Recibos.FindAsync(id);
            _context.Recibos.Remove(recibo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReciboExists(int id)
        {
            return _context.Recibos.Any(e => e.ID == id);
        }
    }
}
