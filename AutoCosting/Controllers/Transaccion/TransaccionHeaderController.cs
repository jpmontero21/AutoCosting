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
using AutoCosting.Models.TransactionHist;
using AutoCosting.Models.ReceiptsHist;
using Microsoft.AspNetCore.Authorization;
using EmisorModel = AutoCosting.Models.FacturacionElectronica;
using FacturaElectronica.ClasesDatos;
using System.Xml;

namespace AutoCosting.Controllers.Transaccion
{
    [Authorize]
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
            var transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).ToList();
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
                search = Convert.ToDateTime(search).ToString("dd/MM/yyyy");
                transaccions = _context.TransaccionHeaders.Include(t => t.Cliente).Include(t => t.Empleado).Include(t => t.Sede).Include(t => t.Recibos).Include(t => t.TransDetails).Where(t => t.Eliminada == false).Where(h => h.FechaStr.ToLower().Contains(search.Replace("-", "/").ToLower())).ToList();
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
        public async Task<IActionResult> Create(TransaccionHeader transaccionHeader)
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
        public async Task<IActionResult> Edit(int id, TransaccionViewModel transaccionHeader)
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
                else if (transaccionHeader.TipoTransaccion == TipoTransaccion.Venta && vehiculo != null)
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
            var dbContext = this._context.TransaccionHeaders.Include(v => v.Empleado).Include(c => c.Cliente).Include(d => d.TransDetails).Include(d => d.Sede).Include(t => t.Recibos).Where(t => t.TransID == transID);
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

        public async Task<IActionResult> EnviarFacturaElectronicaAsync(int transID)
        {
            var dbContext = this._context.TransaccionHeaders.Include(d => d.TransDetails).Where(t => t.TransID == transID);
            if (dbContext == null)
            {
                return NotFound();
            }
            var transaccion = dbContext.ToList().FirstOrDefault();
            if (transaccion == null)
            {
                return NotFound();
            }
            if (transaccion != null && !transaccion.EnviadaHacienda)
            {
                Datos datos = new Datos();
                string consecutivo = datos.CreaNumeroSecuencia("1", "1", "01", "00000019");
                //string consecutivo = datos.CreaNumeroSecuencia("1", "1", "01", transaccion.TransIdStr);
                DateTime date = DateTime.Now;
                //string codigoSeguridad = datos.CreaCodigoSeguridad("01", "1", "1", date, transaccion.TransIdStr);
                string codigoSeguridad = datos.CreaCodigoSeguridad("01", "1", "1", date, "00000019");
                string clave = datos.CreaClave("506", date.Day.ToString(), date.Month.ToString(), date.Year.ToString(), "207030159", consecutivo, "1", codigoSeguridad);

                Cliente client = this._context.Clientes.FirstOrDefault(cliente => cliente.ID == transaccion.ClienteID);
                EmisorModel.Emisor emi = this._context.Emisor.FirstOrDefault( emisor => emisor.ID > 0);
                if (client != null && emi != null)
                {
                    //string rutaCertificado = "C:\\ATV Alexis\\Llave Criptografica (firma) stag\\020703015917.p12";
                    string rutaCertificado = emi.RutaArchivoCertificado;
                    //string rutaOutput = "C:\\OutputFiles\\";
                    string rutaOutput = emi.OutputFolder;
                    Receptor receptor = new Receptor(client.NombreCompleto, "01", client.CedulaSinGuiones, "506", client.Telefono.Replace("-", string.Empty), client.Email);
                    Emisor emisor = new Emisor(emi.NombreCompleto, emi.TipoIdentificacion, emi.NumeroIdentificacion, emi.Provincia, emi.Canton, emi.Distrito, emi.Barrio,
                emi.OtrasSenas, emi.CodigoPaisTelefono, emi.NumeroTelefono, emi.CorreoElectronico, rutaCertificado, emi.PinCertificado, emi.UsuarioApi.Trim(), emi.ClaveApi.Trim(), emi.OutputFolder);
                    int numeroLinea = 0;
                    List<LineaDetalle> listaDetalles = new List<LineaDetalle>();
                    foreach (TransaccionDetail det in transaccion.TransDetails)
                    {
                        LineaDetalle detalle = new LineaDetalle();
                        detalle.NumeroLinea = ++numeroLinea;
                        detalle.ArticuloTipo = "03";
                        detalle.ArticuloCodigo = det.VINVehiculo;
                        detalle.Cantidad = 1;
                        detalle.UnidadMedida = "Sp";
                        detalle.DetalleArticulo = "venta de vehiculo " + det.VINVehiculo;
                        detalle.PrecioUnitario = Convert.ToDouble (det.PrecioAcordado);
                        detalle.MontoDescuento = 0;
                        detalle.NaturalezaDescuento = "0";
                        detalle.ImpuestoCodigo = "07";
                        detalle.ImpuestoTarifa = 0;
                        detalle.isGravado = false;
                        detalle.isServicio = true;
                        listaDetalles.Add(detalle);
                    }
                    CustomFacturaElectronicaCR fact = new CustomFacturaElectronicaCR(consecutivo, clave, emisor, receptor, "01", string.Empty, "01", listaDetalles, "CRC", 1);
                    XmlDocument xmlSinFirmar = fact.CreaXMLFacturaElectronica();
                    emisor.OutputFolder = rutaOutput;
                    try
                    {
                        await fact.ProcesaAsync(xmlSinFirmar.OuterXml, emisor.OutputFolder, consecutivo);
                    }
                    catch (Exception )
                    {
                        transaccion.EnviadaHacienda = false;
                        ///throw;
                        this.TempData["ErrorMessage"] = "Error al enviar a Hacienda. Intente de nuevo más tarde.";
                    }
                    transaccion.ClaveHacienda = clave;
                    transaccion.EnviadaHacienda = true;
                    transaccion.ConsecutivoHacienda = consecutivo;
                    this._context.TransaccionHeaders.Update(transaccion);
                    await _context.SaveChangesAsync();
                }
            }
            return this.RedirectToAction(nameof(Edit), new { id = transID });

        }

        public async Task<IActionResult> ConsultarFacturaElectronicaAsync(int transID)
        {
            var dbContext = this._context.TransaccionHeaders.Include(d => d.TransDetails).Where(t => t.TransID == transID);
            if (dbContext == null)
            {
                return NotFound();
            }
            var transaccion = dbContext.ToList().FirstOrDefault();
            if (transaccion == null)
            {
                return NotFound();
            }
            if (transaccion != null && transaccion.EnviadaHacienda && !string.IsNullOrEmpty(transaccion.ClaveHacienda))
            {
                
                EmisorModel.Emisor emi = this._context.Emisor.FirstOrDefault(emisor => emisor.ID > 0);
                if (emi != null)
                {
                    FacturaElectronicaCR fact = new FacturaElectronicaCR();
                    string rutaCertificado = emi.RutaArchivoCertificado;
                    string rutaOutput = emi.OutputFolder;
                    Emisor emisor = new Emisor(emi.NombreCompleto, emi.TipoIdentificacion, emi.NumeroIdentificacion, emi.Provincia, emi.Canton, emi.Distrito, emi.Barrio,
                emi.OtrasSenas, emi.CodigoPaisTelefono, emi.NumeroTelefono, emi.CorreoElectronico, rutaCertificado, emi.PinCertificado, emi.UsuarioApi.Trim(), emi.ClaveApi.Trim(), emi.OutputFolder);
                    emisor.OutputFolder = emi.OutputFolder;
                    fact.Emisor = emisor;
                    string message = string.Empty;
                    try
                    {
                        message = fact.ConsultaFactura(transaccion.ClaveHacienda, emisor.OutputFolder);
                    }
                    catch (Exception)
                    {
                        this.TempData["ErrorMessage"] = "Hubo un error consultando la factura. Intente mas tarde.";
                    }
                    if (message.ToLower().Equals("aceptada"))
                    {
                        transaccion.AceptadaHacienda = "true";
                        this.TempData["OKMessage"] = message;
                    }
                    else
                    {
                        transaccion.AceptadaHacienda = "false";
                        this.TempData["ErrorMessage"] = message;
                    }
                    this._context.TransaccionHeaders.Update(transaccion);
                    await _context.SaveChangesAsync();
                }
            }
            return this.RedirectToAction(nameof(Edit), new { id = transID });

        }        

        public async Task<IActionResult> ConvertToSale(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.Include(t => t.TransDetails).AsNoTracking().FirstOrDefaultAsync(t => t.TransID == transId);
            if (transHeader != null && transHeader.TipoTransaccion == TipoTransaccion.Apartado)
            {
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
            }
            else if (transHeader != null && transHeader.TipoTransaccion == TipoTransaccion.Cotizacion)
            {
                string error = string.Empty;
                transHeader.TransDetails.ToList().ForEach(detail =>
                {
                    Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                    if (vehiculo != null)
                    {
                        if (vehiculo.ApartadoYN || vehiculo.VendidoYN)
                        {//ModelState.AddModelError("TipoTransaccion", "No se puede convertir a venta, porque uno o más vehículos ya fueron vendidos o apartados.");
                            error = "No se puede convertir a venta, porque uno o más vehículos ya fueron vendidos o apartados.";
                        }
                        else
                        {
                            vehiculo.VendidoYN = true;
                        }
                        if (string.IsNullOrEmpty(error))
                            this._context.Vehiculos.Update(vehiculo);
                    }                    
                });
                if (!string.IsNullOrEmpty(error))
                {
                    this.TempData["ErrorMessage"] = error;
                    return this.RedirectToAction(nameof(Edit), new { id = transId });
                }
                transHeader.TipoTransaccion = TipoTransaccion.Venta;
                this._context.TransaccionHeaders.Update(transHeader);
                await _context.SaveChangesAsync();
            }
            return this.RedirectToAction(nameof(Edit), new { id = transId });
        }

        public async Task<IActionResult> ConvertToReserve(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.Include(t => t.TransDetails).AsNoTracking().FirstOrDefaultAsync(t => t.TransID == transId);            
            string error = string.Empty;
            transHeader.TransDetails.ToList().ForEach(detail =>
            {
                Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                if (vehiculo != null)
                {
                    if (vehiculo.ApartadoYN || vehiculo.VendidoYN)
                    {//ModelState.AddModelError("TipoTransaccion", "No se puede convertir a venta, porque uno o más vehículos ya fueron vendidos o apartados.");
                        error = "No se puede convertir a apartado, porque uno o más vehículos ya fueron vendidos o apartados.";
                    }
                    else
                    {
                        vehiculo.ApartadoYN = true;
                    }
                    if (string.IsNullOrEmpty(error))
                        this._context.Vehiculos.Update(vehiculo);
                }                
            });
            if (!string.IsNullOrEmpty(error))
            {//ModelState.AddModelError("TipoTransaccion", "No se puede convertir a apartado, porque uno o más vehículos ya fueron vendidos o apartados.");
                this.TempData["ErrorMessage"] = error;
                return this.RedirectToAction(nameof(Edit), new { id = transId });
            }
            transHeader.TipoTransaccion = TipoTransaccion.Apartado;
            this._context.TransaccionHeaders.Update(transHeader);
            await _context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Edit), new { id = transId });
        }

        public async Task<IActionResult> CloseQuote(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.AsNoTracking().Include(t => t.TransDetails).AsNoTracking().Include(t => t.Recibos).AsNoTracking().FirstOrDefaultAsync(t => t.TransID == transId);
            if (transHeader != null)
            {
                TransaccionHeaderHist histHeader = new TransaccionHeaderHist()
                {
                    TransID = transHeader.TransID,
                    VendedorID = transHeader.VendedorID,
                    ClienteID = transHeader.ClienteID,
                    SedeID = transHeader.SedeID,
                    EmpresaID = transHeader.EmpresaID,
                    TipoPago = transHeader.TipoPago,
                    TipoTransaccion = transHeader.TipoTransaccion,
                    Fecha = transHeader.Fecha,
                    Eliminada = transHeader.Eliminada
                };
                //Create Details
                List<TransaccionDetailHist> histDetails = new List<TransaccionDetailHist>();
                transHeader.TransDetails.ToList().ForEach(detail =>
                {
                    TransaccionDetailHist histDetail = new TransaccionDetailHist()
                    {
                        ID = detail.ID,
                        TransID = detail.TransID,
                        VINVehiculo = detail.VINVehiculo,
                        PrecioAcordado = detail.PrecioAcordado
                    };
                    histDetails.Add(histDetail);
                });

                //Add Hist Header
                await this._context.TransHistoryHeader.AddAsync(histHeader);
                //Add Hist Details
                await this._context.TransDetailHistory.AddRangeAsync(histDetails);

                await _context.SaveChangesAsync();

                //Remove Live Trans
                this._context.TransaccionDetails.RemoveRange(transHeader.TransDetails);
                this._context.Recibos.RemoveRange(transHeader.Recibos);
                this._context.TransaccionHeaders.Remove(transHeader);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return NotFound();

        }

        public async Task<IActionResult> CloseReserve(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.AsNoTracking().Include(t => t.TransDetails).AsNoTracking().Include(t => t.Recibos).AsNoTracking().FirstOrDefaultAsync(t => t.TransID == transId);
            if (transHeader != null)
            {
                TransaccionHeaderHist histHeader = new TransaccionHeaderHist()
                {
                    TransID = transHeader.TransID,
                    VendedorID = transHeader.VendedorID,
                    ClienteID = transHeader.ClienteID,
                    SedeID = transHeader.SedeID,
                    EmpresaID = transHeader.EmpresaID,
                    TipoPago = transHeader.TipoPago,
                    TipoTransaccion = transHeader.TipoTransaccion,
                    Fecha = transHeader.Fecha,
                    Eliminada = transHeader.Eliminada
                };

                //Create Details
                List<TransaccionDetailHist> histDetails = new List<TransaccionDetailHist>();
                transHeader.TransDetails.ToList().ForEach(detail =>
                {
                    TransaccionDetailHist histDetail = new TransaccionDetailHist()
                    {
                        ID = detail.ID,
                        TransID = detail.TransID,
                        VINVehiculo = detail.VINVehiculo,
                        PrecioAcordado = detail.PrecioAcordado
                    };
                    histDetails.Add(histDetail);
                    //Set ApartadoYN = false
                    Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                    if (vehiculo != null)
                    {
                        vehiculo.ApartadoYN = false;
                        this._context.Vehiculos.Update(vehiculo);
                        this._context.SaveChanges();
                    }
                });

                //Create Receipts
                List<ReciboHist> histReceipts = new List<ReciboHist>();
                transHeader.Recibos.ToList().ForEach(recibo =>
                {
                    ReciboHist histReceipt = new ReciboHist()
                    {
                        ID = recibo.ID,
                        TransID = recibo.TransID,
                        Descripcion = recibo.Descripcion,
                        Abono = recibo.Abono,
                        Fecha = recibo.Fecha
                    };
                    histReceipts.Add(histReceipt);
                });
                //Update Header
                await this._context.TransHistoryHeader.AddAsync(histHeader);
                //Update Details
                await this._context.TransDetailHistory.AddRangeAsync(histDetails);
                //Update Receipts
                await this._context.ReciboHistory.AddRangeAsync(histReceipts);
                await _context.SaveChangesAsync();


                this._context.TransaccionDetails.RemoveRange(transHeader.TransDetails);
                this._context.Recibos.RemoveRange(transHeader.Recibos);
                this._context.TransaccionHeaders.Remove(transHeader);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return NotFound();

        }

        public async Task<IActionResult> CloseSale(int? transId)
        {
            var transHeader = await this._context.TransaccionHeaders.AsNoTracking().Include(t => t.TransDetails).AsNoTracking().Include(t => t.Recibos).AsNoTracking().FirstOrDefaultAsync(t => t.TransID == transId);
            if (transHeader != null)
            {
                TransaccionHeaderHist histHeader = new TransaccionHeaderHist()
                {
                    TransID = transHeader.TransID,
                    VendedorID = transHeader.VendedorID,
                    ClienteID = transHeader.ClienteID,
                    SedeID = transHeader.SedeID,
                    EmpresaID = transHeader.EmpresaID,
                    TipoPago = transHeader.TipoPago,
                    TipoTransaccion = transHeader.TipoTransaccion,
                    Fecha = transHeader.Fecha,
                    Eliminada = transHeader.Eliminada,
                    AceptadaHacienda = transHeader.AceptadaHacienda,
                    ClaveHacienda = transHeader.ClaveHacienda,
                    EnviadaHacienda = transHeader.EnviadaHacienda,
                    ConsecutivoHacienda = transHeader.ConsecutivoHacienda
                };

                //Create Details
                List<TransaccionDetailHist> histDetails = new List<TransaccionDetailHist>();
                transHeader.TransDetails.ToList().ForEach(detail =>
                {
                    TransaccionDetailHist histDetail = new TransaccionDetailHist()
                    {
                        ID = detail.ID,
                        TransID = detail.TransID,
                        VINVehiculo = detail.VINVehiculo,
                        PrecioAcordado = detail.PrecioAcordado
                    };
                    histDetails.Add(histDetail);
                    //Set ApartadoYN = false
                    Vehiculo vehiculo = this._context.Vehiculos.AsNoTracking().FirstOrDefault(v => v.VIN == detail.VINVehiculo);
                    if (vehiculo != null)
                    {
                        vehiculo.ApartadoYN = true;
                        vehiculo.VendidoYN = true;
                        this._context.Vehiculos.Update(vehiculo);
                        this._context.SaveChanges();
                    }
                });

                //Create Receipts
                List<ReciboHist> histReceipts = new List<ReciboHist>();
                transHeader.Recibos.ToList().ForEach(recibo =>
                {
                    ReciboHist histReceipt = new ReciboHist()
                    {
                        ID = recibo.ID,
                        TransID = recibo.TransID,
                        Descripcion = recibo.Descripcion,
                        Abono = recibo.Abono,
                        Fecha = recibo.Fecha
                    };
                    histReceipts.Add(histReceipt);
                });

                List<Comision> comisiones = await this._context.Comisiones.Where(c => c.TransID == transId).ToListAsync();
                List<ComisionHist> histComm = new List<ComisionHist>();
                comisiones.ForEach(comm =>
                {
                    ComisionHist hist = new ComisionHist()
                    {
                        ID = comm.ID,
                        Descripcion = comm.Descripcion,
                        Monto = comm.Monto,
                        Nombre = comm.Nombre,
                        TipoComision = comm.TipoComision,
                        TransID = comm.TransID
                    };
                    histComm.Add(hist);
                });

                //Update Header
                await this._context.TransHistoryHeader.AddRangeAsync(histHeader);
                //Update Details
                await this._context.TransDetailHistory.AddRangeAsync(histDetails);
                //Update Receipts
                await this._context.ReciboHistory.AddRangeAsync(histReceipts);
                //Update Commission Receipts
                await this._context.ComisionHistory.AddRangeAsync(histComm);


                await _context.SaveChangesAsync();


                this._context.TransaccionDetails.RemoveRange(transHeader.TransDetails);
                this._context.Recibos.RemoveRange(transHeader.Recibos);
                this._context.TransaccionHeaders.Remove(transHeader);
                this._context.Comisiones.RemoveRange(comisiones);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
