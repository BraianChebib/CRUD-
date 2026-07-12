using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using CRUD.Models;
using CRUD.Data; 
using System.Linq;

namespace CRUD.Controllers
{
    public class AccesoController : Controller
    {
        
        private readonly AppDbContext _context;

        public AccesoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuarioEncontrado = _context.Users
                .FirstOrDefault(u => u.Dni == model.Dni && u.Clave == model.Clave);

            if (usuarioEncontrado != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
            new Claim(ClaimTypes.Role, usuarioEncontrado.Rol) // Acá viaja el rol (ej: "Produccion")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // --- LÓGICA DE REDIRECCIÓN SEGÚN ROL ---
                if (usuarioEncontrado.Rol == "Produccion")
                {
                    return RedirectToAction("Index", "Produccion");
                }
                else
                {
                    // Compras, Admin y el resto van al Home vacío con la mancha de color
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "El DNI o la contraseña son incorrectos.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}