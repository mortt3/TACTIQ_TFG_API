namespace TactiqApi.Models.DTOs
{
    /// <summary>
    /// DTO para respuesta de login exitoso
    /// </summary>
    public class LoginResponseDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de error
    /// </summary>
    public class ErrorResponseDTO
    {
        public int CodigoError { get; set; }
        public string Mensaje { get; set; }
        public string Detalles { get; set; }
    }

    /// <summary>
    /// DTO para respuesta de éxito genérica
    /// </summary>
    public class SuccessResponseDTO<T>
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public T Datos { get; set; }
    }
}
