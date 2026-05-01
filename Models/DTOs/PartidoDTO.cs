namespace TactiqApi.Models.DTOs
{
    /// <summary>
    /// DTO para transferir datos de partidos
    /// </summary>
    public class PartidoDTO
    {
        public int IdPartido { get; set; }
        public int IdTemporada { get; set; }
        public int Jornada { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public string Condicion { get; set; }  // "local" o "visitante"

        // Equipo local
        public int IdEquipoLocal { get; set; }
        public string NombreEquipoLocal { get; set; }
        public int GolesLocal { get; set; }

        // Equipo visitante
        public int IdEquipoVisitante { get; set; }
        public string NombreEquipoVisitante { get; set; }
        public int GolesVisitante { get; set; }

        // Pabellón
        public int? IdPabellon { get; set; }
        public string NombrePabellon { get; set; }
    }
}
