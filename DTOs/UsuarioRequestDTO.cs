namespace SistemaPrestamosWeb.DTOs
{
    public class UsuarioRequestDTO
    {
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public int IdRol { get; set; }
    }
}