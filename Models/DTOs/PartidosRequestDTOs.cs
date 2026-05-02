namespace TactiqApi.Models.DTOs
{
    public class CreatePartidoRequest
    {
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public int? IdTemporada { get; set; }
        public int? Jornada { get; set; }
        public int? IdPabellon { get; set; }
        public string? Condicion { get; set; }
    }

    public class AddEventoPartidoRequest
    {
        public int Minute { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public int? PlayerNumber { get; set; }
    }
}