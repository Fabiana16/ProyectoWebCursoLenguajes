using Microsoft.AspNetCore.Mvc;
using ProyectoWebCursoLenguajes.Data;
using ProyectoWebCursoLenguajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

using System.Net.Http.Json;

namespace ProyectoWebCursoLenguajes.Controllers
{

    public class VentaController : Controller
    {
        //Factura facturaForm;
        //variable para usar la api clientes
        private ClienteAPI clienteApi;

        //variable para usar el modelo cliente
        public static Cliente varClient;

        //variable para usar el contexto
        ProyectoWebCursoLenguajesContext cnt;

        public VentaController(ProyectoWebCursoLenguajesContext context)
        {
            this.cnt = context;

            //this.extraerCliente();
            //this.agregarCliente();

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //este metodo se tiene que terminar aun
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> traerClient([Bind("cedula,nombreCompleto,telefono,direccion,email,metodoPago,numeroCheque,banco")] ResumenFactura resumen)
        {
            Email email = new Email();
            var guardar = "";
            var id = 0;

            Cliente client = new Cliente();

            if (resumen.numeroCheque == 0)
            {
                resumen.numeroCheque = 0;
            }
            if (resumen.banco == null)
            {
                resumen.banco = "";
            }

            var url = "https://localhost:44332/api/clientes/agregar";
            var json = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            client.cedula = resumen.cedula;
            client.nombreCompleto = resumen.nombreCompleto;
            client.telefono = resumen.telefono;
            client.direccion = resumen.direccion;
            client.email = resumen.email;
            using (var httpclient = new HttpClient())
            {
                var response = await httpclient.PostAsJsonAsync(url, client);

                if (response.IsSuccessStatusCode)
                {
                    guardar = await response.Content.ReadAsStringAsync();
                    id = Int32.Parse(guardar);
                }
            }

            //this.facturaForm.idCliente = id;
            //this.facturaForm.idUsuario = 2;
            //this.facturaForm.metodoPago = resumen.metodoPago;
            //this.facturaForm.montoTotal = 20000;
            //this.facturaForm.descuento = 0;
            //cnt.Add(this.facturaForm);
            //cnt.SaveChanges();
            return View(resumen);
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

        [HttpGet]
        public ActionResult anadirCarrito(int? id)
        {
            try
            {
                var productoAgregado = cnt.Carrito.FirstOrDefault(m => m.idProducto == id);

                var producto = cnt.Producto.FirstOrDefault(m => m.idProducto == id);
                var categoriaVista = producto.categoria;
                if (productoAgregado == null)
                {
                    Carrito carrito = new Carrito();
                    carrito.idProducto = producto.idProducto;
                    carrito.codigoBarra = producto.codigoBarra;
                    carrito.descripcion = producto.descripcion;
                    carrito.precioCompra = producto.precioCompra;
                    carrito.porcentajeImpuesto = producto.porcentajeImpuesto;
                    carrito.unidadMedida = producto.unidadMedida;
                    carrito.precioVenta = producto.precioVenta;
                    carrito.estado = producto.estado;
                    carrito.categoria = producto.categoria;
                    carrito.foto = producto.foto;
                    cnt.Carrito.Add(carrito);
                    cnt.SaveChanges();
                }
                if (categoriaVista == "Linea Blanca")
                {
                    return RedirectToAction("lineaBlanca", "Producto");
                }
                if (categoriaVista == "Linea Hogar")
                {
                    return RedirectToAction("lineaHogar", "Producto");
                }
                if (categoriaVista == "Linea Tecnologica")
                {
                    return RedirectToAction("lineaTecnologica", "Producto");
                }
                if (categoriaVista == "Abarrotes")
                {
                    return RedirectToAction("abarrotes", "Producto");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult factura()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Carrito()
        {
            List<Carrito> carritoArray = cnt.Carrito.ToList();
            return View(carritoArray.ToList());
        }

        [HttpPost]
        public ActionResult Carrito(List<Carrito> productos)
        {
            List<Carrito> carritoArray = cnt.Carrito.ToList();

            foreach (Carrito carrito in carritoArray)
            {

                System.Diagnostics.Debug.WriteLine(carrito.unidadMedida);
                System.Diagnostics.Debug.WriteLine(carrito);
            }
            return RedirectToAction("Carrito", "Venta");
        }

        public IActionResult DeleteConfirmed(int? id)
        {
            var carrito = cnt.Carrito.FirstOrDefault(m => m.idProducto == id);
            cnt.Carrito.Remove(carrito);
            cnt.SaveChanges();
            return RedirectToAction(nameof(Carrito));
        }


        public IActionResult Calcular(int id)
        {
            int cantidad = id;

            return RedirectToAction(nameof(Carrito));
        }

        [HttpPost]
        public async Task<IActionResult> crearFatura(List<IFormFile> files, [Bind("idCliente, fecha, idFactura, cedula, nombreCompleto, telefono, direccion, email, idUsuario, subtotal, montoTotal, cantidad, descuento, porcentaje")] ResumenFactura resumenFactura)
        {
            await cnt.SaveChangesAsync();
            return View(resumenFactura);

        }


        public IActionResult verSubtotal() 
        {
            return View();
        }

    }
}




