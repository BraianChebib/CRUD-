using CRUD.Models;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/cotizaciones")]
    public class CotizacionController : ControllerBase
    {
        private readonly CotizacionService _service;

        public CotizacionController(CotizacionService service)
        {
            _service = service;
        }

        // GET: api/cotizaciones/pedido/5
        [HttpGet("pedido/{pedidoId}")]
        public IActionResult GetByPedido(int pedidoId)
        {
            return Ok(_service.GetByPedidoId(pedidoId));
        }

        // POST: api/cotizaciones
        [HttpPost]
        public IActionResult Create(Cotizacion cotizacion)
        {
            var result = _service.Create(cotizacion);
            return Ok(result);
        }
    }
}