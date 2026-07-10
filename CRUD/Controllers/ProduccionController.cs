using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Data; // 🔑 Namespace donde está tu AppDbContext
using CRUD.Models; // 🔑 Namespace donde están tus modelos (User, Pedido)

namespace CRUD.Controllers
{
    public class ProduccionController : Controller
    {
        // 🔑 Cambiado de ApplicationDbContext a AppDbContext
        private readonly AppDbContext _context;

        // 🔑 El constructor ahora recibe tu contexto real
        public ProduccionController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Pantalla Inicial: Listado de Pedidos de Planta (CONECTADO Y CORREGIDO)
        public async Task<IActionResult> Index()
        {
            // Trae los pedidos de MySQL, incluye los datos del usuario (JOIN) y los ordena del más nuevo al más viejo
            var listaPedidos = await _context.Pedidos
                                             .Include(p => p.Usuario)
                                             .OrderByDescending(p => p.Fecha)
                                             .ToListAsync();

            return View(listaPedidos); // Le pasa la lista real a la vista Index.cshtml
        }

        // 2. Pantalla: Nuevo Pedido (Formulario de carga)
        public IActionResult Crear()
        {
            // Nota: Si en el futuro usas AppDbContext acá para cargar datos (ej. proveedores), 
            // ya lo tenés disponible usando _context
            return View();
        }

        // 3. Pantalla: Modificar Pedido (Formulario de edición)
        public IActionResult Editar(int id)
        {
            return View();
        }
    }
}