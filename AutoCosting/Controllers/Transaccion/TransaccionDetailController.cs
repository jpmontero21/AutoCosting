using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Transaction;
using System.ComponentModel.DataAnnotations;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.Maintenance;

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
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion");
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
            var vehiculo = await _context.Vehiculos.FindAsync(transaccionDetail.VINVehiculo);
            if (vehiculo != null)
            {
                transaccionDetail.PrecioMinimo = vehiculo.PrecioMinimo;
                transaccionDetail.PrecioRecomendado = vehiculo.PrecioRecomendado;
            }

            return View(transaccionDetail);
        }

        // GET: TransaccionDetail/Create
        public IActionResult Create(int transID)
        {
            TransaccionDetail detail = new TransaccionDetail()
            {
                Parent = this._context.TransaccionHeaders.FirstOrDefault(h => h.TransID == transID),
                TransID = transID
            };
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN), "VIN", "Descripcion");
            return View(detail);
        }

        // POST: TransaccionDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransaccionDetail transaccionDetail)
        {
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v=>(!v.ApartadoYN && !v.VendidoYN) || v.VIN == transaccionDetail.VINVehiculo), "VIN", "Descripcion", transaccionDetail.VINVehiculo);
            if (ModelState.IsValid)
            {
                transaccionDetail.Parent = await this._context.TransaccionHeaders.FirstOrDefaultAsync(t => t.TransID == transaccionDetail.TransID);
                if (transaccionDetail.Parent == null)
                {
                    ModelState.AddModelError("VINVehiculo", "Error con la transacción.");
                    return View(transaccionDetail);
                }
                if (transaccionDetail.PrecioAcordado < transaccionDetail.PrecioMinimo)
                {
                    ModelState.AddModelError("PrecioAcordado", "El precio acordado no puede ser menor que el precio mínimo.");
                    return View(transaccionDetail);
                }
                Vehiculo vehiculo = await this._context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.VIN == transaccionDetail.VINVehiculo);
                if (vehiculo != null)
                {
                    if (vehiculo.VendidoYN || vehiculo.ApartadoYN)
                    {
                        ModelState.AddModelError("VINVehiculo", "El vehículo se encuentra apartado o ya está vendido.");
                        return View(transaccionDetail);
                    }

                    if (transaccionDetail.Parent.TipoTransaccion == TipoTransaccion.Venta)
                        vehiculo.VendidoYN = true;
                    else if (transaccionDetail.Parent.TipoTransaccion == TipoTransaccion.Apartado)
                        vehiculo.ApartadoYN = true;

                    this._context.Vehiculos.Update(vehiculo);
                    _context.Add(transaccionDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Edit), "TransaccionHeader", new { @id = transaccionDetail.TransID });
                }
                ModelState.AddModelError("VINVehiculo", "Error al encontrar el vehículo.");
                return View(transaccionDetail);
            }            
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
            var vehiculo = await _context.Vehiculos.FindAsync(transaccionDetail.VINVehiculo);
            if (vehiculo != null)
            {
                transaccionDetail.PrecioMinimo = vehiculo.PrecioMinimo;
                transaccionDetail.PrecioRecomendado = vehiculo.PrecioRecomendado;
            }
            //ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion", transaccionDetail.VINVehiculo);//_context.Vehiculos.Where(v=>(!v.ApartadoYN && !v.VendidoYN) || v.VIN == transaccionDetail.VINVehiculo)
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v => (!v.ApartadoYN && !v.VendidoYN) || v.VIN == transaccionDetail.VINVehiculo), "VIN", "Descripcion", transaccionDetail.VINVehiculo);
            return View(transaccionDetail);
        }

        // POST: TransaccionDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransaccionDetail transaccionDetail)
        {
            //ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion", transaccionDetail.VINVehiculo);
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v => (!v.ApartadoYN && !v.VendidoYN) || v.VIN == transaccionDetail.VINVehiculo), "VIN", "Descripcion", transaccionDetail.VINVehiculo);
            if (id != transaccionDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (transaccionDetail.PrecioAcordado < transaccionDetail.PrecioMinimo)
                    {
                        ModelState.AddModelError("PrecioAcordado", "El precio acordado no puede ser menor que el precio mínimo.");
                        return View(transaccionDetail);
                    }
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
                return RedirectToAction(nameof(Edit), "TransaccionHeader", new { @id = transaccionDetail.TransID });
                //return RedirectToAction(nameof(Index));
            }
            
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
            var vehiculo = await _context.Vehiculos.FindAsync(transaccionDetail.VINVehiculo);
            if (vehiculo != null)
            {
                transaccionDetail.PrecioMinimo = vehiculo.PrecioMinimo;
                transaccionDetail.PrecioRecomendado = vehiculo.PrecioRecomendado;
                transaccionDetail.Vehiculo = vehiculo;
            }

            return View(transaccionDetail);
        }

        // POST: TransaccionDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccionDetail = await _context.TransaccionDetails.FindAsync(id);

            /**/
            transaccionDetail.Parent = await this._context.TransaccionHeaders.FirstOrDefaultAsync(t => t.TransID == transaccionDetail.TransID);
            if (transaccionDetail.Parent == null)
            {
                ModelState.AddModelError("VINVehiculo", "Error con la transacción.");
                return View(transaccionDetail);
            }
            Vehiculo vehiculo = await this._context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.VIN == transaccionDetail.VINVehiculo);
            if (vehiculo != null)
            {
                //if (vehiculo.VendidoYN || vehiculo.ApartadoYN)
                //{
                //    ModelState.AddModelError("VINVehiculo", "El vehículo se encuentra apartado o ya está vendido.");
                //    return View(transaccionDetail);
                //}
                if (transaccionDetail.Parent.TipoTransaccion == TipoTransaccion.Venta)
                    vehiculo.VendidoYN = false;
                else if (transaccionDetail.Parent.TipoTransaccion == TipoTransaccion.Apartado)
                    vehiculo.ApartadoYN = false;

                this._context.Vehiculos.Update(vehiculo);
                this._context.TransaccionDetails.Remove(transaccionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), "TransaccionHeader", new { @id = transaccionDetail.TransID });
                /**/
            }
            
            ModelState.AddModelError("VINVehiculo", "Error al encontrar el vehículo.");
            return View(transaccionDetail.ID);
        }

        private bool TransaccionDetailExists(int id)
        {
            return _context.TransaccionDetails.Any(e => e.ID == id);
        }

        [HttpPost]
        public string GetMinPrice(string VIN)
        {
            var vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == VIN);
            if (vehiculo != null)
            {
                return vehiculo.PrecioMinimo.ToString();
            }
            return string.Empty;
        }
        [HttpPost]
        public string GetRecommendedPrice(string VIN)
        {
            var vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == VIN);
            if (vehiculo != null)
            {
                return vehiculo.PrecioRecomendado.ToString();
            }
            return string.Empty;
        }
                
    }
}
