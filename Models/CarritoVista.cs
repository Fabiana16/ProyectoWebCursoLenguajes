using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class CarritoVista
    {

		public int idCarrito { get; set; }
		public int idProducto { get; set; }

		public int codigoBarra { get; set; }

		public string descripcion { get; set; }

		public decimal precioCompra { get; set; }

		public decimal porcentajeImpuesto { get; set; }

		public int unidadMedida { get; set; }

		public decimal precioVenta { get; set; }

		public char estado { get; set; }

		public string categoria { get; set; }

		public string foto { get; set; }

        public int cantidad { get; set; }

        public decimal subtotal { get; set; }

		public decimal subtotalIva { get; set; }

		public decimal subtotalEnvio { get; set; }

        public decimal precioFinal { get; set; }
    }
}
