using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoCosting.Data;
using AutoCosting.HelpersAndValidations;
using AutoCosting.Models.Backups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AutoCosting.Controllers.Backups
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class DbBackupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public DbBackupController(IConfiguration config, ApplicationDbContext context)
        {
            this._config = config;
            this._context = context;
        }

        public IActionResult Index()
        {
            DbBackup dbBackup = new DbBackup();
            if (this._context.Empresa.Count() == 0)
            {
                return NotFound();
            }
            dbBackup.Empresa = this._context.Empresa.FirstOrDefault();
            dbBackup.Path = dbBackup.Empresa.DBBackupPath;
            dbBackup.Name = $"AutoCosting{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
            return View(dbBackup);
        }

        [HttpPost]
        public IActionResult Create(DbBackup dbBackup)
        {
            string connString = _config.GetConnectionString("DefaultConnection");            
            string route = dbBackup.Path;
            string name = dbBackup.Name;
            SqlConnection connection = new SqlConnection(connString);
            string strCommd = $@"BACKUP DATABASE [AutoCosting] to disk='{route}/{name}'";
            SqlCommand command = new SqlCommand(strCommd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            ViewData["CreateMessage"] = "Respaldo de base de datos creado con éxito.";

            //dbBackup.Name = $"AutoCosting{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
            ModelState["Name"].RawValue = $"AutoCosting{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
            return View(nameof(Index));
        }

        [HttpPost]
        public IActionResult Restore(DbBackup dbBackup)
        {
            try
            {
                dbBackup = new DbBackup() { Empresa = this._context.Empresa.FirstOrDefault() };
                string file = string.Empty;
                if (HttpContext.Request.Form.Files != null)
                {
                    IFormFileCollection files = HttpContext.Request.Form.Files;
                    file = ProcessBackupFile(files, dbBackup);
                }


                string connString = _config.GetConnectionString("DefaultConnection");
                SqlConnection.ClearAllPools();
                SqlConnection connection = new SqlConnection(connString);
                SqlCommand command = new SqlCommand($@"
                USE master
                ALTER DATABASE AutoCosting SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                RESTORE DATABASE AutoCosting from disk='{file}' WITH REPLACE", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                ViewData["RestoreMessage"] = "Restauración de datos completada con éxito.";
                dbBackup.Name = $"AutoCosting{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
                dbBackup.Path = dbBackup.Empresa.DBBackupPath;
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
            return View(nameof(Index),dbBackup);
        }

        private string ProcessBackupFile(IFormFileCollection files_, DbBackup dbBackup)
        {            
            var fileName = string.Empty;            
            IFormFileCollection files = files_;
            var file = files[0];
            if (file == null)
            {
                return string.Empty; 
            }
            fileName = Path.Combine(dbBackup.Empresa.DBBackupPath, file.FileName);

            if (!System.IO.File.Exists(fileName))
            {                
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var FileExtension = Path.GetExtension(fileName);
                    fileName = Path.Combine(dbBackup.Empresa.DBBackupPath, file.FileName);                    
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            return fileName;
        }
    }
}