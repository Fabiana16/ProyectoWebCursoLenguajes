using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Factura
    {
        [key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idFactura { get; set; }

        public int idCliente { get; set; }

        public int idUsuario { get; set; }

        public decimal montoTotal { get; set; }

        public string metodoPago { get; set; }

        public decimal descuento { get; set; }
    }
}
