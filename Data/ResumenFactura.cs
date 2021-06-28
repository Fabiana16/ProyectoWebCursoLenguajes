using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Data
{
    public class ResumenFactura
    {
        public int idCliente { get; set; }
        public DateTime fecha { get; set; }
        public int idFactura { get; set; }
        public string cedula { get; set; }
        public string nombreCompleto { get; set; }
        public int telefono { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public int idUsuario{ get; set; }
        public decimal subtotal { get; set; }
        public decimal montoTotal { get; set; }
        public int cantidad { get; set; }
        public decimal descuento { get; set; }
        public decimal porcentajeImpuesto { get; set; }
        public string metodoPago { get; set; }

        public int numeroCheque { get; set; }
        public string banco { get; set; }


    }
}
