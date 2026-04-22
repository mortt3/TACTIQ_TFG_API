namespace TactiqApi.Models
{
    public class PlayerStatsDTO
    {
        public int id_jugador { get; set; }
        public string nombre_jugador { get; set; }
        public int dorsal { get; set; }
        public string posicion { get; set; }
        public int total_lanzamientos { get; set; }
        public int total_goles { get; set; }
        public int exclusiones_2min { get; set; }
        public decimal valoracion_total { get; set; }
        public string? imagen_jugador { get; set; }
        public int? edad { get; set; }
    }
}