using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;
using TactiqApi.Models.DTOs;

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

        /// <summary>
        /// Obtiene lista de todos los jugadores del equipo de Zaragoza (id_equipo = 13)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JugadorDTO>>> GetJugadores()
        {
            try
            {
                var jugadores = await _context.Jugadores
                    .Where(j => j.IdEquipo == 13) // Balonmano Zaragoza
                    .Join(
                        _context.Posicions,
                        j => j.IdPosicion,
                        p => p.IdPosicion,
                        (j, p) => new JugadorDTO
                        {
                            IdJugador = j.IdJugador,
                            NombreJugador = j.NombreJugador,
                            Dorsal = j.Dorsal,
                            ImagenJugador = j.ImagenJugador,
                            Posicion = p.Categoria,
                            RolEspecifico = p.RolEspecifico,
                            IdEquipo = j.IdEquipo ?? 0,
                            NombreEquipo = "Balonmano Zaragoza"
                        })
                    .ToListAsync();

                return Ok(jugadores);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener jugadores", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene detalles de un jugador específico
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<JugadorDTO>> GetJugador(int id)
        {
            try
            {
                var jugador = await _context.Jugadores
                    .Where(j => j.IdJugador == id && j.IdEquipo == 13)
                    .Join(
                        _context.Posicions,
                        j => j.IdPosicion,
                        p => p.IdPosicion,
                        (j, p) => new JugadorDTO
                        {
                            IdJugador = j.IdJugador,
                            NombreJugador = j.NombreJugador,
                            Dorsal = j.Dorsal,
                            ImagenJugador = j.ImagenJugador,
                            Posicion = p.Categoria,
                            RolEspecifico = p.RolEspecifico,
                            IdEquipo = j.IdEquipo ?? 0,
                            NombreEquipo = "Balonmano Zaragoza"
                        })
                    .FirstOrDefaultAsync();

                if (jugador == null)
                    return NotFound(new { error = "Jugador no encontrado" });

                return Ok(jugador);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener jugador", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene estadísticas totales de un jugador en toda la temporada
        /// Suma de todos sus partidos
        /// </summary>
        [HttpGet("{id}/estadisticas-temporada")]
        public async Task<ActionResult<dynamic>> GetEstadisticasTemporada(int id)
        {
            try
            {
                var estadisticas = await _context.EstadisticasCampos
                    .Where(ec => ec.IdBase != null)
                    .Join(
                        _context.EstadisticasBases.Where(eb => eb.IdJugador == id),
                        ec => ec.IdBase,
                        eb => eb.IdBase,
                        (ec, eb) => new { estads_campo = ec, estads_base = eb })
                    .GroupBy(x => x.estads_base.IdJugador)
                    .Select(g => new
                    {
                        idJugador = g.Key,
                        totalLanzamientos = g.Sum(x => x.estads_campo.TotalLanzamientos ?? 0),
                        totalGoles = g.Sum(x => x.estads_campo.TotalGoles ?? 0),
                        totalParadas = g.Sum(x => x.estads_campo.TotalParadas ?? 0),
                        totalPostes = g.Sum(x => x.estads_campo.TotalPostes ?? 0),
                        totalFuera = g.Sum(x => x.estads_campo.TotalFuera ?? 0),
                        m7Goles = g.Sum(x => x.estads_campo.M7Goles ?? 0),
                        contraataqueGoles = g.Sum(x => x.estads_campo.ContraataqueGoles ?? 0),
                        asistencias = g.Sum(x => x.estads_campo.ValoracionPositivaAsistencia ?? 0),
                        recuperaciones = g.Sum(x => x.estads_campo.ValoracionPositivaRecuperacion ?? 0),
                        sanciones2Mins = g.Sum(x => x.estads_base.Sancion2mins1 ?? 0),
                        sancionesRojas = g.Sum(x => x.estads_base.SancionRoja ?? 0),
                        porcentajeGol = g.Sum(x => x.estads_campo.TotalGoles ?? 0) > 0 
                            ? Math.Round(((decimal)g.Sum(x => x.estads_campo.TotalGoles ?? 0) / g.Sum(x => x.estads_campo.TotalLanzamientos ?? 0) * 100), 2)
                            : 0
                    })
                    .FirstOrDefaultAsync();

                if (estadisticas == null)
                    return NotFound(new { error = "No hay estadísticas para este jugador" });

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener estadísticas", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene todas las estadísticas de un jugador en un partido específico
        /// </summary>
        [HttpGet("{idJugador}/partidos/{idPartido}")]
        public async Task<ActionResult<EstadisticasJugadorDTO>> GetEstadisticasPartido(int idJugador, int idPartido)
        {
            try
            {
                // Obtener jugador
                var jugador = await _context.Jugadores
                    .Where(j => j.IdJugador == idJugador)
                    .FirstOrDefaultAsync();

                if (jugador == null)
                    return NotFound(new { error = "Jugador no encontrado" });

                // Obtener estadísticas base
                var estadsBase = await _context.EstadisticasBases
                    .Where(eb => eb.IdJugador == idJugador && eb.IdPartido == idPartido)
                    .FirstOrDefaultAsync();

                if (estadsBase == null)
                    return NotFound(new { error = "Sin estadísticas en este partido" });

                // Obtener información del partido
                var partido = await _context.Partidos
                    .Where(p => p.IdPartido == idPartido)
                    .FirstOrDefaultAsync();

                // Obtener posición del jugador
                var posicion = await _context.Posicions
                    .Where(p => p.IdPosicion == jugador.IdPosicion)
                    .FirstOrDefaultAsync();

                // Construir DTO
                var dto = new EstadisticasJugadorDTO
                {
                    IdJugador = idJugador,
                    NombreJugador = jugador.NombreJugador,
                    Dorsal = jugador.Dorsal,
                    Posicion = posicion?.Categoria ?? "Desconocida",
                    IdPartido = idPartido,
                    Jornada = partido?.Jornada ?? 0,
                    FechaPartido = partido?.Fecha ?? DateTime.MinValue,
                    EquipoRival = GetEquipoRival(partido, jugador.IdEquipo).Result ?? "Desconocido",
                    TiempoDef = estadsBase.TiempoDef,
                    TiempoTot = estadsBase.TiempoTot,
                    ValoracionGlobal = estadsBase.ValoracionGlobal ?? 0,
                    SancionAmarilla = estadsBase.SancionAmarilla ?? 0,
                    Sancion2Mins1 = estadsBase.Sancion2mins1 ?? 0,
                    Sancion2Mins2 = estadsBase.Sancion2mins2 ?? 0,
                    SancionRoja = estadsBase.SancionRoja ?? 0,
                    SancionAzul = estadsBase.SancionAzul ?? 0
                };

                // Si es jugador de campo, añadir estadísticas de campo
                if (posicion?.Categoria == "Campo")
                {
                    var estadsCampo = await _context.EstadisticasCampos
                        .Where(ec => ec.IdBase == estadsBase.IdBase)
                        .FirstOrDefaultAsync();

                    if (estadsCampo != null)
                    {
                        dto.TotalLanzamientos = estadsCampo.TotalLanzamientos;
                        dto.TotalGoles = estadsCampo.TotalGoles;
                        dto.TotalParadas = estadsCampo.TotalParadas;
                        dto.TotalPostes = estadsCampo.TotalPostes;
                        dto.TotalFuera = estadsCampo.TotalFuera;
                        dto.TotalBloqueados = estadsCampo.TotalBloqueados;
                        dto.M7Lanzamientos = estadsCampo.M7Lanzamientos;
                        dto.M7Goles = estadsCampo.M7Goles;
                        dto.ContraataqueLanzamientos = estadsCampo.ContraataqueLanzamientos;
                        dto.ContraataqueGoles = estadsCampo.ContraataqueGoles;
                        dto.M9Lanzamientos = estadsCampo.M9Lanzamientos;
                        dto.M9Goles = estadsCampo.M9Goles;
                        dto.ExtremeLanzamientos = estadsCampo.ExtremeLanzamientos;
                        dto.ExtremeGoles = estadsCampo.ExtremeGoles;
                        dto.M6Lanzamientos = estadsCampo.M6Lanzamientos;
                        dto.M6Goles = estadsCampo.M6Goles;
                        dto.ValoracionPositivaAsistencia = estadsCampo.ValoracionPositivaAsistencia;
                        dto.ValoracionPositivaRecuperacion = estadsCampo.ValoracionPositivaRecuperacion;
                        dto.ValoracionNegativaPerdida = estadsCampo.ValoracionNegativaPerdida;
                        dto.ValoracionNegativaPasos = estadsCampo.ValoracionNegativaPasos;
                        dto.ValoracionNegativaDobles = estadsCampo.ValoracionNegativaDobles;
                        dto.ValoracionNegativaFaltaAtaque = estadsCampo.ValoracionNegativaFaltaAtaque;
                    }
                }

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener estadísticas", detalles = ex.Message });
            }
        }

        private async Task<string> GetEquipoRival(Partido partido, int? idEquipoZaragoza)
        {
            if (partido == null) return null;

            int idRival = partido.IdEquipoLocal == idEquipoZaragoza 
                ? partido.IdEquipoVisitante 
                : partido.IdEquipoLocal;

            var equipo = await _context.Equipos.FirstOrDefaultAsync(e => e.IdEquipo == idRival);
            return equipo?.NombreEquipo;
        }
    }
}
