using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using AutoCosting.Models.ViewModel;
using AutoCosting.Models.Transaction;

namespace AutoCosting.Controllers
{
    [Authorize]
    public class VehiculoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        public VehiculoController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;
        }

        // GET: Vehiculo
        public async Task<IActionResult> Index(string option = null,string search = null)
        {
            var vehiculos = await _context.Vehiculos.Where(v=>!v.ApartadoYN && !v.VendidoYN).ToListAsync();
            if (option == "Marca" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v=> !v.ApartadoYN && !v.VendidoYN && v.Marca.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Modelo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Modelo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Anno" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Anno.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Transmision" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Transmision.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }

            if (option == "Estilo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Estilo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Combustible" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Combustible.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Estado" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.Estado.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("MM/dd/yyyy");
                vehiculos = await _context.Vehiculos.Where(v => !v.ApartadoYN && !v.VendidoYN && v.FechaIngreso.ToString("MM/dd/yyyy").ToLower().Contains(search.Replace("-","/").ToLower())).ToListAsync();
            }
            return View(vehiculos);
        }

        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(m => m.VIN == id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            ViewData["Brands"] = new SelectList(Brands);
            return View(vehiculo);
        }

        // GET: Vehiculo/Create
        public IActionResult Create()
        {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.FechaIngreso = DateTime.Today;            
            ViewData["Brands"] = new SelectList(Brands);
            return View(vehiculo);
        }

        // POST: Vehiculo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            Vehiculo existingVehiculo = await _context.Vehiculos.SingleOrDefaultAsync(m => m.VIN == vehiculo.VIN);
            ViewData["Brands"] = new SelectList(Brands);
            if (existingVehiculo != null)
            {
                ModelState.AddModelError("VIN", "Ya existe un vehículo con este VIN");                
                return View(vehiculo);
            }
            if (ModelState.IsValid)
            {
                if (vehiculo.PrecioMinimo > vehiculo.PrecioRecomendado)
                {
                    ModelState.AddModelError("PrecioRecomendado","El precio recomendado debe ser mayor que el precio mínimo.");
                    return View(vehiculo);
                }
                if (HttpContext.Request.Form.Files != null)
                {
                    IFormFileCollection files = HttpContext.Request.Form.Files;
                    ProcessImages(files, vehiculo, true);
                }
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehiculo);
        }

        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            if (vehiculo.VendidoYN)
            {
                return View(nameof(Details), vehiculo);
            }

            ViewData["Brands"] = new SelectList(Brands);
            return View(vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Vehiculo vehiculo)
        {
            ViewData["Brands"] = new SelectList(Brands);
            if (id != vehiculo.VIN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vehiculo.PrecioMinimo > vehiculo.PrecioRecomendado)
                    {
                        ModelState.AddModelError("PrecioRecomendado", "El precio recomendado debe ser mayor que el precio mínimo.");
                        return View(vehiculo);
                    }
                    if (HttpContext.Request.Form.Files != null)
                    {
                        IFormFileCollection files = HttpContext.Request.Form.Files;
                        ProcessImages(files, vehiculo, false);
                    }
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.VIN))
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
            return View(vehiculo);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(m => m.VIN == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(string id)
        {
            return _context.Vehiculos.Any(e => e.VIN == id);
        }

        public List<string> Brands
        {
            get
            {
                List<string> brands = new List<string>();
                brands.Add("Acura");
                brands.Add("Alfa Romeo");
                brands.Add("AMC");
                brands.Add("Aro");
                brands.Add("Asia");
                brands.Add("Aston Martin");
                brands.Add("Audi");
                brands.Add("Austin");
                brands.Add("Bentley");
                brands.Add("Bluebird");
                brands.Add("BMW");
                brands.Add("Buick");
                brands.Add("BYD");
                brands.Add("Cadillac");
                brands.Add("Chana");
                brands.Add("Changan");
                brands.Add("Chery");
                brands.Add("Chevrolet");
                brands.Add("Chrysler");
                brands.Add("Citroen");
                brands.Add("Dacia");
                brands.Add("Daewoo");
                brands.Add("Daihatsu");
                brands.Add("Datsun");
                brands.Add("Dodge/RAM");
                brands.Add("Donfeng (ZNA)");
                brands.Add("Eagle");
                brands.Add("Faw");
                brands.Add("Ferrari");
                brands.Add("Fiat");
                brands.Add("Ford");
                brands.Add("Foton");
                brands.Add("Freightliner");
                brands.Add("Geely");
                brands.Add("Genesis");
                brands.Add("Geo");
                brands.Add("GMC");
                brands.Add("Gonow");
                brands.Add("Great Wall");
                brands.Add("Hafei");
                brands.Add("Heibao");
                brands.Add("Higer");
                brands.Add("Hino");
                brands.Add("Honda");
                brands.Add("Hummer");
                brands.Add("Hyundai");
                brands.Add("Infiniti");
                brands.Add("International");
                brands.Add("Isuzu");
                brands.Add("Iveco");
                brands.Add("JAC");
                brands.Add("Jaguar");
                brands.Add("Jeep");
                brands.Add("Jinbei");
                brands.Add("JMC");
                brands.Add("Kenworth");
                brands.Add("Kia");
                brands.Add("Lada");
                brands.Add("Lamborghini");
                brands.Add("Lancia");
                brands.Add("Land Rover");
                brands.Add("Lexus");
                brands.Add("Lifan");
                brands.Add("Lincoln");
                brands.Add("Lotus");
                brands.Add("Mack");
                brands.Add("Magiruz");
                brands.Add("Mahindra");
                brands.Add("Maserati");
                brands.Add("Mazda");
                brands.Add("Mercedes Benz");
                brands.Add("Mercury");
                brands.Add("MG");
                brands.Add("Mini");
                brands.Add("Mitsubishi");
                brands.Add("Nissan");
                brands.Add("Oldsmobile");
                brands.Add("Opel");
                brands.Add("Peterbilt");
                brands.Add("Peugeot");
                brands.Add("Plymouth");
                brands.Add("Polarsun");
                brands.Add("Pontiac");
                brands.Add("Porsche");
                brands.Add("Proton");
                brands.Add("Rambler");
                brands.Add("Renault");
                brands.Add("Reva");
                brands.Add("Rolls Royce");
                brands.Add("Rover");
                brands.Add("Saab");
                brands.Add("Samsung");
                brands.Add("Saturn");
                brands.Add("Scania");
                brands.Add("Scion");
                brands.Add("Seat");
                brands.Add("Skoda");
                brands.Add("Smart");
                brands.Add("Ssang Yong");
                brands.Add("Subaru");
                brands.Add("Suzuki");
                brands.Add("Tianma");
                brands.Add("Tiger Truck");
                brands.Add("Toyota");
                brands.Add("Volkswagen");
                brands.Add("Volvo");
                brands.Add("Western Star");
                brands.Add("Yugo");
                return brands;
            }
        }

        private void ProcessImages(IFormFileCollection files_, Vehiculo vehiculo, bool isAdd)
        {
            var newFileName = string.Empty;
            var fileName = string.Empty;
            string PathDB = string.Empty;
            IFormFileCollection files = files_;
            if (isAdd)
            {
                fileName = Path.Combine(_environment.WebRootPath, "CarImages", vehiculo.VIN);
                if (System.IO.Directory.Exists(fileName))
                    System.IO.Directory.Delete(fileName, true);
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //Getting FileName
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var FileExtension = Path.GetExtension(fileName);

                    // concating  FileName + FileExtension
                    newFileName = myUniqueFileName + FileExtension;

                    // Combines two strings into a path.
                    fileName = Path.Combine(_environment.WebRootPath, "CarImages", vehiculo.VIN);
                    System.IO.Directory.CreateDirectory(fileName);
                    fileName += $@"\{newFileName}";

                    // if you want to store path of folder in database
                    PathDB = "CarImages/" + vehiculo.VIN + "/" + newFileName;

                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    using (MagickImage image = new MagickImage(fileName))
                    {
                        image.Resize(640, 480);
                        image.Write(fileName);
                    }
                        switch (file.Name)
                        {
                            case "files1":
                            {
                                if (!string.IsNullOrEmpty(vehiculo.Imagen1) && !isAdd)
                                {
                                    string fileNameExistent = Path.Combine(_environment.WebRootPath, vehiculo.Imagen1);
                                    fileNameExistent = fileNameExistent.Replace(@"\", "//");
                                    if (System.IO.File.Exists(fileNameExistent))
                                        System.IO.File.Delete(fileNameExistent);
                                }
                                vehiculo.Imagen1 = PathDB;
                                break;
                            }
                            case "files2":
                            {
                                if (!string.IsNullOrEmpty(vehiculo.Imagen2) && !isAdd)
                                {
                                    string fileNameExistent = Path.Combine(_environment.WebRootPath, vehiculo.Imagen2);
                                    fileNameExistent = fileNameExistent.Replace(@"\", "//");
                                    if (System.IO.File.Exists(fileNameExistent))
                                        System.IO.File.Delete(fileNameExistent);
                                }
                                vehiculo.Imagen2 = PathDB;
                                break;
                            }
                            case "files3":
                            {
                                if (!string.IsNullOrEmpty(vehiculo.Imagen3) && !isAdd)
                                {
                                    string fileNameExistent = Path.Combine(_environment.WebRootPath, vehiculo.Imagen3);
                                    fileNameExistent = fileNameExistent.Replace(@"\", "//");
                                    if (System.IO.File.Exists(fileNameExistent))
                                        System.IO.File.Delete(fileNameExistent);
                                }
                                vehiculo.Imagen3 = PathDB;
                                break;
                            }
                            case "files4":
                            {
                                if (!string.IsNullOrEmpty(vehiculo.Imagen4) && !isAdd)
                                {
                                    string fileNameExistent = Path.Combine(_environment.WebRootPath, vehiculo.Imagen4);
                                    fileNameExistent = fileNameExistent.Replace(@"\", "//");
                                    if (System.IO.File.Exists(fileNameExistent))
                                        System.IO.File.Delete(fileNameExistent);
                                }
                                vehiculo.Imagen4 = PathDB;
                                break;
                            }
                            case "files5":
                            {
                                if (!string.IsNullOrEmpty(vehiculo.Imagen5) && !isAdd)
                                {
                                    string fileNameExistent = Path.Combine(_environment.WebRootPath, vehiculo.Imagen5);
                                    fileNameExistent = fileNameExistent.Replace(@"\", "//");
                                    if (System.IO.File.Exists(fileNameExistent))
                                        System.IO.File.Delete(fileNameExistent);
                                }
                                vehiculo.Imagen5 = PathDB;
                                break;
                            }
                        }
                }
            }

        }

        public async Task<IActionResult> SoldVehicles(string option = null, string search = null)
        {
            var vehiculos = await _context.Vehiculos.Where(v=>v.VendidoYN).ToListAsync();
            if (option == "Marca" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Marca.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Modelo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Modelo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Anno" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Anno.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Transmision" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Transmision.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }

            if (option == "Estilo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Estilo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Combustible" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Combustible.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Estado" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.Estado.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("MM/dd/yyyy");
                vehiculos = await _context.Vehiculos.Where(v => v.VendidoYN && v.FechaIngreso.ToString("MM/dd/yyyy").ToLower().Contains(search.Replace("-", "/").ToLower())).ToListAsync();
            }
            return View(vehiculos);
        }

        public async Task<IActionResult> ReservedVehicles(string option = null, string search = null)
        {
            var vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN).ToListAsync();
            if (option == "Marca" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Marca.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Modelo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Modelo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Anno" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Anno.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Transmision" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Transmision.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }

            if (option == "Estilo" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Estilo.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Combustible" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Combustible.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Estado" && search != null)
            {
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.Estado.ToString().ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Fecha" && search != null)
            {
                search = Convert.ToDateTime(search).ToString("MM/dd/yyyy");
                vehiculos = await _context.Vehiculos.Where(v => v.ApartadoYN && v.FechaIngreso.ToString("MM/dd/yyyy").ToLower().Contains(search.Replace("-", "/").ToLower())).ToListAsync();
            }
            return View(vehiculos);
        }

        public async Task<IActionResult> VehicleHistory(string VIN)
        {
            var vehiculo = await this._context.Vehiculos.FirstOrDefaultAsync(v => v.VIN == VIN);
            if (vehiculo == null)
            {
                return NotFound();
            }
            var trackingHeader = await this._context.TrackingHeaders.Include(t=>t.TrackingDetails).FirstOrDefaultAsync(t => t.VINVehiculo == VIN);


            var transHeaders = await this._context.TransaccionHeaders.Include(t => t.TransDetails).Include(t=>t.Cliente).Include(t => t.Empleado).Where(t => t.TransDetails.FirstOrDefault(d => d.VINVehiculo == VIN) != null).ToListAsync();

            // t => new{
            //    TransaccionHeader = t,
            //    TransaccionDetail = t.TransDetails.Where(d => d.VINVehiculo == VIN)
            //}).Include(t=> t.TransaccionHeader.Cliente).Include(t=>t.TransaccionHeader.Empleado).ToListAsync();

            var transHistory = await this._context.TransHistoryHeader.Include(t => t.TransDetails).Include(t=>t.Cliente).Include(t => t.Empleado).Where(t => t.TransDetails.FirstOrDefault(d => d.VINVehiculo == VIN) != null).ToListAsync();
            //{
            //    TransaccionHeaderHist = t,
            //    TransaccionDetailHist = t.TransDetails.Where(d=>d.VINVehiculo == VIN)
            //}).Include(t => t.TransaccionHeaderHist.Cliente).Include(t => t.TransaccionHeaderHist.Empleado).ToListAsync();

            //var transDetails = await this._context.TransaccionDetails.Where(d => d.VINVehiculo == VIN).ToListAsync();

            

            VehiculoHistoryViewModel historyModel = new VehiculoHistoryViewModel()
            {
                Vehiculo = vehiculo,
                Tracking = trackingHeader,
                TransaccionHeaderList = transHeaders,
                TransaccionHeaderHistList =transHistory
                //TransaccionHeaderList = 
                //TransaccionHeaderHistList = transHistory
            };

            return View(historyModel);
        }
    }
}
