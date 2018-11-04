using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Transaction;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.ViewModel;

namespace AutoCosting.Controllers.Transaccion
{
    public class TransaccionHeaderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransaccionHeaderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransaccionHeader
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Where(t=>t.Eliminada == false);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransaccionHeader/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionHeader = await _context.TransaccionHeaders
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .Include(t => t.Sede)
                .FirstOrDefaultAsync(m => m.TransID == id);
            if (transaccionHeader == null)
            {
                return NotFound();
            }
            List<TransaccionDetail> list = this._context.TransaccionDetails.Where(d => d.TransID == id).ToList();
            list.ForEach(d =>
            {
                d.Vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == d.VINVehiculo);

            });
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,
                TransaccionDetalles = list
            };
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeader.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeader.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeader.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transView);
        }

        // GET: TransaccionHeader/Create
        public IActionResult Create()
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion");
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto");
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre");
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            TransaccionHeader trans = new TransaccionHeader();
            trans.Fecha = DateTime.Today;
            trans.TipoPago = TipoPago.Contado;
            return View(trans);
        }

        // POST: TransaccionHeader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransID,VendedorID,ClienteID,SedeID,EmpresaID,TipoPago,TipoTransaccion,Fecha,Saldo,Eliminada")] TransaccionHeader transaccionHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccionHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { ID = transaccionHeader.TransID });                
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeader.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeader.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeader.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transaccionHeader);
        }

        // GET: TransaccionHeader/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionHeader = await _context.TransaccionHeaders.FindAsync(id);
            if (transaccionHeader == null)
            {
                return NotFound();
            }
            List<TransaccionDetail> list = this._context.TransaccionDetails.Where(d => d.TransID == id).ToList();
            list.ForEach(d =>
            {
                d.Vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == d.VINVehiculo);
                
            });
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,
                TransaccionDetalles = list
            };

            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeader.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeader.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeader.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transView);
        }

        // POST: TransaccionHeader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, /*[Bind("TransID,VendedorID,ClienteID,SedeID,EmpresaID,TipoPago,TipoTransaccion,Fecha,Saldo,Eliminada")]*/ TransaccionViewModel transaccionHeader)
        {
            if (id != transaccionHeader.Transaccion.TransID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccionHeader.Transaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccionHeaderExists(transaccionHeader.Transaccion.TransID))
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
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeader.Transaccion.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeader.Transaccion.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeader.Transaccion.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transaccionHeader);
        }

        // GET: TransaccionHeader/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccionHeader = await _context.TransaccionHeaders
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .Include(t => t.Sede)
                .FirstOrDefaultAsync(m => m.TransID == id);
            if (transaccionHeader == null)
            {
                return NotFound();
            }
            List<TransaccionDetail> list = this._context.TransaccionDetails.Where(d => d.TransID == id).ToList();
            list.ForEach(d =>
            {
                d.Vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == d.VINVehiculo);

            });
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,
                TransaccionDetalles = list
            };
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ID", "Informacion", transaccionHeader.ClienteID);
            ViewData["VendedorID"] = new SelectList(_context.Empleados, "Cedula", "NombreCompleto", transaccionHeader.VendedorID);
            ViewData["SedeID"] = new SelectList(_context.Sede, "ID", "Nombre", transaccionHeader.SedeID);
            ViewData["EmpresaID"] = new SelectList(_context.Empresa, "ID", "ID");
            return View(transView);
        }

        // POST: TransaccionHeader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccionHeader = await _context.TransaccionHeaders.FindAsync(id);
            //_context.TransaccionHeaders.Remove(transaccionHeader);
            transaccionHeader.Eliminada = true;
            _context.Update(transaccionHeader);
            await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionHeaderExists(int id)
        {
            return _context.TransaccionHeaders.Any(e => e.TransID == id);
        }
    }
}
