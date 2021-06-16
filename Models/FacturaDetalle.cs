using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class FacturaDetalle
    {
        public int idFactura { get; set; }

        public int idProducto { get; set; }

        public int cantidad { get; set; }

        public decimal subtotal { get; set; }

    }
}
