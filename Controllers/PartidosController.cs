using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TactiqApi.Models;
using TactiqApi.Models.DTOs;

namespace TactiqApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly TactiqDbContext _context;

        public PartidosController(TactiqDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene lista de todos los partidos de Zaragoza
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartidoDTO>>> GetPartidos()
        {
            try
            {
                var partidos = await _context.Partidos
                    .Where(p => p.IdEquipoLocal == 13 || p.IdEquipoVisitante == 13) // Zaragoza id = 13
                    .Join(
                        _context.Equipos,
                        p => p.IdEquipoLocal,
                        e => e.IdEquipo,
                        (p, e) => new { p, e })
                    .Join(
                        _context.Equipos,
                        x => x.p.IdEquipoVisitante,
                        e => e.IdEquipo,
                        (x, e2) => new { x.p, x.e, equipoVisitante = e2 })
                    .GroupJoin(
                        _context.Pabellones,
                        x => x.p.IdPabellon,
                        p => p.IdPabellon,
                        (x, pab) => new PartidoDTO
                        {
                            IdPartido = x.p.IdPartido,
                            IdTemporada = x.p.IdTemporada ?? 0,
                            Jornada = x.p.Jornada ?? 0,
                            Fecha = x.p.Fecha ?? DateTime.MinValue,
                            Hora = x.p.Hora,
                            Condicion = x.p.Condicion,
                            IdEquipoLocal = x.p.IdEquipoLocal ?? 0,
                            NombreEquipoLocal = x.e.NombreEquipo,
                            GolesLocal = x.p.GolesLocal ?? 0,
                            IdEquipoVisitante = x.p.IdEquipoVisitante ?? 0,
                            NombreEquipoVisitante = x.equipoVisitante.NombreEquipo,
                            GolesVisitante = x.p.GolesVisitante ?? 0,
                            IdPabellon = x.p.IdPabellon,
                            NombrePabellon = pab.FirstOrDefault() != null ? pab.FirstOrDefault().NombrePab : "Desconocido"
                        })
                    .OrderBy(p => p.Jornada)
                    .ToListAsync();

                return Ok(partidos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener partidos", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene detalles de un partido específico con todas las estadísticas de todos los jugadores
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetPartido(int id)
        {
            try
            {
                var partido = await _context.Partidos
                    .Where(p => p.IdPartido == id)
                    .FirstOrDefaultAsync();

                if (partido == null)
                    return NotFound(new { error = "Partido no encontrado" });

                // Obtener equipos
                var equipoLocal = await _context.Equipos.FirstOrDefaultAsync(e => e.IdEquipo == partido.IdEquipoLocal);
                var equipoVisitante = await _context.Equipos.FirstOrDefaultAsync(e => e.IdEquipo == partido.IdEquipoVisitante);
                var pabellon = await _context.Pabellones.FirstOrDefaultAsync(p => p.IdPabellon == partido.IdPabellon);

                // Obtener estadísticas de jugadores de Zaragoza en este partido
                var estadisticasJugadores = await _context.EstadisticasBases
                    .Where(eb => eb.IdPartido == id)
                    .Join(
                        _context.Jugadores.Where(j => j.IdEquipo == 13),
                        eb => eb.IdJugador,
                        j => j.IdJugador,
                        (eb, j) => new { estadsBase = eb, jugador = j })
                    .Join(
                        _context.Posicions,
                        x => x.jugador.IdPosicion,
                        p => p.IdPosicion,
                        (x, p) => new
                        {
                            x.estadsBase,
                            x.jugador,
                            posicion = p,
                        })
                    .Select(x => new
                    {
                        idJugador = x.jugador.IdJugador,
                        nombreJugador = x.jugador.NombreJugador,
                        dorsal = x.jugador.Dorsal,
                        posicion = x.posicion.RolEspecifico,
                        tiempoJugado = x.estadsBase.TiempoTot,
                        valoracion = x.estadsBase.ValoracionGlobal,
                        sanciones = new
                        {
                            amarillas = x.estadsBase.SancionAmarilla,
                            dos_minutos_1 = x.estadsBase.Sancion2mins1,
                            dos_minutos_2 = x.estadsBase.Sancion2mins2,
                            roja = x.estadsBase.SancionRoja,
                            azul = x.estadsBase.SancionAzul
                        }
                    })
                    .ToListAsync();

                return Ok(new
                {
                    idPartido = partido.IdPartido,
                    jornada = partido.Jornada,
                    fecha = partido.Fecha,
                    hora = partido.Hora,
                    pabellon = pabellon?.NombrePab,
                    equipoLocal = new
                    {
                        id = equipoLocal?.IdEquipo,
                        nombre = equipoLocal?.NombreEquipo,
                        goles = partido.GolesLocal
                    },
                    equipoVisitante = new
                    {
                        id = equipoVisitante?.IdEquipo,
                        nombre = equipoVisitante?.NombreEquipo,
                        goles = partido.GolesVisitante
                    },
                    estadisticasJugadores = estadisticasJugadores
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener partido", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un resumen de todos los partidos con resultados
        /// </summary>
        [HttpGet("resumen")]
        public async Task<ActionResult<dynamic>> GetResumenPartidos()
        {
            try
            {
                var partidos = await _context.Partidos
                    .Where(p => p.IdEquipoLocal == 13 || p.IdEquipoVisitante == 13)
                    .OrderBy(p => p.Jornada)
                    .Select(p => new
                    {
                        jornada = p.Jornada,
                        equipoLocal = _context.Equipos
                            .Where(e => e.IdEquipo == p.IdEquipoLocal)
                            .Select(e => e.NombreEquipo)
                            .FirstOrDefault(),
                        equipoVisitante = _context.Equipos
                            .Where(e => e.IdEquipo == p.IdEquipoVisitante)
                            .Select(e => e.NombreEquipo)
                            .FirstOrDefault(),
                        resultado = p.GolesLocal + "-" + p.GolesVisitante,
                        condicion = p.Condicion
                    })
                    .ToListAsync();

                var totalPartidos = partidos.Count();
                var golesAFavor = await _context.Partidos
                    .Where(p => (p.IdEquipoLocal == 13 && 1 == 1) || (p.IdEquipoVisitante == 13 && 1 == 1))
                    .Select(p => p.IdEquipoLocal == 13 ? p.GolesLocal : p.GolesVisitante)
                    .DefaultIfEmpty(0)
                    .SumAsync();

                return Ok(new
                {
                    totalPartidos = totalPartidos,
                    golesAFavor = golesAFavor,
                    partidos = partidos
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener resumen", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los partidos filtrados por jornada
        /// </summary>
        [HttpGet("jornada/{jornada}")]
        public async Task<ActionResult<IEnumerable<PartidoDTO>>> GetPartidosPorJornada(int jornada)
        {
            try
            {
                var partidos = await _context.Partidos
                    .Where(p => p.Jornada == jornada && (p.IdEquipoLocal == 13 || p.IdEquipoVisitante == 13))
                    .Join(
                        _context.Equipos,
                        p => p.IdEquipoLocal,
                        e => e.IdEquipo,
                        (p, e) => new { p, e })
                    .Join(
                        _context.Equipos,
                        x => x.p.IdEquipoVisitante,
                        e => e.IdEquipo,
                        (x, e2) => new { x.p, x.e, equipoVisitante = e2 })
                    .GroupJoin(
                        _context.Pabellones,
                        x => x.p.IdPabellon,
                        p => p.IdPabellon,
                        (x, pab) => new PartidoDTO
                        {
                            IdPartido = x.p.IdPartido,
                            IdTemporada = x.p.IdTemporada ?? 0,
                            Jornada = x.p.Jornada ?? 0,
                            Fecha = x.p.Fecha ?? DateTime.MinValue,
                            Hora = x.p.Hora,
                            Condicion = x.p.Condicion,
                            IdEquipoLocal = x.p.IdEquipoLocal ?? 0,
                            NombreEquipoLocal = x.e.NombreEquipo,
                            GolesLocal = x.p.GolesLocal ?? 0,
                            IdEquipoVisitante = x.p.IdEquipoVisitante ?? 0,
                            NombreEquipoVisitante = x.equipoVisitante.NombreEquipo,
                            GolesVisitante = x.p.GolesVisitante ?? 0,
                            IdPabellon = x.p.IdPabellon,
                            NombrePabellon = pab.FirstOrDefault() != null ? pab.FirstOrDefault().NombrePab : "Desconocido"
                        })
                    .ToListAsync();

                return Ok(partidos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener partidos", detalles = ex.Message });
            }
        }
    }
}