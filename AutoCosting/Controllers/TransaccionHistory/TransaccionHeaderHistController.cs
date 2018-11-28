using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.TransactionHist;
using Microsoft.AspNetCore.Authorization;

namespace AutoCosting.Controllers.TransaccionHistory
{
    [Authorize]
    public class TransaccionHeaderHistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaccionHeaderHistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransaccionHeaderHist
        public async Task<IActionResult> Index(string option = null, string search = null)
        {
            var transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).ToListAsync();
            if (option == "Cliente" && search != null)
            {
                transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(h => h.Cliente.Informacion.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Empleado" && search != null)
            {
                transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(h => h.Empleado.NombreCompleto.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("dd/MM/yyyy");
                transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(h => h.FechaStr.ToLower().Contains(search.Replace("-", "/").ToLower())).ToListAsync();
            }
            if (option == "Sede" && search != null)
            {
                transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(h => h.Sede.Nombre.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "TransID" && search != null)
            {
                transaccions = await _context.TransHistoryHeader.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(h => h.TransIdStr.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            return View(transaccions);
        }

        // GET: TransaccionHeaderHist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionHeaderHist = await _context.TransHistoryHeader
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .Include(t => t.Sede)
                .Include(t => t.TransDetails)
                .Include(t => t.Recibos)                
                .FirstOrDefaultAsync(m => m.TransID == id);
            transaccionHeaderHist.TransDetails.ToList().ForEach(detail => 
            {
                detail.Vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == detail.VINVehiculo);
            });
            if (transaccionHeaderHist == null)
            {
                return NotFound();
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeaderHist.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeaderHist.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeaderHist.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transaccionHeaderHist);
        }

        public IActionResult ImprimirRep(int transID)
        {
            var dbContext = this._context.TransHistoryHeader.Include(v => v.Empleado).Include(c => c.Cliente).Include(d => d.TransDetails).Include(d => d.Sede).Include(t => t.Recibos).Where(t => t.TransID == transID);
            if (dbContext == null)//Include(t => t.Vehiculo).Include(d=>d.TrackingDetails)
            {
                return NotFound();
            }
            var transaccion = dbContext.ToList().FirstOrDefault();
            if (transaccion == null)
            {
                return NotFound();
            }
            transaccion.TransDetails.ToList().ForEach(detail =>
            {
                detail.Vehiculo = this._context.Vehiculos.First(v => v.VIN == detail.VINVehiculo);
            });
            transaccion.Sede.Empresa = this._context.Empresa.FirstOrDefault(e => e.ID == transaccion.EmpresaID);
            return View(transaccion);
        }

        public async Task<IActionResult> ReceiptDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recibo = await _context.ReciboHistory
                .Include(r => r.Parent)
                .Include(r => r.Parent.Sede)
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
    }
}
