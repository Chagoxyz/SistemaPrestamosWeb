namespace SistemaPrestamosWeb.DTOs
{
    public class RespuestaEncuestaDTO
    {
        public int idPregunta { get; set; }
        public string valorTexto { get; set; }
        public int? valorEntero { get; set; }
        public decimal? valorDecimal { get; set; }
    }
}