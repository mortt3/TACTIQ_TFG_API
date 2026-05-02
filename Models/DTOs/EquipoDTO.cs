namespace TactiqApi.Models.DTOs
{
    public class EquipoDTO
    {
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public string? ImagenLogo { get; set; }
        public string? Ciudad { get; set; }
        public int? IdPabellon { get; set; }
    }
}