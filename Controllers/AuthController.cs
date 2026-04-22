using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;

namespace TactiqApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TactiqDbContext _context; 

        public AuthController(TactiqDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            /*
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Correo == request.Correo);

            if (user == null || user.Contrasena != request.Contrasena)
            {
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            }

            return Ok(new {
                id = user.IdUsuario,
                nombre = user.NombreUsuario,
                rol = user.IdRol
            });*/
            return Ok(new {
                id = 1,
                nombre = "usario de prueba",
                rol = 1
            });

        }
    }
}