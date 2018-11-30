using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Tracking;
using AutoCosting.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.HelpersAndValidations;

namespace AutoCosting.Controllers.Tracking
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class TrackingHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackingHeadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrackingHeaders
        public IActionResult Index(string option = null,string search=null)
        {
            var trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(d=>d.TrackingDetails).Where(h=> !h.Vehiculo.VendidoYN).ToList();
            if (option == "Vehiculo" && search != null)
            {
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => !h.Vehiculo.VendidoYN && h.Vehiculo.Descripcion.ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("dd/MM/yyyy");
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => !h.Vehiculo.VendidoYN && h.FechaStr.ToLower().Contains(search.Replace("-", "/").ToLower())).ToList();
            }
            if (option == "TrackingID" && search != null)
            {
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => !h.Vehiculo.VendidoYN && h.TrackingIdStr.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(trackings);
        }

        // GET: TrackingHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackingHeader = await _context.TrackingHeaders
                .Include(t => t.Vehiculo)
                .FirstOrDefaultAsync(m => m.TrackingID == id);
            if (trackingHeader == null)
            {
                return NotFound();
            }
            List<TrackingDetail> list = this._context.TrackinDetails.Where(d => d.TrackingId == id).ToList();
            list.ForEach(d =>
            {
                d.Proveedor = this._context.Proveedores.FirstOrDefault(p => p.ID == d.ProveedorId);
                d.Trabajo = this._context.Trabajos.FirstOrDefault(t => t.ID == d.TrabajoId);
            });
            TrackingViewModel tracking = new TrackingViewModel()
            {
                Tracking = trackingHeader,
                TrackingDetalles = list
            };
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion", trackingHeader.VINVehiculo);
            return View(tracking);
        }

        // GET: TrackingHeaders/Create
        public IActionResult Create()
        {
            TrackingHeader header = new TrackingHeader();
            header.Fecha = DateTime.Today;
            ;
            ViewData["VINVehiculo"] = new SelectList(this._context.Vehiculos.Where(v => !this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN)&&!v.VendidoYN && !v.ApartadoYN), "VIN", "Descripcion");
            return View(header);
        }

        // POST: TrackingHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TrackingHeader trackingHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trackingHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { ID = trackingHeader.TrackingID });
            }
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "VIN", trackingHeader.VINVehiculo);
            return View(trackingHeader);
        }

        // GET: TrackingHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var trackingHeader = await _context.TrackingHeaders.Include(t=> t.Vehiculo).FirstOrDefaultAsync(t=>t.TrackingID==id);
            if (trackingHeader == null)
            {
                return NotFound();
            }
            List<TrackingDetail> list = this._context.TrackinDetails.Where(d => d.TrackingId == id).ToList();
            list.ForEach(d => 
            {
                d.Proveedor = this._context.Proveedores.FirstOrDefault(p => p.ID == d.ProveedorId);
                d.Trabajo = this._context.Trabajos.FirstOrDefault(t => t.ID == d.TrabajoId);
            });
            TrackingViewModel tracking = new TrackingViewModel()
            {
                Tracking = trackingHeader,
                TrackingDetalles = list
            };
            
            //ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion", trackingHeader.VINVehiculo);//Where(v => !this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN)&&!v.VendidoYN && !v.ApartadoYN)
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v => (!this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN) && !v.VendidoYN && !v.ApartadoYN)||v.VIN==trackingHeader.VINVehiculo), "VIN", "Descripcion", trackingHeader.VINVehiculo);//
            if (trackingHeader.Vehiculo.VendidoYN)
            {
                return View(nameof(Details),tracking);
            }
            return View(tracking);
        }

        // POST: TrackingHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrackingViewModel trackingHeader)
        {
            //ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion", trackingHeader.Tracking.VINVehiculo);
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos.Where(v => (!this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN) && !v.VendidoYN && !v.ApartadoYN) || v.VIN == trackingHeader.Tracking.VINVehiculo), "VIN", "Descripcion", trackingHeader.Tracking.VINVehiculo);//
            if (id != trackingHeader.Tracking.TrackingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trackingHeader.Tracking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackingHeaderExists(trackingHeader.Tracking.TrackingID))
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
            return View(trackingHeader);
        }

        // GET: TrackingHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackingHeader = await _context.TrackingHeaders
                .Include(t => t.Vehiculo)
                .FirstOrDefaultAsync(m => m.TrackingID == id);
            if (trackingHeader == null)
            {
                return NotFound();
            }
            List<TrackingDetail> list = this._context.TrackinDetails.Where(d => d.TrackingId == id).ToList();
            list.ForEach(d =>
            {
                d.Proveedor = this._context.Proveedores.FirstOrDefault(p => p.ID == d.ProveedorId);
                d.Trabajo = this._context.Trabajos.FirstOrDefault(t => t.ID == d.TrabajoId);
            });
            TrackingViewModel tracking = new TrackingViewModel()
            {
                Tracking = trackingHeader,
                TrackingDetalles = list
            };
            //ViewData["VINVehiculo"] = new SelectList(this._context.Vehiculos.Where(v => !this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN)), "VIN", "Descripcion");            
            ViewData["VINVehiculo"] = new SelectList(this._context.Vehiculos.Where(v => (!this._context.TrackingHeaders.Any(t => t.VINVehiculo == v.VIN) && !v.VendidoYN && !v.ApartadoYN) || v.VIN == trackingHeader.VINVehiculo), "VIN", "Descripcion", trackingHeader.VINVehiculo);
            return View(tracking);
        }

        // POST: TrackingHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trackingHeader = await _context.TrackingHeaders.FindAsync(id);
            _context.TrackingHeaders.Remove(trackingHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackingHeaderExists(int id)
        {
            return _context.TrackingHeaders.Any(e => e.TrackingID == id);
        }

        public IActionResult SoldTrackings(string option = null, string search = null)
        {
            var trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(d => d.TrackingDetails).Where(t=>t.Vehiculo.VendidoYN).ToList();
            if (option == "Vehiculo" && search != null)
            {
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => h.Vehiculo.VendidoYN && h.Vehiculo.Descripcion.ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("dd/MM/yyyy");
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => h.Vehiculo.VendidoYN && h.FechaStr.ToLower().Contains(search.Replace("-", "/").ToLower())).ToList();
            }
            if (option == "TrackingID" && search != null)
            {
                trackings = _context.TrackingHeaders.Include(t => t.Vehiculo).Include(t => t.TrackingDetails).Where(h => h.Vehiculo.VendidoYN && h.TrackingIdStr.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(trackings);
        }        
    }
}
