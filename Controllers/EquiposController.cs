using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;
using TactiqApi.Models.DTOs;

namespace TactiqApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquiposController : ControllerBase
    {
        private readonly TactiqDbContext _context;

        public EquiposController(TactiqDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipoDTO>>> GetEquipos()
        {
            try
            {
                var equipos = await _context.Equipos
                    .OrderBy(e => e.NombreEquipo)
                    .Select(e => new EquipoDTO
                    {
                        IdEquipo = e.IdEquipo,
                        NombreEquipo = e.NombreEquipo,
                        ImagenLogo = e.ImagenLogo,
                        Ciudad = e.Ciudad,
                        IdPabellon = e.IdPabellon
                    })
                    .ToListAsync();

                return Ok(equipos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener equipos", detalles = ex.Message });
            }
        }
    }
}