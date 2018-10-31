using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoCosting.Data;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoCosting.Areas.Identity.Pages.Account
{
    [AllowAnonymous]//[Authorize]//
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _db;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El email es requerido.")]
            [EmailAddress(ErrorMessage = "La dirección de email no es válida.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida.")]
            [StringLength(100, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Password", ErrorMessage = "La Contraseña y la confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }
                        
            [Display(Name = "Empleado")]
            public string EmpleadoID { get; set; }

            [Display(Name = "Rol")]
            [Required(ErrorMessage = "El Rol es requerido.")]
            public string RolId { get; set; }

        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IEnumerable<string> rolesList;
            if (this._db.Users.Count() == 0)
            {
                rolesList = new List<string>() { SD.AdminEndUser };
            }
            else
            {
                rolesList = new List<string>() {  SD.SalesAgentUser, SD.AdminEndUser };
            }        
            ViewData["Empleado"] = new SelectList(_db.Empleados.Where(e => !this._db.Users.Any(u => u.EmpleadoID == e.Cedula)), "Cedula", "NombreCompleto");
            ViewData["Roles"] = new SelectList(rolesList);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, EmpleadoID = Input.EmpleadoID };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(SD.SalesAgentUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.SalesAgentUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.AdminEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
                    }
                    if (Input.RolId.Equals(SD.AdminEndUser) || this._db.Users.Count() == 1)
                    {
                        await _userManager.AddToRoleAsync(user, SD.AdminEndUser);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.SalesAgentUser);
                    }
                    _logger.LogInformation("El usuario creó una nueva cuenta con contraseña.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirmación de correo electrónico",
                        $"Por favor confirme su cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click aqui</a>.");
                    if (!User.IsInRole(SD.AdminEndUser))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            OnGet();
            return Page();
        }
    }
}
