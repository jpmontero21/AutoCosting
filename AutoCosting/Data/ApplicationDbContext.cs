using System;
using System.Collections.Generic;
using System.Text;
using AutoCosting.Models;
using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Receipts;
using AutoCosting.Models.Tracking;
using AutoCosting.Models.Transaction;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoCosting.Models.Backups;
using AutoCosting.Models.CierreAperturaCaja;
using System.Linq;
using AutoCosting.Models.TransactionHist;
using AutoCosting.Models.ReceiptsHist;

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
        public DbSet<TrackingHeader> TrackingHeaders { get; set; }
        public DbSet<TrackingDetail> TrackinDetails { get; set; }
        public DbSet<TransaccionHeader> TransaccionHeaders { get; set; }
        public DbSet<TransaccionDetail> TransaccionDetails { get; set; }
        public DbSet<Recibo> Recibos { get; set; }
        public DbSet<Caja> AperturaCierreCaja { get; set; }
        public DbSet<Comision> Comisiones { get; set; }
        //History Tables
        public DbSet<TransaccionHeaderHist> TransHistoryHeader {get; set;}
        public DbSet<TransaccionDetailHist> TransDetailHistory { get; set; }
        public DbSet<ReciboHist> ReciboHistory { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Sede>().HasKey(c => new { c.ID, c.EmpresaID });
            builder.Entity<Cliente>(entity => { entity.HasIndex(c => c.Cedula).IsUnique(); });            
        }
    }
}
