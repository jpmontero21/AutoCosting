using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoCosting.Models;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.Models.ViewModel;
using AutoCosting.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoCosting.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext db)            
        {
            this._context = db;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexModel = new IndexViewModel()
            {
                Vehiculos = await _context.Vehiculos.OrderByDescending (v => v.FechaIngreso).Take(7).ToListAsync(),
                Trackings = await this._context.TrackingHeaders.OrderByDescending(t => t.Fecha).Take(7).Include(t=>t.TrackingDetails).Include(t=>t.Vehiculo).ToListAsync(),
                Transaccions = await this._context.TransaccionHeaders.OrderByDescending(t => t.Fecha).Take(7).Include(t=>t.Cliente).ToListAsync(),
                Empresa = await this._context.Empresa.FirstOrDefaultAsync()
            };
            return View(indexModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
