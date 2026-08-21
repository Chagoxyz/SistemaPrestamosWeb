namespace SistemaPrestamosWeb.DTOs
{
    public class PreguntaDTO
    {
        public int IdPregunta { get; set; }
        public string Texto { get; set; }
        public string TipoControl { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }
    }
}