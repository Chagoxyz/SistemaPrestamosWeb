using System.Collections.Generic;

namespace SistemaPrestamosWeb.DTOs
{
    public class EncuestaDetalleDTO
    {
        public EncuestaCabeceraDTO encuesta { get; set; }
        public List<RespuestaDetalleDTO> respuestas { get; set; }
        public List<MedioDetalleDTO> medios { get; set; }
    }

    public class EncuestaCabeceraDTO
    {
        public int idEncuesta { get; set; }
        public string cliente { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }

        public int idGenero { get; set; }
        public int idNivelEducativo { get; set; }
        public int idRangoIngresos { get; set; }
        public int idRangoEdad { get; set; }
        public int idCiudad { get; set; }
        public int idOcupacion { get; set; }

        public int idTipoPrestamo { get; set; }
        public int idPlazo { get; set; }
        public int idMontoRango { get; set; }
        public int idTasaRango { get; set; }
        public int idCapacidadPago { get; set; }
        public int idHistorial { get; set; }

        public string observacion { get; set; }
    }

    public class RespuestaDetalleDTO
    {
        public int idPregunta { get; set; }
        public string valorTexto { get; set; }
        public int? valorEntero { get; set; }
        public decimal? valorDecimal { get; set; }
    }

    public class MedioDetalleDTO
    {
        public int idMedio { get; set; }
    }
}