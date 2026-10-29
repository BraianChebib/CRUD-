using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    public class ComprasController : Controller
    {
        private readonly AppDbContext _context;

        public ComprasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Compras/Pedidos
        public async Task<IActionResult> Pedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: Compras/EvaluarCotizaciones/5
        public async Task<IActionResult> EvaluarCotizaciones(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return NotFound();

            // Traemos los proveedores reales de la BD y los pasamos a la vista
            ViewBag.Proveedores = await _context.Proveedores.ToListAsync();

            return View(pedido);
        }

        // POST: Compras/GenerarOrdenCompra
        [HttpPost]
        public async Task<IActionResult> GenerarOrdenCompra([FromBody] GenerarOCRequest req)
        {
            // 1. Buscamos la cotización seleccionada para sacar el precio
            var cotizacion = await _context.Cotizaciones.FindAsync(req.CotizacionId);
            if (cotizacion == null) return NotFound("Cotización no encontrada");

            // 2. Buscamos el ID del Proveedor
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.RazonSocial == cotizacion.RazonSocialProveedor);
            if (proveedor == null) return BadRequest("El proveedor no existe en la base de datos.");

            // 3. Buscamos el Pedido original para heredar el Usuario y actualizar su estado
            var pedido = await _context.Pedidos.FindAsync(req.PedidoId);
            if (pedido == null) return NotFound("Pedido no encontrado");

            // 4. Armamos la Orden de Compra mapeando EXACTAMENTE los campos de tu modelo
            var nuevaOC = new OrdenCompra
            {
                PedidoId = pedido.Id,
                ProveedorId = proveedor.Id,

                // Usamos el mismo usuario que hizo el pedido (o podés poner el ID del usuario logueado si ya lo tenés configurado)
                UsuarioId = pedido.UsuarioId,

                Total = (decimal)cotizacion.Precio, // Tomamos el precio total directo de la oferta ganadora

                // Como el tiempo en la cotización es un texto ("15 días"), le sumamos una fecha por defecto (ej: 7 días desde hoy)
                FechaEstimadaEntrega = DateTime.Now.AddDays(7),

                Estado = "Emitida" // O "Pendiente", según prefieras que nazca la OC
            };

            // 5. La agregamos a la base de datos
            _context.OrdenesCompra.Add(nuevaOC);

            // 6. Actualizamos el estado del pedido a "Completado" (o el estado que manejes cuando ya se compró)
            pedido.Estado = "Completado";

            // 7. Ahora sí, guardamos todo!
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    // Clase auxiliar (Va AFUERA del ComprasController, pero ADENTRO del namespace)
    public class GenerarOCRequest
    {
        public int PedidoId { get; set; }
        public int CotizacionId { get; set; }
    }
}