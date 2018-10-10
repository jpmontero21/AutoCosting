using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Tracking;

namespace AutoCosting.Controllers.Tracking
{
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

            return View(trackingDetail);
        }

        // GET: TrackingDetails/Create
        public IActionResult Create()
        {
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreContacto");
            ViewData["TrabajoId"] = new SelectList(_context.Trabajos, "ID", "Descripcion");
            return View();
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
                _context.Add(trackingDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreContacto", trackingDetail.ProveedorId);
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
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreContacto", trackingDetail.ProveedorId);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrackingId"] = new SelectList(_context.TrackingHeaders, "TrackingID", "TrackingID", trackingDetail.TrackingId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "ID", "NombreContacto", trackingDetail.ProveedorId);
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

            return View(trackingDetail);
        }

        // POST: TrackingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trackingDetail = await _context.TrackinDetails.FindAsync(id);
            _context.TrackinDetails.Remove(trackingDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackingDetailExists(int id)
        {
            return _context.TrackinDetails.Any(e => e.ID == id);
        }
    }
}
