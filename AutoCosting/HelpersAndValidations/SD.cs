using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.HelpersAndValidations
{
    public class SD
    {
        [Display(Name = "Administrador")]
        public const string AdminEndUser = "Admin";
        [Display(Name = "Agente de ventas")]
        public const string SalesAgentUser = "SalesAgent";
    }
}
