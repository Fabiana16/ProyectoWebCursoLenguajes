using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoWebCursoLenguajes.Data;
using ProyectoWebCursoLenguajes.Models;

namespace ProyectoWebCursoLenguajes.Controllers
{
    public class ProveedorProductoController : Controller
    {
        private readonly ProyectoWebCursoLenguajesContext _context;

        public ProveedorProductoController(ProyectoWebCursoLenguajesContext context)
        {
            _context = context;
        }

        // GET: ProveedorProductoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProveedorProducto.ToListAsync());
        }

        // GET: ProveedorProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.ProveedorProducto
                .FirstOrDefaultAsync(m => m.idProveedor == id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }

            return View(proveedorProducto);
        }

        // GET: ProveedorProductoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProveedorProductoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create([Bind("idProveedor,idProducto")] ProveedorProducto proveedorProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedorProducto);
                _context.SaveChangesAsync();  
            }
            
        }

        // GET: ProveedorProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.ProveedorProducto.FindAsync(id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }
            return View(proveedorProducto);
        }

        // POST: ProveedorProductoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProveedor,idProducto")] ProveedorProducto proveedorProducto)
        {
            if (id != proveedorProducto.idProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedorProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorProductoExists(proveedorProducto.idProveedor))
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
            return View(proveedorProducto);
        }

        // GET: ProveedorProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.ProveedorProducto
                .FirstOrDefaultAsync(m => m.idProveedor == id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }

            return View(proveedorProducto);
        }

        // POST: ProveedorProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedorProducto = await _context.ProveedorProducto.FindAsync(id);
            _context.ProveedorProducto.Remove(proveedorProducto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorProductoExists(int id)
        {
            return _context.ProveedorProducto.Any(e => e.idProveedor == id);
        }
    }
}
