using CRUD.Models;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/articulos")]
    public class ArticuloController : ControllerBase
    {
        private readonly ArticuloService _service;

        public ArticuloController(ArticuloService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Create(Articulo articulo)
        {
            return Ok(_service.Create(articulo));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Articulo articulo)
        {
            var result = _service.Update(id, articulo);
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
