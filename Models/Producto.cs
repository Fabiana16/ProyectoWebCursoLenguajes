using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Producto
    {
		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("ID de Producto")]
		public int idProducto { get; set; }
		
		[DisplayName("Código de barra")]
		public int codigoBarra { get; set; }

		[DisplayName("Descripción")]
		public string descripcion { get; set; }
		
		[DisplayName("Precio de compra")]
		public decimal precioCompra { get; set; }

		[DisplayName("Porcentaje de impuesto")]
		public decimal porcentajeImpuesto { get; set; }
		
		[DisplayName("Unidad de medida")]
		public int unidadMedida { get; set; }
		
		[DisplayName("Precio de venta")]
		public decimal precioVenta { get; set; }

		[DisplayName("Estado")]
		public char estado { get; set; }
		
		[DisplayName("Categoría")]
		public string categoria { get; set; }

		[DisplayName("Imagen")]
		public string foto { get; set; }
    }
}
