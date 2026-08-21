namespace SistemaPrestamosWeb.DTOs
{
    public class CatalogoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Meses { get; set; }
        public bool Activo { get; set; }
    }
}