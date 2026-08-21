namespace SistemaPrestamosWeb.DTOs
{
    public class ActualizarEncuestaDTO : CrearEncuestaDTO
    {
        public int idEncuesta { get; set; }
        public int? idUsuarioAuditoria { get; set; }
    }
}