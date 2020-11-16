using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notas.Models;

namespace Notas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly NotasContext db;

        public HomeController(ILogger<HomeController> logger,
            NotasContext contexto)
        {
            this.logger = logger;
            this.db = contexto;
        }

        public IActionResult Index()
        {
            return View(db.Nota.ToList());
        }

        public IActionResult Contact()
        {
            return View();
        } 

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult VistaConOtroLayout()
        {
            return View();
        }

        public JsonResult ConsultarNotas()
        {
            return Json(db.Nota.ToList());
        }

        public JsonResult CrearNota(string titulo, string texto)
        {
            Usuario usuario = HttpContext.Session.Get<Usuario>("UsuarioLogueado");
            if(usuario != null)
            {       
                Usuario usuarioBase = db.Usuario.FirstOrDefault(u => u.Mail.Equals(usuario.Mail));
                Nota nuevaNota = new Nota{
                    Titulo = titulo,
                    Cuerpo = texto,
                    Creador = usuarioBase
                };

                db.Nota.Add(nuevaNota);
                db.SaveChanges();
                return Json(nuevaNota);
            }
            else
            {
                return Json("No te dejo agregar la nota porque no estás logueado.");
            }
            
        }

        public JsonResult AgregarUsuarioALaSession(string mail, string nombre)
        {
            Usuario nuevoUsuario = new Usuario{
                Mail = mail, 
                Nombre = nombre
            };

            HttpContext.Session.Set<Usuario>("UsuarioLogueado", nuevoUsuario);
            return Json(nuevoUsuario);
        }

        public JsonResult ConsultarUsuarioEnSesion()
        {
            Usuario usuario = HttpContext.Session.Get<Usuario>("UsuarioLogueado");
            return Json(usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
