using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProyectoWebCursoLenguajes.Models
{
    public class ProveedorProducto
    {
        [Key]
        public int idProveedor { get; set; }

        public int idProducto { get; set; }
    }
}
