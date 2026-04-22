using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;

namespace TactiqApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JugadoresController : ControllerBase
    {
        private readonly TactiqDbContext _context;

        public JugadoresController(TactiqDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jugadore>>> GetJugadores()
        {
            return await _context.Jugadores.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerStatsDTO>> GetJugador(int id)
        {
            var jugador = await _context.Database
                .SqlQuery<PlayerStatsDTO>($"SELECT id_jugador, nombre_jugador, dorsal, edad, imagen_jugador , posicion, total_lanzamientos, total_goles, exclusiones_2min, valoracion_total FROM v_totales_campo WHERE id_jugador = {id}")
                .FirstOrDefaultAsync();

            if (jugador == null)
            {
                return NotFound();
            }

            return Ok(jugador);
        }
    }
}
