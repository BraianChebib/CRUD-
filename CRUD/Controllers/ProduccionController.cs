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

        // 1. Pantalla Inicial: Listado de Pedidos de Planta 
        public async Task<IActionResult> Index()
        {
            
            var listaPedidos = await _context.Pedidos
                                             .Include(p => p.Usuario)
                                             .OrderByDescending(p => p.Fecha)
                                             .ToListAsync();

            return View(listaPedidos); 
        }

        // 2. Pantalla: Nuevo Pedido (Formulario de carga)
        public async Task<IActionResult> CrearPedido()
        {
            var articulos = await _context.Articulos.ToListAsync();
            return View(articulos); 
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPedido([FromBody] CrearPedidoDTO modelo)
        {
            if (modelo == null || !modelo.Detalles.Any())
            {
                return BadRequest(new { mensaje = "El pedido no contiene artículos." });
            }

            try
            {
                // 1. Creamos la cabecera del Pedido usando tu modelo nativo
                var nuevoPedido = new Pedido
                {
                    UsuarioId = modelo.UsuarioId,
                    Fecha = DateTime.Now, // Cambia a DateTime.UtcNow si preferís guardar en UTC
                    Estado = "Pendiente"  // Alineado con tu cambio del Index
                };

                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync(); // 🔑 MySQL guarda y genera el ID automáticamente

                // 2. Iteramos los detalles temporales que vinieron de la tabla de JS
                foreach (var detalle in modelo.Detalles)
                {
                    var nuevoDetalle = new PedidoDetalle
                    {
                        PedidoId = nuevoPedido.Id, // Usamos el ID recién generado arriba
                        ArticuloId = detalle.ArticuloId,
                        Cantidad = detalle.Cantidad
                    };
                    _context.PedidoDetalles.Add(nuevoDetalle);
                }

                await _context.SaveChangesAsync(); // Guardamos todos los renglones en PedidoDetalles

                return Ok(new { success = true, mensaje = "Pedido guardado con éxito." });
            }
            catch (Exception ex)
            {
                // Devolvemos el error en caso de que falle alguna restricción de la base de datos
                return StatusCode(500, new { mensaje = "Error interno en el servidor: " + ex.Message });
            }
        }

        // 3. Pantalla: Modificar Pedido (Formulario de edición)
        public IActionResult Editar(int id)
        {
            return View();
        }
    }
}