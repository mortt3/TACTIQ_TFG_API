namespace TactiqApi.Models.DTOs
{
    /// <summary>
    /// DTO que unifica todas las estadísticas de un jugador en un partido
    /// Combina: Estadisticas_Base + Estadisticas_Campo + Estadisticas_Portero
    /// </summary>
    public class EstadisticasJugadorDTO
    {
        // --- Datos del Jugador ---
        public int IdJugador { get; set; }
        public string NombreJugador { get; set; }
        public int Dorsal { get; set; }
        public string Posicion { get; set; }  // "Portero" o "Campo"

        // --- Datos del Partido ---
        public int IdPartido { get; set; }
        public int Jornada { get; set; }
        public DateTime FechaPartido { get; set; }
        public string EquipoRival { get; set; }

        // --- ESTADÍSTICAS BASE (aplica a todos) ---
        public string TiempoDef { get; set; }  // Tiempo defensivo
        public string TiempoTot { get; set; }  // Tiempo total jugado
        public decimal ValoracionGlobal { get; set; }
        public int SancionAmarilla { get; set; }
        public int Sancion2Mins1 { get; set; }
        public int Sancion2Mins2 { get; set; }
        public int SancionRoja { get; set; }
        public int SancionAzul { get; set; }

        // --- ESTADÍSTICAS DE CAMPO (solo si es jugador de campo) ---
        public int? TotalLanzamientos { get; set; }
        public int? TotalGoles { get; set; }
        public int? TotalParadas { get; set; }
        public int? TotalPostes { get; set; }
        public int? TotalFuera { get; set; }
        public int? TotalBloqueados { get; set; }

        // 7 metros
        public int? M7Lanzamientos { get; set; }
        public int? M7Goles { get; set; }

        // Contraataque
        public int? ContraataqueLanzamientos { get; set; }
        public int? ContraataqueGoles { get; set; }

        // 9 metros
        public int? M9Lanzamientos { get; set; }
        public int? M9Goles { get; set; }

        // Extremo
        public int? ExtremeLanzamientos { get; set; }
        public int? ExtremeGoles { get; set; }

        // 6 metros / Pivote
        public int? M6Lanzamientos { get; set; }
        public int? M6Goles { get; set; }

        // Valoraciones positivas
        public int? ValoracionPositivaAsistencia { get; set; }
        public int? ValoracionPositivaRecuperacion { get; set; }

        // Valoraciones negativas
        public int? ValoracionNegativaPerdida { get; set; }
        public int? ValoracionNegativaPasos { get; set; }
        public int? ValoracionNegativaDobles { get; set; }
        public int? ValoracionNegativaFaltaAtaque { get; set; }

        // --- ESTADÍSTICAS DE PORTERO (solo si es portero) ---
        public int? GolesRecibidos { get; set; }
        public int? LanzamientosRecibidos { get; set; }
        public decimal? PorcentajeParadas { get; set; }
    }
}
