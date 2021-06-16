using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebCursoLenguajes.Models;

namespace ProyectoWebCursoLenguajes.Data
{
    public class ProyectoWebCursoLenguajesContext : DbContext
    {
        public ProyectoWebCursoLenguajesContext (DbContextOptions<ProyectoWebCursoLenguajesContext> options)
            : base(options)
        {
        }

        public DbSet<ProyectoWebCursoLenguajes.Models.Producto> Producto { get; set; }

        public DbSet<ProyectoWebCursoLenguajes.Models.Proveedor> Proveedor { get; set; }
    }
}
