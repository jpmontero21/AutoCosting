using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Maintenance;
using AutoCosting.HelpersAndValidations;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.Models.Transaction;
using AutoCosting.Models;

namespace AutoCosting.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EmpleadoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empleado
        public async Task<IActionResult> Index(string option=null,string search=null)
        {
            var empleados = await _context.Empleados.ToListAsync();
            if (option == "Cedula" && search != null)
            {
                empleados = await _context.Empleados.Where(c => c.Cedula.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Nombre" && search != null)
            {
                empleados = await _context.Empleados.Where(c => c.Nombre.ToLower().Contains(search.ToLower()) ||
                c.Apellido1.ToLower().Contains(search.ToLower()) ||
                c.Apellido2.ToLower().Contains(search.ToLower())
                ).ToListAsync();
            }
            if (option == "Telefono" && search != null)
            {
                empleados = await _context.Empleados.Where(c => c.Telefono.ToLower().Contains(search.ToLower())).ToListAsync();
            }

            return View(empleados);
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Cedula == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cedula,Nombre,Apellido1,Apellido2,Direccion,Telefono,Notas,FechaNacimiento,DescripcionPuesto")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Cedula,Nombre,Apellido1,Apellido2,Direccion,Telefono,Notas,FechaNacimiento,DescripcionPuesto")] Empleado empleado)
        {
            if (id != empleado.Cedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Cedula))
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
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Cedula == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            TransaccionHeader trans = await this._context.TransaccionHeaders.FirstOrDefaultAsync(t => t.VendedorID == id);
            if (trans != null)
            {
                ModelState.AddModelError("Cedula", "No se puede eliminar este empleado.");
                return View(empleado);
            }
            ApplicationUser user = await this._context.Users.FirstOrDefaultAsync(u => u.EmpleadoID == id);
            if (user != null)
            {
                ModelState.AddModelError("Cedula", "No se puede eliminar este empleado.");
                return View(empleado);
            }
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(string id)
        {
            return _context.Empleados.Any(e => e.Cedula == id);
        }
    }
}
