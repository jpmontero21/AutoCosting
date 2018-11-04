using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Tracking;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.HelpersAndValidations;

namespace AutoCosting.Controllers.Tracking
{
    [Authorize (Roles = SD.AdminEndUser)]
    public class TrackingDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackingDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrackingDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TrackinDetails.Include(t => t.Parent).Include(t => t.Proveedor).Include(t => t.Trabajo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TrackingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackingDetail = await _context.TrackinDetails
                .Include(t => t.Parent)
                .Include(t => t.Proveedor)
                .Include(t => t.Trabajo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trackingDetail == null)
            {
                return NotFound();
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa");
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion");
            return View(trackingDetail);
        }

        // GET: TrackingDetails/Create
        public IActionResult Create(int trackingId)
        {
            TrackingDetail detail = new TrackingDetail()
            {
                Parent = this._context.TrackingHeaders.FirstOrDefault(h => h.TrackingID == trackingId),
                TrackingId = trackingId
            };
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa");
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion");
            return View(detail);
        }

        // POST: TrackingDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TrackingId,Costo,TrabajoId,ProveedorId,NumeroFactura,Descripcion,AddDescripcion")] TrackingDetail trackingDetail)
        {
            if (ModelState.IsValid)
            {
                TrackingHeader parent = await _context.TrackingHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.TrackingID == trackingDetail.TrackingId);
                Vehiculo vehiculo = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.VIN == parent.VINVehiculo);
                if (vehiculo != null)
                {
                    vehiculo.PrecioMinimo += trackingDetail.Costo;
                    vehiculo.PrecioRecomendado += trackingDetail.Costo;
                    _context.Vehiculos.Update(vehiculo);
                }

                _context.Add(trackingDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), "TrackingHeaders", new { @id = trackingDetail.TrackingId });
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa", trackingDetail.ProveedorId);
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion", trackingDetail.TrabajoId);
            return View(trackingDetail);
        }

        // GET: TrackingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackingDetail = await _context.TrackinDetails.FindAsync(id);
            if (trackingDetail == null)
            {
                return NotFound();
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa", trackingDetail.ProveedorId);
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion", trackingDetail.TrabajoId);
            return View(trackingDetail);
        }

        // POST: TrackingDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TrackingId,Costo,TrabajoId,ProveedorId,NumeroFactura,Descripcion,AddDescripcion")] TrackingDetail trackingDetail)
        {
            if (id != trackingDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TrackingDetail originalDetail = await _context.TrackinDetails.AsNoTracking().FirstOrDefaultAsync(d => d.ID == id);
                    if (originalDetail != null)
                    {
                        originalDetail.Parent = await _context.TrackingHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.TrackingID == originalDetail.TrackingId);
                        Vehiculo vehiculo = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.VIN == originalDetail.Parent.VINVehiculo);
                        if (vehiculo != null)
                        {
                            if (originalDetail.Costo > trackingDetail.Costo)
                            {
                                vehiculo.PrecioMinimo -= originalDetail.Costo - trackingDetail.Costo;
                                vehiculo.PrecioRecomendado -= originalDetail.Costo - trackingDetail.Costo;
                            }
                            else if (trackingDetail.Costo > originalDetail.Costo)
                            {
                                vehiculo.PrecioMinimo += trackingDetail.Costo - originalDetail.Costo;
                                vehiculo.PrecioRecomendado += trackingDetail.Costo - originalDetail.Costo;
                            }
                            //how to update this vehiculo ?
                            _context.Vehiculos.Update(vehiculo);
                        }
                    }

                    _context.Update(trackingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackingDetailExists(trackingDetail.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), "TrackingHeaders", new { @id = trackingDetail.TrackingId });
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa", trackingDetail.ProveedorId);
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion", trackingDetail.TrabajoId);
            return View(trackingDetail);
        }

        // GET: TrackingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackingDetail = await _context.TrackinDetails
                .Include(t => t.Parent)
                .Include(t => t.Proveedor)
                .Include(t => t.Trabajo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trackingDetail == null)
            {
                return NotFound();
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreEmpresa", trackingDetail.ProveedorId);
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion", trackingDetail.TrabajoId);
            return View(trackingDetail);
        }

        // POST: TrackingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int trackingID = 0;
            var trackingDetail = await _context.TrackinDetails.FindAsync(id);
            trackingID = trackingDetail.TrackingId;

            TrackingHeader parent = await _context.TrackingHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.TrackingID == trackingDetail.TrackingId);
            Vehiculo vehiculo = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.VIN == parent.VINVehiculo);
            if (vehiculo != null)
            {
                vehiculo.PrecioMinimo -= trackingDetail.Costo;
                vehiculo.PrecioRecomendado -= trackingDetail.Costo;
                _context.Vehiculos.Update(vehiculo);
            }

            _context.TrackinDetails.Remove(trackingDetail);
            await _context.SaveChangesAsync();
            if (trackingID != 0)
            {
                return RedirectToAction(nameof(Edit), "TrackingHeaders", new { @id = trackingID });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TrackingDetailExists(int id)
        {
            return _context.TrackinDetails.Any(e => e.ID == id);
        }

        [HttpPost]
        public string GetTrabajoInfo(string trabajoID )
        {
            var trabajo = this._context.Trabajos.FirstOrDefault(t => t.ID == Convert.ToInt32(trabajoID));
            if (trabajo != null)
            {
                return trabajo.PrecioPromedio.ToString();
            }
            return string.Empty;
        }
    }
}
