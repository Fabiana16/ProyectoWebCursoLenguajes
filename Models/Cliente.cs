using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Cliente
    {
        [key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCliente { get; set; }

        public string cedula { get; set; }

        public string nombreCompleto { get; set; }

        public int telefono { get; set; }

        public string direccion { get; set; }

        public string email { get; set; }
    }
}
