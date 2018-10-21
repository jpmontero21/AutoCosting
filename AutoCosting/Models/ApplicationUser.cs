using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCosting.Models
{
    public class ApplicationUser : IdentityUser
    {
        //[Display(Name = "Nombre")]
        //public string FirstName { get; set; }
        //[Display(Name = "Apellidos")]
        //public string LastName { get; set; }
        [Display(Name = "Empleado")]
        public string EmpleadoID { get; set; }
        [Display(Name = "Teléfono")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        [Display(Name = "Usuario")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
    }
}
