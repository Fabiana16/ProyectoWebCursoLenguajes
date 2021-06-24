using Microsoft.AspNetCore.Mvc;
using ProyectoWebCursoLenguajes.Data;
using ProyectoWebCursoLenguajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Controllers
{
    public class VentaController : Controller
    {
      

        //variable para usar la api clientes
        private ClienteAPI clienteApi;

        //variable para usar el modelo cliente
        public static Cliente cliente;

        //variable para usar el contexto
        ProyectoWebCursoLenguajesContext cnt;
        public VentaController(ProyectoWebCursoLenguajesContext context)
        {
            this.cnt = context;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
