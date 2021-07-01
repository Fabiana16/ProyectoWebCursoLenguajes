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

        //variable para usar la api clientes
        private ClienteAPI clienteApi;

        //variable para usar el modelo cliente
        public static Cliente varClient;

        //variable para usar el contexto
        ProyectoWebCursoLenguajesContext cnt;

        public VentaController(ProyectoWebCursoLenguajesContext context)
        {
            this.cnt = context;

            // this.extraerCliente(14);
            //this.agregarCliente();
            // enviarFactura(14);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        //metodo para traer un cliente de la api
        [HttpGet]
        public async Task<Cliente> extraerCliente(int id)
        {

            try
            {
                Cliente clienteReturn = new Cliente();
                //se instancia la api
                this.clienteApi = new ClienteAPI();

                //se obtiene el objeto de la api
                HttpClient cliente = this.clienteApi.Inicial();

                HttpResponseMessage response = await cliente.GetAsync("/api/clientes/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = response.Content.ReadAsStringAsync().Result;

                    varClient = JsonConvert.DeserializeObject<Cliente>(resultado);
                    clienteReturn = varClient;
                }
                return clienteReturn;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }//extraercliente

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
        }//metodo anadirCarrito




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
            Carrito carritoGuardar = new Carrito();
            for (int i = 0; i < carritoArray.Count; i++)
            {
                carritoArray[i].unidadMedida = productos[i].unidadMedida;
                carritoGuardar = carritoArray[i];
                cnt.Update(carritoGuardar);
                cnt.SaveChanges();
            }
            return RedirectToAction("verSubtotal", "Venta");
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
        }//calcular

        [HttpPost]
        public async Task<IActionResult> crearFatura(List<IFormFile> files, [Bind("idCliente, fecha, idFactura, cedula, nombreCompleto, telefono, direccion, email, idUsuario, subtotal, montoTotal, cantidad, descuento, porcentaje")] ResumenFactura resumenFactura)
        {
            await cnt.SaveChangesAsync();
            return View(resumenFactura);

        }//crear factura

        [HttpGet]
        public IActionResult verSubtotal()
        {
            List<Carrito> carritoArray = cnt.Carrito.ToList();
            List<CarritoVista> listaSubtotal = new List<CarritoVista>();


            foreach (var item in carritoArray)
            {
                CarritoVista carritoVista = new CarritoVista();
                carritoVista.descripcion = item.descripcion;
                carritoVista.idProducto = item.idProducto;
                carritoVista.precioCompra = item.precioCompra;
                carritoVista.cantidad = item.unidadMedida;
                carritoVista.subtotal = item.unidadMedida * item.precioCompra;
                listaSubtotal.Add(carritoVista);
            }

            return View(listaSubtotal.ToList());
        }//metodo ver subtotal

        //este metodo se tiene que terminar aun
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> traerClient([Bind("cedula,nombreCompleto,telefono,direccion,email,metodoPago,numeroCheque,banco")] ResumenFactura resumen)
        {
            var guardar = "";
            var id = 0;

            Cliente client = new Cliente();

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


                    ResumenFactura resumenVista = new ResumenFactura();

                    resumenVista.idCliente = id;
                    resumenVista.fecha = DateTime.UtcNow;
                    resumenVista.nombreCompleto = client.nombreCompleto;
                    resumenVista.cedula = client.cedula;
                    resumenVista.telefono = client.telefono;
                    resumenVista.direccion = client.direccion;
                    resumenVista.email = client.email;
                    resumenVista.metodoPago = resumen.metodoPago;
                    resumenVista.banco = "";
                    resumenVista.numeroCheque = 0;
                    if (resumen.metodoPago == "Cheque")
                    {
                        resumenVista.banco = resumen.banco;
                        resumenVista.numeroCheque = resumen.numeroCheque;

                    }
                    List<Carrito> carritoArray = cnt.Carrito.ToList();
                    decimal subtotal = 0;
                    int cantidadElementos = 0;
                    foreach (var item in carritoArray)
                    {
                        subtotal = subtotal + (item.unidadMedida * item.precioCompra);
                        cantidadElementos += item.unidadMedida;

                    }

                    resumenVista.subtotal = subtotal;

                    decimal descuento = 0;
                    decimal montoTotal = 0;
                    montoTotal = subtotal;
                    if (resumen.metodoPago == "Efectivo")//descuento
                    {
                        if (cantidadElementos >= 3 && cantidadElementos <= 6)
                        {
                            descuento = subtotal * 0.10m;
                            montoTotal = subtotal - descuento;

                        }
                        if (cantidadElementos >= 7 && cantidadElementos <= 9)
                        {
                            descuento = subtotal * 0.15m;
                            montoTotal = subtotal - descuento;
                        }
                        if (cantidadElementos >= 10 && cantidadElementos <= 12)
                        {
                            descuento = subtotal * 0.20m;
                            montoTotal = subtotal - descuento;
                        }
                        if (cantidadElementos >= 13)
                        {
                            descuento = subtotal * 0.25m;
                            montoTotal = subtotal - descuento;
                        }
                    }


                    montoTotal = montoTotal + (montoTotal * 0.13m);//iva
                    decimal iva = montoTotal * 0.13m;
                    montoTotal = montoTotal + (montoTotal * 0.02m);//imp envio
                    decimal envio = montoTotal * 0.02m;
                    resumenVista.montoTotal = montoTotal;
                    resumenVista.descuento = descuento;
                    resumenVista.impEnvio = envio;
                    resumenVista.porcentajeImpuesto = iva;
                    resumenVista.cantidad = cantidadElementos;

                    return View(resumenVista);

                }
            }

            return View(resumen);
        }//metodo traer cliente

        [HttpPost]
        public void enviarFactura([Bind("idCliente, metodoPago, banco, numeroCheque")] ResumenFactura cliente)
        {
            Factura factura = new Factura();
            FacturaDetalle detalle = new FacturaDetalle();
            Cliente clientep = new Cliente();

            Task<Cliente> task1 = extraerCliente(cliente.idCliente);

            factura.idCliente = task1.Result.idCliente;
            factura.idUsuario = 2;
            factura.metodoPago = cliente.metodoPago;

            List<Carrito> carritoArray = cnt.Carrito.ToList();
            decimal subtotal = 0;
            int cantidadElementos = 0;
            decimal descuento = 0;
            decimal montoTotal = 0;
            int numeroCheque = 0;
            string banco = "";
            if (cliente.metodoPago == "Cheque")
            {
                banco = cliente.banco;
                numeroCheque = cliente.numeroCheque;
            }

            factura.banco = banco;
            factura.numeroCheque = numeroCheque;

            foreach (var item in carritoArray)
            {
                subtotal = subtotal + (item.unidadMedida * item.precioCompra);
                cantidadElementos += item.unidadMedida;

            }
            montoTotal = subtotal;
            if (cliente.metodoPago == "Efectivo")//descuento
            {
                if (cantidadElementos >= 3 && cantidadElementos <= 6)
                {
                    descuento = subtotal * 0.10m;
                    montoTotal = subtotal - descuento;

                }
                if (cantidadElementos >= 7 && cantidadElementos <= 9)
                {
                    descuento = subtotal * 0.15m;
                    montoTotal = subtotal - descuento;
                }
                if (cantidadElementos >= 10 && cantidadElementos <= 12)
                {
                    descuento = subtotal * 0.20m;
                    montoTotal = subtotal - descuento;
                }
                if (cantidadElementos >= 13)
                {
                    descuento = subtotal * 0.25m;
                    montoTotal = subtotal - descuento;
                }
            }
            factura.descuento = descuento;
            montoTotal = montoTotal + (montoTotal * 0.13m);//iva
            decimal iva = montoTotal * 0.13m;
            montoTotal = montoTotal + (montoTotal * 0.02m);//imp envio
            decimal envio = montoTotal * 0.02m;
            factura.montoTotal = montoTotal;
            cnt.Factura.Add(factura);
            cnt.SaveChanges();


        }//enviar factura
    }//fin de clase
}//fin de namespace



    








