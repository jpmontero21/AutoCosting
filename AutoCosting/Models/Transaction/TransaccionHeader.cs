﻿using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.Maintenance;
using AutoCosting.Models.Receipts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Transaction
{
    [Table("tblTransHeader")]
    public class TransaccionHeader
    {
        [Key]
        [Display(Name = "Número de Transacción")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int TransID { get; set; }

        [ForeignKey("VendedorID")]
        public Empleado Empleado { get; set; }
        [Display(Name = "Vendedor")]
        [Required(ErrorMessage = "El vendedor es requerido")]
        public string VendedorID { get; set; }

        [ForeignKey("ClienteID")]
        public Cliente Cliente { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El Cliente es requerido.")]
        public int ClienteID { get; set; }

        [ForeignKey("SedeID,EmpresaID")]
        public Sede Sede { get; set; }
        [Display(Name = "Sede")]
        public int SedeID { get; set; }
        public int EmpresaID { get; set; }

        [Display(Name = "Tipo de Pago")]
        [Required(ErrorMessage = "El Tipo de pago es requerido.")]
        public TipoPago TipoPago { get; set; }
        [Display(Name = "Tipo de Transacción")]
        [Required(ErrorMessage = "El Tipo de transacción es requerido.")]
        public TipoTransaccion TipoTransaccion { get; set; }
        [Required(ErrorMessage = "La Fecha es requerida.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha { get; set; }
        [NotMapped]
        public string FechaStr
        {
            get
            {
                return this.Fecha.GetValueOrDefault(DateTime.Today).ToString("dd/MM/yyyy");
            }
        }
        [NotMapped]
        [Display(Name = "Apartado Vencido")]
        public bool ApartadoVencidoYN
        {
            get
            {
                if (this.TipoTransaccion == TipoTransaccion.Apartado)
                {
                    return DateTime.Today > Fecha.GetValueOrDefault(DateTime.Today).AddDays(30);//30 De reserva.
                }
                return false;
            }
        }

        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double? Saldo
        {
            get
            {
                return (this.Recibos == null) ? 0 : this.Total - this.Recibos.Sum(r => r.Abono);
            }
            set
            {
            }
        }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double? Total
        {
            get
            {
                double? sum = 0;
                if (this.TransDetails != null)
                    foreach (TransaccionDetail detail in this.TransDetails)
                        sum += detail.PrecioAcordado;
                return sum;
            }
        }

        public bool Eliminada { get; set; }
        public bool EnviadaHacienda { get; set; }
        public string ClaveHacienda { get; set; }
        public string AceptadaHacienda { get; set; }
        public string ConsecutivoHacienda { get; set; }

        public IEnumerable<TransaccionDetail> TransDetails { get; set; }

        public IEnumerable<Recibo> Recibos { get; set; }

        [NotMapped]
        public string TransIdStr
        {
            get => this.TransID.ToString("00000000");
        }

        [NotMapped]
        public string IndexInfo
        {
            get
            {
                string nombre = this.Cliente != null ? this.Cliente.Nombre : string.Empty;
                string apellido1 = this.Cliente != null ? this.Cliente.Apellido1 : string.Empty;
                return $" {this.TransIdStr} {this.TipoTransaccion} - {nombre} {apellido1} ";
            }
        }
    }
}
