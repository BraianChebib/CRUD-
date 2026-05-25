using CRUD.Models;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/ordenes-compra")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly OrdenCompraService _service;

        public OrdenCompraController(OrdenCompraService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Create(OrdenCompra orden)
        {
            return Ok(_service.Create(orden));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, OrdenCompra orden)
        {
            var result = _service.Update(id, orden);
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
