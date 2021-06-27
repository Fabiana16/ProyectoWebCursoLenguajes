using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoWebCursoLenguajes.Data;
using ProyectoWebCursoLenguajes.Models;

using System.IO;

//importacion de libreria

using Microsoft.AspNetCore.Http;

namespace ProyectoWebCursoLenguajes.Controllers
{
    public class ProductoController : Controller
    {

        private readonly ProyectoWebCursoLenguajesContext _context;
        //VARIABLE PARA ALMACENAR LA FOTO ANTERIOR
        private static string fotoAnterior = "";

        public ProductoController(ProyectoWebCursoLenguajesContext context)
        {
            _context = context;
           
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producto.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.idProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("idProducto,codigoBarra,descripcion,precioCompra,porcentajeImpuesto,unidadMedida,precioVenta,estado,categoria,foto")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                long size = files.Sum(f => f.Length);

                string filePath = @"wwwroot\css\img\";
                string fileName = "";

                foreach (var formFile in files)
                {
                    //se valida el tamano del archivo
                    if (formFile.Length > 0)
                    {
                        //se construye el nombre de la foto con el codigo producto
                        fileName = producto.idProducto + "_" + formFile.FileName;

                        //aqui se quitan los espacios en blanco dentro del nombre de la foto
                        fileName = fileName.Replace(" ", "_");

                        // en la ruta fisica se agrega el nombre de la foto
                        filePath += fileName;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            //ahora le indicamos en nuestra db donde esta la foto
                            producto.foto = "/css/img/" + fileName;
                        } //using
                    }//if
                }// foreach


                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            fotoAnterior = producto.foto;
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProducto,codigoBarra,descripcion,precioCompra,porcentajeImpuesto," +
            "unidadMedida,precioVenta,estado")] Producto producto, List<IFormFile> files)
        {
            if (id != producto.idProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //se toma la ruta para borrar la foto anterior
                    string filePath = this.rutaFisicaBorrar();

                    //aqui se indica el nombre de la foto a borar
                    filePath += fotoAnterior;

                    //aqui se borra la foto anterior
                    this.borrarFoto(filePath);

                    //aqui le indicamos la ruta donde se guarda la foto nueva
                    filePath = this.rutaFisicaGuardar();

                    //variable para almacenar el nombre de la foto
                    string fileName = "";

                    //se revisa si el formulario tiene fotos adjuntas
                    foreach (var item in files)
                    {
                        if (item.Length > 0)
                        {
                            //aqui asignamos el id del producto con su nombre de foto
                            fileName = producto.idProducto + "_" + item.FileName;

                            //en caso que tenga espacios en blanco, lo quitamos
                            fileName = fileName.Replace(" ", "_");

                            //aqui indicamos el nombre de la nueva foto a guardar
                            filePath += fileName;

                            //se crea un objeto para guardar la foto
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                //aqui esperamos que se copie la nueva foto
                                await item.CopyToAsync(stream);

                                //aqui se indica el nombre de la nueva foto
                                producto.foto = "/css/img/" + fileName;
                            }//cierre del using
                        }//cierre del if
                    }// cierre del foreach
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.idProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }
        private string rutaFisicaBorrar()
        {
            
            string filePath = @"wwwroot";

            return filePath;
        }

        public string rutaFisicaGuardar()
        {

            string filePath = @"wwwroot\css\img\";

            return filePath;
        }

        private void borrarFoto(string pFileName)
        {
            try
            {
                //AQUI SE BORRA LA FOTO ANTERIOR DEL PRODUCTO
                System.IO.File.Delete(pFileName);
            }
            catch (Exception)
            {


            }

        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.idProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.idProducto == id);
        }

        public async Task<IActionResult> lineaBlanca()
        {

            return View(await this._context.Producto.ToListAsync());

        }

        public async Task<IActionResult> lineaTecnologica()
        {
            return View(await this._context.Producto.ToListAsync());
        }

        public async Task<IActionResult> lineaHogar()
        {
            return View(await this._context.Producto.ToListAsync());
        }

        public async Task<IActionResult> abarrotes()
        {
            return View(await this._context.Producto.ToListAsync());
        }

        public async Task<IActionResult> productosDisponibles()
        {
            return View(await this._context.Producto.ToListAsync());
        }
    }
}
