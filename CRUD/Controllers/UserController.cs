using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using CRUD.Services;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            return Ok(_service.Create(user));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            var result = _service.Update(id, user);
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