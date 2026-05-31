using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using CRUD.Services;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Esto hace que la ruta base sea: api/auth
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        // Le pedimos a ASP.NET que nos pase el servicio que creamos antes
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")] // La ruta final para el login será: api/auth/login
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Ejecutamos la validación que programaste en el Servicio
            var resultado = _authService.ValidarUsuario(request);

            if (resultado.Success)
            {
                // Si todo está bien, devolvemos un estado 200 (Ok) con el Nombre y Rol
                return Ok(resultado);
            }

            // Si falló (DNI inexistente o clave mal), devolvemos un estado 400 (Bad Request) con el error
            return BadRequest(new { mensaje = resultado.Mensaje });
        }
    }
}
