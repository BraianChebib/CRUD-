using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Data; // 🔑 Namespace donde está tu AppDbContext
using CRUD.Models; // 🔑 Namespace donde están tus modelos (User, Pedido)

namespace CRUD.Controllers
{
    public class ProduccionController : Controller
    {
        private readonly AppDbContext _context;

        public ProduccionController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================================================
        // 1. PANTALLA INICIAL: LISTADO DE PEDIDOS DE PLANTA 
        // ==========================================================================
        public async Task<IActionResult> Index()
        {
            var listaPedidos = await _context.Pedidos
                                             .Include(p => p.Usuario)
                                             .OrderByDescending(p => p.Fecha)
                                             .ToListAsync();

            return View(listaPedidos);
        }

        // ==========================================================================
        // 2. PANTALLA: NUEVO PEDIDO (FORMULARIO DE CARGA)
        // ==========================================================================
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
                var nuevoPedido = new Pedido
                {
                    UsuarioId = modelo.UsuarioId,
                    Fecha = DateTime.Now,
                    Estado = "Pendiente"
                };

                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync();

                foreach (var detalle in modelo.Detalles)
                {
                    var nuevoDetalle = new PedidoDetalle
                    {
                        PedidoId = nuevoPedido.Id,
                        ArticuloId = detalle.ArticuloId,
                        Cantidad = detalle.Cantidad
                    };
                    _context.PedidoDetalles.Add(nuevoDetalle);
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true, mensaje = "Pedido guardado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno en el servidor: " + ex.Message });
            }
        }

        // ==========================================================================
        // 3. PANTALLA: MODIFICAR PEDIDO (FORMULARIO DE EDICIÓN)
        // ==========================================================================
        [HttpGet]
        [Route("Produccion/Editar/{id}")]
        public async Task<IActionResult> EditarPedido(int id)
        {
            // 🔑 Buscamos el pedido con sus relaciones completas para la pantalla de edición
            var pedido = await _context.Pedidos
                                       .Include(p => p.Usuario)
                                       .Include(p => p.Detalles)
                                           .ThenInclude(d => d.Articulo)
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            // 🔑 Catálogo para el buscador de la vista
            ViewBag.ArticulosDisponibles = await _context.Articulos.ToListAsync();

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPedido([FromBody] ActualizarPedidoDTO model)
        {
            if (model == null || model.PedidoId <= 0 || model.Detalles.Count == 0)
            {
                return Json(new { success = false, mensaje = "Los datos del pedido no son válidos o la lista está vacía." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var pedido = await _context.Pedidos
                                           .Include(p => p.Detalles)
                                           .FirstOrDefaultAsync(p => p.Id == model.PedidoId);

                if (pedido == null)
                {
                    return Json(new { success = false, mensaje = "El pedido que intenta modificar no existe." });
                }

                if (pedido.Estado != "Pendiente" && pedido.Estado != "En Revisión")
                {
                    return Json(new { success = false, mensaje = "Solo se pueden modificar pedidos en estado Pendiente." });
                }

                _context.PedidoDetalles.RemoveRange(pedido.Detalles);

                foreach (var item in model.Detalles)
                {
                    var nuevoDetalle = new PedidoDetalle
                    {
                        PedidoId = pedido.Id,
                        ArticuloId = item.ArticuloId,
                        Cantidad = (decimal)item.Cantidad
                    };

                    _context.PedidoDetalles.Add(nuevoDetalle);
                }
                pedido.UltimaModificacion = DateTime.Now; // Cambia el nombre si en tu modelo se llama distinto

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // 🔑 Corregido: Uso de Console.WriteLine para evitar error de compilación en C#
                Console.WriteLine("Error al actualizar pedido: " + ex.Message);
                return Json(new { success = false, mensaje = "Ocurrió un error interno en el servidor al guardar los cambios." });
            }
        }

    } // 👈 ACÁ CIERRA LA CLASE PRODUCCIONCONTROLLER

    // ==========================================================================
    // 🔑 LAS CLASES DTO QUEDAN DECLARADAS AFUERA DE LA CLASE PRINCIPAL
    // ==========================================================================
    public class ActualizarPedidoDTO
    {
        public int PedidoId { get; set; }
        public List<DetallePedidoDTO> Detalles { get; set; } = new List<DetallePedidoDTO>();
    }

    public class DetallePedidoDTO
    {
        public int ArticuloId { get; set; }
        public double Cantidad { get; set; }
    }
}