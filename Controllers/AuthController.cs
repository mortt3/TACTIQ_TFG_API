using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;
using TactiqApi.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Endpoint para login de usuarios
        /// Requiere correo y contraseña
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Contrasena))
                {
                    return BadRequest(new { error = "Correo y contraseña son requeridos" });
                }

                // Buscar usuario por correo
                var user = await _context.Users
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefaultAsync(u => u.Correo == request.Correo);

                if (user == null)
                {
                    return Unauthorized(new { error = "Usuario no encontrado" });
                }

                // Validar contraseña (aquí se podría usar hashing en producción)
                if (user.Contrasena != request.Contrasena)
                {
                    return Unauthorized(new { error = "Contraseña incorrecta" });
                }

                // Construir respuesta
                var response = new LoginResponseDTO
                {
                    IdUsuario = user.IdUsuario,
                    NombreUsuario = user.NombreUsuario,
                    Correo = user.Correo,
                    Rol = user.IdRolNavigation?.Rol ?? "Desconocido",
                    Mensaje = "Login exitoso"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error en login", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para verificar si el usuario está autenticado
        /// </summary>
        [HttpPost("verify")]
        public async Task<ActionResult> Verify([FromBody] int idUsuario)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

                if (user == null)
                {
                    return Unauthorized(new { error = "Usuario no encontrado" });
                }

                return Ok(new { mensaje = "Usuario verificado", usuario = user.NombreUsuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error en verificación", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para obtener información del usuario actual
        /// </summary>
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<dynamic>> GetUsuario(int id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.IdUsuario == id)
                    .Include(u => u.IdRolNavigation)
                    .Select(u => new
                    {
                        id = u.IdUsuario,
                        nombre = u.NombreUsuario,
                        correo = u.Correo,
                        rol = u.IdRolNavigation.Rol
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { error = "Usuario no encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener usuario", detalles = ex.Message });
            }
        }
    }
}