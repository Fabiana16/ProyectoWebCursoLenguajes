using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Cliente
    {
        [Key]
        
        public int idCliente { get; set; }

        public string cedula { get; set; }

        public string nombreCompleto { get; set; }

        public int telefono { get; set; }

        public string direccion { get; set; }

        public string email { get; set; }
    }
}
