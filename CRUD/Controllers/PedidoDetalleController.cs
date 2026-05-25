using CRUD.Models;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/pedido-detalles")]
    public class PedidoDetalleController : ControllerBase
    {
        private readonly PedidoDetalleService _service;

        public PedidoDetalleController(PedidoDetalleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Create(PedidoDetalle detalle)
        {
            return Ok(_service.Create(detalle));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, PedidoDetalle detalle)
        {
            var result = _service.Update(id, detalle);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
