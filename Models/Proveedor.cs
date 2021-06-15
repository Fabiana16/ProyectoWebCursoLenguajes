﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Proveedor
    {
		[key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int idProveedor { get; set; }

		public string cedulaLegal { get; set; }

		public string nombreCompleto { get; set; }

		public int telefono { get; set; }

		public string direccionExacta { get; set; }

		public int contacto { get; set; }

		public string email { get; set; }
	}
}
