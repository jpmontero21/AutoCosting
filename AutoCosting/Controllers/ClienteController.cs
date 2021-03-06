﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Data;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Authorization;
using AutoCosting.Models.Transaction;

namespace AutoCosting.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index(string option=null,string search=null)
        {
            var clientes = await _context.Clientes.ToListAsync();
            if (option == "Cedula" && search != null)
            {
                clientes = await _context.Clientes.Where(c=> c.Cedula.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Nombre" && search != null)
            {
                clientes = await _context.Clientes.Where(c => c.Nombre.ToLower().Contains(search.ToLower()) ||
                c.Apellido1.ToLower().Contains(search.ToLower()) ||
                c.Apellido2.ToLower().Contains(search.ToLower())
                ).ToListAsync();
            }
            if (option == "Telefono" && search != null)
            {
                clientes = await _context.Clientes.Where(c => c.Telefono.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            if (option == "Email" && search != null)
            {
                clientes = await _context.Clientes.Where(c => c.Email.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            return View(clientes);
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("ID,Nombre,Apellido1,Apellido2,Direccion,Telefono,Email,Notas")] */Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (_context.Clientes.AsNoTracking().FirstOrDefault(c=> c.Cedula == cliente.Cedula) != null)
                {
                    ModelState.AddModelError("Cedula", "Ya existe un cliente con la cedula ingresada.");
                    return View(cliente);
                }
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, /*[Bind("ID,Nombre,Apellido1,Apellido2,Direccion,Telefono,Email,Notas")]*/ Cliente cliente)
        {
            if (id != cliente.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ID))
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
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            TransaccionHeader trans = await this._context.TransaccionHeaders.FirstOrDefaultAsync(t => t.ClienteID == id);
            if (trans != null)
            {
                ModelState.AddModelError("ID", "No se puede eliminar este cliente.");
                return View(cliente);
            }            
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ID == id);
        }
    }
}
