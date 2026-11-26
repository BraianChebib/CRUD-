using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Data; // Ajustá este namespace si tu DbContext está en otra carpeta
using CRUD.Models;

namespace CRUD.Controllers
{
    public class ContabilidadController : Controller
    {
        private readonly AppDbContext _context; // Usá el mismo nombre de DbContext que en ComprasController
        private readonly IWebHostEnvironment _env;

        public ContabilidadController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==========================================
        // SECCIÓN 1: APROBACIÓN DE ÓRDENES DE COMPRA
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Aprobaciones()
        {
            // Traemos todas las OC generadas por Compras (las Pendientes primero)
            var ordenes = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Usuario)
                .OrderByDescending(o => o.Estado == "Pendiente")
                .ThenByDescending(o => o.Id)
                .ToListAsync();

            return View(ordenes);
        }

        [HttpGet]
        public async Task<IActionResult> Revisar(int id)
        {
            var oc = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oc == null) return NotFound();

            // Traemos los artículos del pedido original para que Contabilidad audite el detalle
            ViewBag.DetallesPedido = await _context.PedidoDetalles
                .Include(d => d.Articulo)
                .Where(d => d.PedidoId == oc.PedidoId)
                .ToListAsync();

            return View(oc);
        }

        [HttpPost]
        public async Task<IActionResult> Aprobar(int id)
        {
            var oc = await _context.OrdenesCompra.FindAsync(id);
            if (oc == null) return NotFound();

            oc.Estado = "Aprobada";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Aprobaciones));
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(int id, string motivoRechazo)
        {
            var oc = await _context.OrdenesCompra.FindAsync(id);
            if (oc == null) return NotFound();

            oc.Estado = "Rechazada";
            oc.MotivoRechazo = motivoRechazo;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Aprobaciones));
        }

        // ==========================================
        // SECCIÓN 2: ÓRDENES PENDIENTES DE PAGO
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Pagos()
        {
            // Solo mostramos las que ya fueron recibidas en depósito o ya pagadas
            var ordenes = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Usuario)
                .Where(o => o.Estado == "Recibido" || o.Estado == "Pagada")
                .OrderByDescending(o => o.Estado == "Recibido")
                .ThenByDescending(o => o.Id)
                .ToListAsync();

            return View(ordenes);
        }

        [HttpGet]
        public async Task<IActionResult> Pagar(int id)
        {
            var oc = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oc == null) return NotFound();

            ViewBag.DetallesPedido = await _context.PedidoDetalles
                .Include(d => d.Articulo)
                .Where(d => d.PedidoId == oc.PedidoId)
                .ToListAsync();

            return View(oc);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPago(int id, string numeroPago, IFormFile? comprobanteArchivo)
        {
            var oc = await _context.OrdenesCompra.FindAsync(id);
            if (oc == null) return NotFound();

            oc.NumeroPago = numeroPago;

            // Si subió un archivo (PDF o imagen), lo guardamos en wwwroot/comprobantes
            if (comprobanteArchivo != null && comprobanteArchivo.Length > 0)
            {
                string carpeta = Path.Combine(_env.WebRootPath, "comprobantes");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string extension = Path.GetExtension(comprobanteArchivo.FileName);
                string nombreArchivo = $"Pago_OC-{oc.Id}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await comprobanteArchivo.CopyToAsync(stream);
                }

                oc.ComprobantePago = $"/comprobantes/{nombreArchivo}";
            }

            oc.Estado = "Pagada";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pagos));
        }
    }
}