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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehiculos.ToListAsync());
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
            if (existingVehiculo != null)
            {
                ModelState.AddModelError("VIN", "Ya existe un vehículo con este VIN");
                ViewData["Brands"] = new SelectList(Brands);
                return View(vehiculo);
            }
            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    IFormFileCollection files = HttpContext.Request.Form.Files;
                    ProcessImages(files, vehiculo);
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
            if (id != vehiculo.VIN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                brands.Add("Alfa Romeo");
                brands.Add("Audi");
                brands.Add("BMW");
                brands.Add("BYD");
                brands.Add("Changan");
                brands.Add("Chevrolet");
                brands.Add("Chrysler");
                brands.Add("Citroen");
                brands.Add("Dodge");
                brands.Add("DS");
                brands.Add("Fiat");
                brands.Add("Ford");
                brands.Add("Fuso");
                brands.Add("Geely");
                brands.Add("Great Wall");
                brands.Add("Hino");
                brands.Add("Honda");
                brands.Add("Hyundai");
                brands.Add("Isuzu");
                brands.Add("JAC");
                brands.Add("Jaguar");
                brands.Add("Jeep");
                brands.Add("Kia");
                brands.Add("Land Rover");
                brands.Add("Lexus");
                brands.Add("Mazda");
                brands.Add("Mercedes Benz");
                brands.Add("MG");
                brands.Add("Mitsubishi");
                brands.Add("Nissan");
                brands.Add("Porsche");
                brands.Add("RAM");
                brands.Add("Renault");
                brands.Add("Ssang Yong");
                brands.Add("Subaru");
                brands.Add("Suzuki");
                brands.Add("Toyota");
                brands.Add("Volkswagen");
                brands.Add("Volvo");
                brands.Add("ZNA (DongFeng)");
                return brands;
            }
        }

        private void ProcessImages(IFormFileCollection files_, Vehiculo vehiculo)
        {
            var newFileName = string.Empty;
            var fileName = string.Empty;
            string PathDB = string.Empty;
            IFormFileCollection files = files_;
            fileName = Path.Combine(_environment.WebRootPath, "CarImages", vehiculo.VIN);
            if (System.IO.Directory.Exists(fileName))
                System.IO.Directory.Delete(fileName, true);
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
                                vehiculo.Imagen1 = PathDB;
                                break;
                            case "files2":
                                vehiculo.Imagen2 = PathDB;
                                break;
                            case "files3":
                                vehiculo.Imagen3 = PathDB;
                                break;
                            case "files4":
                                vehiculo.Imagen4 = PathDB;
                                break;
                            case "files5":
                                vehiculo.Imagen5 = PathDB;
                                break;
                        }
                }
            }

        }

    }
}
