﻿// <auto-generated />
using System;
using AutoCosting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutoCosting.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181022002421_update-database")]
    partial class updatedatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutoCosting.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("EmpleadoID");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Cliente", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido1")
                        .IsRequired();

                    b.Property<string>("Apellido2");

                    b.Property<string>("Direccion");

                    b.Property<string>("Email");

                    b.Property<string>("Nombre")
                        .IsRequired();

                    b.Property<string>("Notas");

                    b.Property<string>("Telefono");

                    b.HasKey("ID");

                    b.ToTable("tblCliente");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Empleado", b =>
                {
                    b.Property<string>("Cedula");

                    b.Property<string>("Apellido1")
                        .IsRequired();

                    b.Property<string>("Apellido2");

                    b.Property<string>("DescripcionPuesto");

                    b.Property<string>("Direccion");

                    b.Property<DateTime>("FechaNacimiento");

                    b.Property<string>("Nombre")
                        .IsRequired();

                    b.Property<string>("Notas");

                    b.Property<string>("Telefono");

                    b.HasKey("Cedula");

                    b.ToTable("tblEmpleado");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Empresa", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactEmail");

                    b.Property<string>("Direccion");

                    b.Property<bool>("MultiSedeYN");

                    b.Property<string>("Nombre")
                        .IsRequired();

                    b.Property<string>("Telefono");

                    b.HasKey("ID");

                    b.ToTable("tblEmpresa");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Proveedor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direccion");

                    b.Property<string>("Email");

                    b.Property<string>("NombreContacto")
                        .IsRequired();

                    b.Property<string>("NombreEmpresa");

                    b.Property<string>("Notas");

                    b.Property<string>("Telefono");

                    b.HasKey("ID");

                    b.ToTable("tblProveedor");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Sede", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmpresaID");

                    b.Property<string>("Direccion");

                    b.Property<bool>("ImprimirLogoYN");

                    b.Property<string>("Nombre")
                        .IsRequired();

                    b.Property<string>("Telefono");

                    b.Property<bool>("UsarCierreCajaYN");

                    b.HasKey("ID", "EmpresaID");

                    b.HasAlternateKey("EmpresaID", "ID");

                    b.ToTable("tblSede");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Trabajo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .IsRequired();

                    b.Property<double>("PrecioPromedio");

                    b.HasKey("ID");

                    b.ToTable("tblTrabajo");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Vehiculo", b =>
                {
                    b.Property<string>("VIN");

                    b.Property<int>("Anno");

                    b.Property<bool>("ApartadoYN");

                    b.Property<int>("Cilindrada");

                    b.Property<string>("Color");

                    b.Property<short>("Combustible");

                    b.Property<short>("Estado");

                    b.Property<string>("Estilo")
                        .IsRequired();

                    b.Property<DateTime>("FechaIngreso");

                    b.Property<int>("Kilometraje");

                    b.Property<string>("Marca")
                        .IsRequired();

                    b.Property<string>("Modelo")
                        .IsRequired();

                    b.Property<string>("NumeroPuertas")
                        .IsRequired();

                    b.Property<string>("Placa");

                    b.Property<double>("PrecioMinimo");

                    b.Property<double>("PrecioRecomendado");

                    b.Property<short>("Transmision");

                    b.HasKey("VIN");

                    b.ToTable("tblVehiculo");
                });

            modelBuilder.Entity("AutoCosting.Models.Tracking.TrackingDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddDescripcion");

                    b.Property<double>("Costo");

                    b.Property<string>("Descripcion");

                    b.Property<string>("NumeroFactura");

                    b.Property<int?>("ProveedorId");

                    b.Property<int>("TrabajoId");

                    b.Property<int>("TrackingId");

                    b.HasKey("ID");

                    b.HasIndex("ProveedorId");

                    b.HasIndex("TrabajoId");

                    b.HasIndex("TrackingId");

                    b.ToTable("tblTrackingDetail");
                });

            modelBuilder.Entity("AutoCosting.Models.Tracking.TrackingHeader", b =>
                {
                    b.Property<int>("TrackingID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Fecha");

                    b.Property<string>("Notas");

                    b.Property<string>("VINVehiculo")
                        .IsRequired();

                    b.HasKey("TrackingID");

                    b.HasIndex("VINVehiculo");

                    b.ToTable("tblTrackingHeader");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AutoCosting.Models.Maintenance.Sede", b =>
                {
                    b.HasOne("AutoCosting.Models.Maintenance.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutoCosting.Models.Tracking.TrackingDetail", b =>
                {
                    b.HasOne("AutoCosting.Models.Maintenance.Proveedor", "Proveedor")
                        .WithMany()
                        .HasForeignKey("ProveedorId");

                    b.HasOne("AutoCosting.Models.Maintenance.Trabajo", "Trabajo")
                        .WithMany()
                        .HasForeignKey("TrabajoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AutoCosting.Models.Tracking.TrackingHeader", "Parent")
                        .WithMany("TrackingDetails")
                        .HasForeignKey("TrackingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutoCosting.Models.Tracking.TrackingHeader", b =>
                {
                    b.HasOne("AutoCosting.Models.Maintenance.Vehiculo", "Vehiculo")
                        .WithMany()
                        .HasForeignKey("VINVehiculo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AutoCosting.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AutoCosting.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AutoCosting.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AutoCosting.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
