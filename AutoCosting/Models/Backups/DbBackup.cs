using AutoCosting.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models.Backups
{
    public class DbBackup
    {
        [Display(Name = "Ruta")]
        public string Path { get; set; }
        [Display(Name = "Nombre del archivo")]
        public string Name { get; set; }
        public Empresa Empresa { get; set; }
        [Display(Name= "Archivo de Respaldo")]
        public string RestoreFilePath { get; set; }
    }
}
