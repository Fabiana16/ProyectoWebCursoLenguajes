﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoWebCursoLenguajes.Models
{
    public class Usuario
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }

        public string login { get; set; }

        public string nombre { get; set; }

        public string password { get; set; }

        public string email { get; set; }
    }
}
