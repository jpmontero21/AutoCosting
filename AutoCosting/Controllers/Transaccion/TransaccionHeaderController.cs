﻿using System;
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
using R = AutoCosting.Models.Receipts;
using AutoCosting.Models.Maintenance;

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
        public IActionResult Index(string option = null, string search = null)
        {
            var transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t=>t.Recibos).Include(t => t.TransDetails).Where(t=>t.Eliminada == false).ToList();
            if (option == "Cliente" && search != null)
            {
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.Cliente.Informacion.ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "Empleado" && search != null)
            {
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.Empleado.NombreCompleto.ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("MM/dd/yyyy");
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.FechaStr.ToLower().Contains(search.Replace("-","/").ToLower())).ToList();
            }
            if (option == "Sede" && search != null)
            {
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.Sede.Nombre.ToLower().Contains(search.ToLower())).ToList();
            }
            if (option == "TransID" && search != null)
            {
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.TransIdStr.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(transaccions);
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
                .Include(t => t.Recibos)
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
            List<R.Recibo> recibos = this._context.Recibos.Where(r => r.TransID == id).ToList();
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,
                TransaccionDetalles = list,
                Recibos = recibos
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
        public async Task<IActionResult> Create( TransaccionHeader transaccionHeader)
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
            List<R.Recibo> recibos = this._context.Recibos.Where(r => r.TransID == id).ToList();
            transaccionHeader.Recibos = recibos;
            transaccionHeader.TransDetails = list;
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,                
                TransaccionDetalles = list,
                Recibos = recibos
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
        public async Task<IActionResult> Edit(int id,  TransaccionViewModel transaccionHeader)
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
            List<R.Recibo> recibos = this._context.Recibos.Where(r => r.TransID == id).ToList();
            TransaccionViewModel transView = new TransaccionViewModel()
            {
                Transaccion = transaccionHeader,
                TransaccionDetalles = list,
                Recibos = recibos
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
            var transaccionHeader = await _context.TransaccionHeaders.Include(t => t.TransDetails).Include(t => t.TransDetails).FirstOrDefaultAsync(t => t.TransID == id);//.FindAsync(id);

            transaccionHeader.TransDetails.ToList().ForEach(detail => 
            {
                Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                if (transaccionHeader.TipoTransaccion == TipoTransaccion.Apartado && vehiculo != null)
                {
                    vehiculo.ApartadoYN = false;
                }
                else if(transaccionHeader.TipoTransaccion == TipoTransaccion.Venta && vehiculo != null)
                {
                    vehiculo.VendidoYN = false;
                }
                this._context.Vehiculos.Update(vehiculo);
            });
            transaccionHeader.Eliminada = true;
            _context.Update(transaccionHeader);
            await _context.SaveChangesAsync();            
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionHeaderExists(int id)
        {
            return _context.TransaccionHeaders.Any(e => e.TransID == id);
        }
               
        public IActionResult ImprimirRep(int transID)
        {
            var dbContext = this._context.TransaccionHeaders.Include(v => v.Empleado).Include(c => c.Cliente).Include(d => d.TransDetails).Include(d => d.Sede).Include(t=>t.Recibos).Where(t => t.TransID == transID);
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

        public async Task<IActionResult> ConvertToSale(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.Include(t=>t.TransDetails).AsNoTracking().FirstOrDefaultAsync(t=>t.TransID==transId);
            transHeader.TipoTransaccion = TipoTransaccion.Venta;
            transHeader.TransDetails.ToList().ForEach(detail =>
            {
                Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                if (vehiculo != null)
                {
                    vehiculo.VendidoYN = true;
                }
                this._context.Vehiculos.Update(vehiculo);
            });
            this._context.TransaccionHeaders.Update(transHeader);
            await _context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Edit),new { id = transId });
        }
    }
}
