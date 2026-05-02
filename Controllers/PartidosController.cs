using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
                            IdTemporada = x.p.IdTemporada,
                            Jornada = x.p.Jornada,
                            Fecha = x.p.Fecha,
                            Hora = x.p.Hora,
                            Condicion = x.p.Condicion,
                            IdEquipoLocal = x.p.IdEquipoLocal,
                            NombreEquipoLocal = x.e.NombreEquipo,
                            GolesLocal = x.p.GolesLocal,
                            IdEquipoVisitante = x.p.IdEquipoVisitante,
                            NombreEquipoVisitante = x.equipoVisitante.NombreEquipo,
                            GolesVisitante = x.p.GolesVisitante,
                            IdPabellon = x.p.IdPabellon,
                            NombrePabellon = pab.FirstOrDefault() != null ? pab.FirstOrDefault()!.NombrePab : "Desconocido"
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
                        goles = partido.GolesLocal,
                        imagenLogo = equipoLocal?.ImagenLogo
                    },
                    equipoVisitante = new
                    {
                        id = equipoVisitante?.IdEquipo,
                        nombre = equipoVisitante?.NombreEquipo,
                        goles = partido.GolesVisitante,
                        imagenLogo = equipoVisitante?.ImagenLogo
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
                            IdTemporada = x.p.IdTemporada,
                            Jornada = x.p.Jornada,
                            Fecha = x.p.Fecha,
                            Hora = x.p.Hora,
                            Condicion = x.p.Condicion,
                            IdEquipoLocal = x.p.IdEquipoLocal,
                            NombreEquipoLocal = x.e.NombreEquipo,
                            GolesLocal = x.p.GolesLocal,
                            IdEquipoVisitante = x.p.IdEquipoVisitante,
                            NombreEquipoVisitante = x.equipoVisitante.NombreEquipo,
                            GolesVisitante = x.p.GolesVisitante,
                            IdPabellon = x.p.IdPabellon,
                            NombrePabellon = pab.FirstOrDefault() != null ? pab.FirstOrDefault()!.NombrePab : "Desconocido"
                        })
                    .ToListAsync();

                return Ok(partidos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al obtener partidos", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo partido
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<dynamic>> CreatePartido([FromBody] CreatePartidoRequest request)
        {
            try
            {
                if (request.IdEquipoLocal <= 0 || request.IdEquipoVisitante <= 0)
                    return BadRequest(new { error = "Debe informar equipos válidos" });

                if (request.IdEquipoLocal == request.IdEquipoVisitante)
                    return BadRequest(new { error = "El equipo local y visitante no pueden ser el mismo" });

                if (string.IsNullOrWhiteSpace(request.Fecha) || string.IsNullOrWhiteSpace(request.Hora))
                    return BadRequest(new { error = "Fecha y hora son obligatorias" });

                if (!DateOnly.TryParseExact(
                    request.Fecha,
                    ["dd/MM/yyyy", "yyyy-MM-dd"],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var fechaPartido))
                {
                    return BadRequest(new { error = "Formato de fecha inválido. Use DD/MM/AAAA" });
                }

                if (!TimeOnly.TryParseExact(
                    request.Hora,
                    ["HH:mm", "HH:mm:ss"],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var horaPartido))
                {
                    return BadRequest(new { error = "Formato de hora inválido. Use HH:mm" });
                }

                var localExiste = await _context.Equipos.AnyAsync(e => e.IdEquipo == request.IdEquipoLocal);
                var visitanteExiste = await _context.Equipos.AnyAsync(e => e.IdEquipo == request.IdEquipoVisitante);

                if (!localExiste || !visitanteExiste)
                    return BadRequest(new { error = "Uno o ambos equipos no existen" });

                var temporadaId = request.IdTemporada
                    ?? await _context.Temporadas
                        .OrderByDescending(t => t.IdTemporada)
                        .Select(t => (int?)t.IdTemporada)
                        .FirstOrDefaultAsync();

                if (temporadaId == null)
                    return BadRequest(new { error = "No hay temporada disponible para crear el partido" });

                var jornada = request.Jornada
                    ?? ((await _context.Partidos
                        .Where(p => p.IdTemporada == temporadaId)
                        .MaxAsync(p => (int?)p.Jornada)) ?? 0) + 1;

                var idPartido = ((await _context.Partidos.MaxAsync(p => (int?)p.IdPartido)) ?? 0) + 1;
                var condicion = string.IsNullOrWhiteSpace(request.Condicion)
                    ? (request.IdEquipoLocal == 13 ? "local" : "visitante")
                    : request.Condicion;

                var idPabellon = request.IdPabellon;
                if (idPabellon == null)
                {
                    idPabellon = await _context.Equipos
                        .Where(e => e.IdEquipo == request.IdEquipoLocal)
                        .Select(e => e.IdPabellon)
                        .FirstOrDefaultAsync();
                }

                var nuevoPartido = new Partido
                {
                    IdPartido = idPartido,
                    IdEquipoLocal = request.IdEquipoLocal,
                    IdEquipoVisitante = request.IdEquipoVisitante,
                    IdTemporada = temporadaId.Value,
                    Jornada = jornada,
                    Fecha = fechaPartido,
                    Hora = horaPartido,
                    IdPabellon = idPabellon,
                    GolesLocal = 0,
                    GolesVisitante = 0,
                    Condicion = condicion
                };

                _context.Partidos.Add(nuevoPartido);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPartido), new { id = nuevoPartido.IdPartido }, new
                {
                    idPartido = nuevoPartido.IdPartido,
                    jornada = nuevoPartido.Jornada,
                    fecha = nuevoPartido.Fecha,
                    hora = nuevoPartido.Hora,
                    idEquipoLocal = nuevoPartido.IdEquipoLocal,
                    idEquipoVisitante = nuevoPartido.IdEquipoVisitante,
                    condicion = nuevoPartido.Condicion
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al crear partido", detalles = ex.Message });
            }
        }

        /// <summary>
        /// Añade un evento manual a un partido (gol o sanción)
        /// </summary>
        [HttpPost("{id}/eventos")]
        public async Task<ActionResult<dynamic>> AddEventoPartido(int id, [FromBody] AddEventoPartidoRequest request)
        {
            try
            {
                var partido = await _context.Partidos.FirstOrDefaultAsync(p => p.IdPartido == id);
                if (partido == null)
                    return NotFound(new { error = "Partido no encontrado" });

                if (request.Minute < 0 || request.Minute > 130)
                    return BadRequest(new { error = "Minuto inválido" });

                var tipo = request.Type?.Trim().ToLowerInvariant();
                var tiposValidos = new[] { "gol", "amarilla", "2min", "roja" };

                if (string.IsNullOrWhiteSpace(tipo) || !tiposValidos.Contains(tipo))
                    return BadRequest(new { error = "Tipo de evento inválido" });

                int? idJugador = request.PlayerId;
                if (idJugador == null)
                {
                    if (!string.IsNullOrWhiteSpace(request.PlayerName))
                    {
                        idJugador = await _context.Jugadores
                            .Where(j => j.NombreJugador == request.PlayerName)
                            .Select(j => (int?)j.IdJugador)
                            .FirstOrDefaultAsync();
                    }

                    if (idJugador == null && request.PlayerNumber != null)
                    {
                        idJugador = await _context.Jugadores
                            .Where(j => j.Dorsal == request.PlayerNumber)
                            .Select(j => (int?)j.IdJugador)
                            .FirstOrDefaultAsync();
                    }
                }

                if (idJugador == null)
                    return BadRequest(new { error = "No se encontró el jugador para registrar el evento" });

                var jugador = await _context.Jugadores.FirstOrDefaultAsync(j => j.IdJugador == idJugador.Value);
                if (jugador == null)
                    return BadRequest(new { error = "Jugador no encontrado" });

                var baseStats = await _context.EstadisticasBases
                    .Include(e => e.EstadisticasCampo)
                    .FirstOrDefaultAsync(e => e.IdPartido == id && e.IdJugador == idJugador.Value);

                if (baseStats == null)
                {
                    var idBase = ((await _context.EstadisticasBases.MaxAsync(e => (int?)e.IdBase)) ?? 0) + 1;
                    baseStats = new EstadisticasBase
                    {
                        IdBase = idBase,
                        IdPartido = id,
                        IdJugador = idJugador.Value,
                        SancionAmarilla = 0,
                        Sancion2mins1 = 0,
                        Sancion2mins2 = 0,
                        SancionRoja = 0,
                        SancionAzul = 0,
                        ValoracionGlobal = 0
                    };

                    _context.EstadisticasBases.Add(baseStats);
                }

                if (tipo == "gol")
                {
                    if (baseStats.EstadisticasCampo == null)
                    {
                        var idEstadCampo = ((await _context.EstadisticasCampos.MaxAsync(e => (int?)e.IdEstadsCampo)) ?? 0) + 1;
                        var estadCampo = new EstadisticasCampo
                        {
                            IdEstadsCampo = idEstadCampo,
                            IdBase = baseStats.IdBase,
                            TotalLanzamientos = 0,
                            TotalGoles = 0
                        };

                        _context.EstadisticasCampos.Add(estadCampo);
                        baseStats.EstadisticasCampo = estadCampo;
                    }

                    baseStats.EstadisticasCampo.TotalLanzamientos = (baseStats.EstadisticasCampo.TotalLanzamientos ?? 0) + 1;
                    baseStats.EstadisticasCampo.TotalGoles = (baseStats.EstadisticasCampo.TotalGoles ?? 0) + 1;

                    if (jugador.IdEquipo == partido.IdEquipoLocal)
                        partido.GolesLocal = (partido.GolesLocal ?? 0) + 1;
                    else if (jugador.IdEquipo == partido.IdEquipoVisitante)
                        partido.GolesVisitante = (partido.GolesVisitante ?? 0) + 1;
                }
                else if (tipo == "amarilla")
                {
                    baseStats.SancionAmarilla = (baseStats.SancionAmarilla ?? 0) + 1;
                }
                else if (tipo == "2min")
                {
                    if ((baseStats.Sancion2mins1 ?? 0) == 0)
                        baseStats.Sancion2mins1 = 1;
                    else
                        baseStats.Sancion2mins2 = (baseStats.Sancion2mins2 ?? 0) + 1;
                }
                else if (tipo == "roja")
                {
                    baseStats.SancionRoja = 1;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = $"{id}-{idJugador}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                    minute = request.Minute,
                    type = tipo,
                    playerId = idJugador,
                    playerName = jugador.NombreJugador,
                    playerNumber = jugador.Dorsal
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al añadir evento", detalles = ex.Message });
            }
        }
    }
}