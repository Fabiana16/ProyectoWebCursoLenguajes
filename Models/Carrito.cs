using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
	public class Carrito
	{
		[Key]

		public int idCarrito { get; set; }
		public int idProducto { get; set; }

		public int codigoBarra { get; set; }

		public string descripcion { get; set; }

		public decimal precioCompra { get; set; }

		public decimal porcentajeImpuesto { get; set; }

		public int unidadMedida { get; set; }

		public decimal precioVenta { get; set; }

		public char estado { get; set; }
	}
}

