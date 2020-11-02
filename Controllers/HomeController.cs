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

        public IActionResult Privacy()
        {
            return View();
        }

        public JsonResult ConsultarNotas()
        {
            return Json(db.Nota.ToList());
        }

        public JsonResult CrearNota(string titulo, string texto)
        {
            Nota nuevaNota = new Nota{
                Titulo = titulo,
                Cuerpo = texto
            };

            db.Nota.Add(nuevaNota);
            db.SaveChanges();
            return Json(nuevaNota);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
