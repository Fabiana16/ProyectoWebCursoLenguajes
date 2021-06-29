
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
        public async Task<ActionResult>  traerClient([Bind("cedula,nombreCompleto,telefono,direccion,email,metodoPago,numeroCheque,banco")] ResumenFactura resumen)
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
                Carrito carrito = new Carrito();
                var producto = cnt.Producto.FirstOrDefault(m => m.idProducto == id);
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
                return RedirectToAction("Index", "Home");

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
        //este metodo retorna la lista de objetos del carrito
        [HttpGet]
        public IActionResult Carrito()
        {
            CarritoVista carritoVista = new CarritoVista();
            List<Carrito> carritoArray = cnt.Carrito.ToList();
            List<CarritoVista> carritoVistaArray = new List<CarritoVista>();
            foreach (var item in carritoArray)
            {
                carritoVista.idCarrito = item.idCarrito;
                carritoVista.idProducto = item.idProducto;
                carritoVista.descripcion = item.descripcion;
                carritoVista.codigoBarra = item.codigoBarra;
                carritoVista.precioCompra = item.precioCompra;
                carritoVista.porcentajeImpuesto = item.porcentajeImpuesto;
                carritoVista.unidadMedida = item.unidadMedida;
                carritoVista.precioVenta = item.precioVenta;// podria considerar quitarse
                carritoVista.estado = item.estado;
                carritoVista.categoria = item.categoria;
                carritoVista.foto = item.foto;
                carritoVistaArray.Add(carritoVista);

            }
            //valores para la compra
            carritoVista.cantidad = 0;
            carritoVista.subtotal = 0;
            carritoVista.subtotalIva = 0;
            carritoVista.subtotalEnvio = 0;
            carritoVista.precioFinal = 0;
            carritoVistaArray.Add(carritoVista);

            return View(carritoVistaArray.ToList());
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

    }
}



    
