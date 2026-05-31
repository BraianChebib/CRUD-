using CRUD.Data;
using CRUD.Models;
using System.Linq;

namespace CRUD.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public UserSession ValidarUsuario(LoginRequest datosLogin)
        {
            
            var usuarioEncontrado = _context.Users
                .FirstOrDefault(u => u.Dni == datosLogin.Dni);

            if (usuarioEncontrado == null)
            {
                return new UserSession { Success = false, Mensaje = "El DNI no está registrado." };
            }


            if (usuarioEncontrado.Clave != datosLogin.Clave)
            {
                return new UserSession { Success = false, Mensaje = "Contraseña incorrecta." };
            }


            return new UserSession
            {
                Success = true,
                Nombre = usuarioEncontrado.Nombre,
                Rol = usuarioEncontrado.Rol
            };
        }
    }

    // Esta clase nos ayuda a empaquetar la respuesta que le daremos al controlador
    public class UserSession
    {
        public bool Success { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}