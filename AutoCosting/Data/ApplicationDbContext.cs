using System;
using System.Collections.Generic;
using System.Text;
using AutoCosting.Models;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoCosting.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Sede> Sede { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Trabajo> Trabajos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Sede>()
           .HasKey(c => new { c.ID, c.EmpresaID});
        }
    }
}
