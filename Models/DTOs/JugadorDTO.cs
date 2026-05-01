namespace TactiqApi.Models.DTOs
{
    /// <summary>
    /// DTO para transferir datos de jugadores al frontend
    /// Contiene solo la información básica del jugador
    /// </summary>
    public class JugadorDTO
    {
        public int IdJugador { get; set; }
        public string NombreJugador { get; set; }
        public int Dorsal { get; set; }
        public string ImagenJugador { get; set; }
        public string Posicion { get; set; }  // ej: "Portero", "Campo"
        public string RolEspecifico { get; set; }  // ej: "Central", "Lateral"
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; }
    }
}
