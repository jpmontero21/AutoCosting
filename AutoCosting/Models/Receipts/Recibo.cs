﻿using AutoCosting.Models.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Receipts
{
    [Table("tblRecibo")]
    public class Recibo
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El Número de Transacción es requerido")]
        [Display(Name = "Número de Transacción")]
        [DisplayFormat(DataFormatString = "{0:00000000}")]
        public int? TransID { get; set; }

        [Display(Name = "Transacción")]
        [ForeignKey("TransID")]
        public TransaccionHeader Parent { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El Monto del Abono es requerido.")]
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public double? Abono { get; set; }
        [Required(ErrorMessage = "La fecha es requerida.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha { get; set; }
    }
}
