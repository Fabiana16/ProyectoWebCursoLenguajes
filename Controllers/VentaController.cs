using Microsoft.AspNetCore.Mvc;
using ProyectoWebCursoLenguajes.Data;
using ProyectoWebCursoLenguajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ProyectoWebCursoLenguajes.Controllers
{
    public class VentaController : Controller
    {
      

        //variable para usar la api clientes
        private ClienteAPI clienteApi;

        //variable para usar el modelo cliente
        public static Cliente varClient;

        //variable para usar el contexto
        ProyectoWebCursoLenguajesContext cnt;

        public VentaController(ProyectoWebCursoLenguajesContext context)
        {
            this.cnt = context;

            this.extraerCliente();
        }
        public IActionResult Index()
        {
            return View();
        }

        //metodo para traer un cliente de la api
        public async void extraerCliente()
        {

            try
            {
                //se instancia la api
                this.clienteApi = new ClienteAPI();

                //se obtiene el objeto de la api
                HttpClient cliente = this.clienteApi.Inicial();

                //aqui se extrae el usuario que se ocupa desde la api.
                //el 1 es el id del usuario que se esta pidiendo.
                HttpResponseMessage response = await cliente.GetAsync("api/clientes/1");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;

                    varClient = JsonConvert.DeserializeObject<Cliente>(resultado);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult Carrito()
        {
            return View();
        }

    }
}
