using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context; 

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // SECCIÓN 1: PROVEEDORES
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Proveedores()
        {
            var proveedores = await _context.Proveedores
                .Where(p => p.Activo)
                .OrderBy(p => p.Id)
                .ToListAsync();

            return View(proveedores);
        }

        [HttpGet]
        public IActionResult CrearProveedor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProveedor(Proveedor proveedor)
        {
            proveedor.Activo = true;
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Proveedores));
        }

        [HttpGet]
        public async Task<IActionResult> EditarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null || !proveedor.Activo) return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProveedor(Proveedor model)
        {
            var proveedor = await _context.Proveedores.FindAsync(model.Id);
            if (proveedor == null) return NotFound();

            proveedor.Nombre = model.Nombre;
            proveedor.RazonSocial = model.RazonSocial;
            proveedor.Cuit = model.Cuit;
            proveedor.Mail = model.Mail;
            proveedor.Telefono = model.Telefono;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Proveedores));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            proveedor.Activo = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Proveedores));
        }

        // ==========================================
        // SECCIÓN 2: ARTÍCULOS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Articulos()
        {
            var articulos = await _context.Articulos
                .Where(a => a.Activo)
                .OrderBy(a => a.Id)
                .ToListAsync();

            return View(articulos);
        }

        [HttpGet]
        public IActionResult CrearArticulo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearArticulo(Articulo articulo)
        {
            articulo.Activo = true;
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Articulos));
        }

        [HttpGet]
        public async Task<IActionResult> EditarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null || !articulo.Activo) return NotFound();

            return View(articulo);
        }

        [HttpPost]
        public async Task<IActionResult> EditarArticulo(Articulo model)
        {
            var articulo = await _context.Articulos.FindAsync(model.Id);
            if (articulo == null) return NotFound();

            articulo.Nombre = model.Nombre;
            articulo.Descripcion = model.Descripcion;
            articulo.UnidadMedida = model.UnidadMedida;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Articulos));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarArticulo(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null) return NotFound();

            articulo.Activo = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Articulos));
        }

        // ==========================================
        // SECCIÓN 3: USUARIOS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _context.Users 
                .Where(u => u.Activo)
                .OrderBy(u => u.Id)
                .ToListAsync();

            return View(usuarios);
        }

        [HttpGet]
        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(User usuario)
        {
            usuario.Activo = true;
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Usuarios));
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null || !usuario.Activo) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(User model)
        {
            var usuario = await _context.Users.FindAsync(model.Id);
            if (usuario == null) return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Dni = model.Dni;
            usuario.Rol = model.Rol;

            // Solo actualiza la clave si se escribió una nueva en el formulario
            if (!string.IsNullOrWhiteSpace(model.Clave))
            {
                usuario.Clave = model.Clave;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Usuarios));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.Activo = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Usuarios));
        }
    }
}