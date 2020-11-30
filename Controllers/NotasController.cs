using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Notas.Models;
using System.Data.Common;

namespace Notas.Controllers
{
    public class NotasController : Controller
    {
        private readonly ILogger<NotasController> logger;
        private readonly NotasContext db;

        public NotasController(ILogger<NotasController> logger,
            NotasContext contexto)
        {
            this.logger = logger;
            this.db = contexto;
        }

        public IActionResult Editar(int ID)
        {
            Nota nota = db.Nota.FirstOrDefault(n => n.ID == ID);
            return View(nota);
        }

        [HttpPost]
        public IActionResult DoEdit(int ID, string titulo, string cuerpo)
        {
            Nota nota = db.Nota.FirstOrDefault(n => n.ID == ID);
            nota.Titulo = titulo;
            nota.Cuerpo = cuerpo;

            db.Nota.Update(nota);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Eliminar(int ID)
        {
            Nota nota = db.Nota.FirstOrDefault(n => n.ID == ID);
            
            db.Nota.Remove(nota);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public JsonResult TestSql(string search)
        {
            //var notas = db.Nota.FromSqlRaw("select * from Nota where titulo = {0}", search).ToList();
            var notas = db.Nota.FromSqlInterpolated($"select * from Nota where titulo like {"%" + search + "%"} or cuerpo like  {"%" + search + "%"}").ToList();

            //db.Nota.Include(n => n.Creador).Where(n => n.Creador.Nombre == "Juan");

            return Json(notas);
        }

        public JsonResult Test2Sql(string q)
        {
            int registrosAfectados = 0;
            //db.Database.ExecuteSqlCommand("delete from Nota");
            using(DbConnection conexion = db.Database.GetDbConnection())
            {
                conexion.Open();
                using(DbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = $"update nota set Cuerpo = '{q}'"; //podría ser un insert, update, delete, ...
                    registrosAfectados = comando.ExecuteNonQuery();        
                }
            }
                        
            return Json("Registros afectados:" + registrosAfectados.ToString());
        }

        public JsonResult Test3Sql(string q)
        {
            List<Nota> notas = new List<Nota>();
            using(DbConnection conexion = db.Database.GetDbConnection())
            {
                conexion.Open();
                using(DbCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = $"select * from nota where titulo like '%{q}%'";
                    using(DbDataReader reader = comando.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Nota nota = new Nota {
                                ID = Convert.ToInt32(reader["ID"]),
                                Titulo = reader["Titulo"].ToString(),
                                Cuerpo = reader["Cuerpo"].ToString(),
                                Creador = new Usuario {
                                    Mail = reader["CreadorMail"].ToString()
                                }
                            };
                            notas.Add(nota);
                        }
                    }
                }
            }
            return Json(notas);
        }        
    }
}