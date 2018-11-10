using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCosting.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoCosting.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutoCosting.Controllers.CalcFinanciamiento
{
    [Authorize]
    public class CalcFinanciamientoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalcFinanciamientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Calculate(AutoCosting.Models.Calc.CalcFinanciamiento calc)
        {
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion");
            if (ModelState.IsValid)
            {                
                calc.Calcular();
                return this.RedirectToAction(nameof(Resultado), calc);
            }
            return View("Index");
        }       

        public IActionResult Index()
        {
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion");
            return View();
        }

        public IActionResult Resultado(AutoCosting.Models.Calc.CalcFinanciamiento calc)
        {
            ViewData["VINVehiculo"] = new SelectList(_context.Vehiculos, "VIN", "Descripcion");
            return View(calc);
        }

        [HttpPost]
        public string GetPrecioVehiculo(string vin)
        {
            var vehiculo = this._context.Vehiculos.FirstOrDefault(v => v.VIN == vin);
            if (vehiculo != null)
            {
                return vehiculo.PrecioRecomendado.ToString();
            }
            return string.Empty;
        }
    }
}