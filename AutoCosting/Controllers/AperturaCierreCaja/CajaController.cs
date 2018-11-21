using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.CierreAperturaCaja;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.ViewModel;

namespace AutoCosting.Controllers.AperturaCierreCaja
{
    public class CajaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CajaController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Caja
        public async Task<IActionResult> Index()
        {
            return View(await _context.AperturaCierreCaja.OrderByDescending(c => c.Fecha).ToListAsync());
        }

        //GET: Caja/Close
        public IActionResult Close()
        {
            Caja dupl = this._context.AperturaCierreCaja.FirstOrDefault(c => c.Tipo == TipoCaja.Cierre && c.Fecha.Value.ToShortDateString() == DateTime.Today.ToShortDateString());
            if (dupl != null)
            {
                ViewData["CerrarMessage"] = "Ya se realizó el cierre de caja el dia de hoy.";
                return View();
            }

            if (!this.IsCajaOpen(DateTime.Today))
            {
                ViewData["CerrarMessage"] = "No se ha abierto caja aun el día de hoy.";
                return View();
            }

            Claim claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Caja apertura = this._context.AperturaCierreCaja.OrderByDescending(c => c.Fecha).FirstOrDefault(c => c.Tipo == TipoCaja.Apertura);
            Caja caja = new Caja()
            {
                Fecha = DateTime.Today,
                Usuario = (claim != null && claim.Subject != null) ? claim.Subject.Name : string.Empty,
                Tipo = TipoCaja.Cierre,
                Monto = this.GetEntradasDeDinero(DateTime.Today) + apertura.Monto,
                Observaciones = $"Cierre de caja, final de día {DateTime.Today.ToShortDateString()}."
            };
            return View(caja);
        }

        public bool IsCajaOpen(DateTime date)
        {
            var caja = this._context.AperturaCierreCaja.FirstOrDefault(c => c.Fecha.Value.ToShortDateString() == date.ToShortDateString() && c.Tipo == TipoCaja.Apertura);
            return caja != null;
        }

        // GET: Caja/Create
        public IActionResult Create()
        {
            Caja dupl = this._context.AperturaCierreCaja.FirstOrDefault(c => c.Tipo == TipoCaja.Apertura && c.Fecha.Value.ToShortDateString() == DateTime.Today.ToShortDateString());
            if (dupl != null)
            {
                ViewData["AbrirMessage"] = "Ya se realizó la apertura de caja el dia de hoy.";
                return View();
            }

            Claim claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Caja anterior = this._context.AperturaCierreCaja.OrderByDescending(c => c.Fecha).FirstOrDefault(c => c.Tipo == TipoCaja.Cierre);
            Caja caja = new Caja()
            {
                Fecha = DateTime.Today,
                Usuario = (claim != null && claim.Subject != null) ? claim.Subject.Name : string.Empty,
                Tipo = TipoCaja.Apertura,
                Monto = (anterior != null) ? anterior.Monto : 0
            };
            return View(caja);
        }

        //GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Caja cierre = await this._context.AperturaCierreCaja.FirstOrDefaultAsync(c => c.ID == id);
            if (cierre == null || this._context.Empresa.Count() == 0)
            {
                return NotFound();
            }
            Caja apertura = await this._context.AperturaCierreCaja.FirstOrDefaultAsync(c => c.Fecha == cierre.Fecha && c.Tipo == TipoCaja.Apertura);
            if (apertura == null)
            {
                return NotFound();
            }
            CierreDeCajaViewModel viewModel = new CierreDeCajaViewModel()
            {
                CierreCaja = cierre,
                AperturaCaja = apertura,
                Empresa = await this._context.Empresa.FirstOrDefaultAsync(),
                Recibos = await this._context.Recibos.Where(r => r.Fecha.Value.ToShortDateString() == cierre.Fecha.Value.ToShortDateString()).Include(t => t.Parent).Include(x=>x.Parent.Cliente).ToListAsync()
            };
            return View(viewModel);
        }

        //GET: Summary
        public async Task<IActionResult> Summary(int? id)
        {            
            if (id == null)
            {
                return NotFound();
            }
            Caja cierre = await this._context.AperturaCierreCaja.FirstOrDefaultAsync(c=>c.ID == id);            
            if (cierre == null || this._context.Empresa.Count() == 0)
            {
                return NotFound();
            }
            Caja apertura = await this._context.AperturaCierreCaja.FirstOrDefaultAsync(c => c.Fecha == cierre.Fecha && c.Tipo == TipoCaja.Apertura);
            if (apertura == null)
            {
                return NotFound();
            }
            CierreDeCajaViewModel viewModel = new CierreDeCajaViewModel()
            {
                CierreCaja = cierre,
                AperturaCaja = apertura,
                Empresa = await this._context.Empresa.FirstOrDefaultAsync(),
                Recibos = await this._context.Recibos.Where(r => r.Fecha.Value.ToShortDateString() == cierre.Fecha.Value.ToShortDateString()).Include(t => t.Parent).ToListAsync()                
            };
            return View(viewModel);
        }

        // POST: Caja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Caja caja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caja);
        }
        
        // GET: Caja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caja = await _context.AperturaCierreCaja
                .FirstOrDefaultAsync(m => m.ID == id);
            if (caja == null)
            {
                return NotFound();
            }

            return View(caja);
        }

        // POST: Caja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caja = await _context.AperturaCierreCaja.FindAsync(id);
            _context.AperturaCierreCaja.Remove(caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CajaExists(int id)
        {
            return _context.AperturaCierreCaja.Any(e => e.ID == id);
        }

        public double? GetEntradasDeDinero(DateTime date)
        {
            return this._context.Recibos.Where(r => r.Fecha.Value.ToShortDateString() == date.ToShortDateString()).Sum(r => r.Abono);
        }
    }
}
