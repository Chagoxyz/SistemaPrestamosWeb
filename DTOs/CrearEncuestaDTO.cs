using System.Collections.Generic;

namespace SistemaPrestamosWeb.DTOs
{
    public class CrearEncuestaDTO
    {
        public string nombreCliente { get; set; }
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

        public int? idUsuarioCreador { get; set; }
        public string identificadorSesion { get; set; }
        public string direccionIP { get; set; }
        public string observacion { get; set; }

        public List<RespuestaEncuestaDTO> respuestas { get; set; }
        public List<int> medios { get; set; }
    }
}