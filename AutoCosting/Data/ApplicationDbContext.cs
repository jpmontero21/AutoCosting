using System;
using System.Collections.Generic;
using System.Text;
using AutoCosting.Models.Maintenance;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoCosting.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Empresa> Empresa { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
