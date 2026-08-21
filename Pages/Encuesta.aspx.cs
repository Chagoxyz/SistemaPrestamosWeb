using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SistemaPrestamosWeb.DTOs;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class Encuesta : System.Web.UI.Page
    {
        private readonly ApiService api = new ApiService();

        private int? IdEncuestaEdicion
        {
            get
            {
                if (int.TryParse(Request.QueryString["id"], out int id))
                {
                    return id;
                }

                return null;
            }
        }

        private EncuestaDetalleDTO EncuestaActual
        {
            get
            {
                return Session["EncuestaActual"] as EncuestaDetalleDTO;
            }
            set
            {
                Session["EncuestaActual"] = value;
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(
                    "Login.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            // =========================================
            // CONTROL DE MENÚ SEGÚN ROL
            // =========================================

            string rol =
                Session["Rol"]?.ToString();

            bool esAdministrador =
                rol == "Administrador";

            bool esConsultor =
                rol == "Consultor";

            bool esEncuestador =
                rol == "Encuestador";


            // Todos pueden ver Dashboard
            lnkDashboard.Visible = true;


            // Los tres roles pueden registrar encuestas
            lnkEncuesta.Visible =
                esAdministrador ||
                esConsultor ||
                esEncuestador;


            // Solo Administrador
            lnkGestionEncuestas.Visible =
                esAdministrador;

            lnkCatalogos.Visible =
                esAdministrador;

            lnkUsuarios.Visible =
                esAdministrador;


            // Administrador y Consultor
            lnkReportes.Visible =
                esAdministrador ||
                esConsultor;


            // =========================================
            // CÓDIGO EXISTENTE DE LA ENCUESTA
            // =========================================

            try
            {
                if (!IsPostBack)
                {
                    if (IdEncuestaEdicion.HasValue)
                    {
                        EncuestaActual =
                            await api.GetAsync<EncuestaDetalleDTO>(
                                "Encuesta/" +
                                IdEncuestaEdicion.Value
                            );
                    }
                    else
                    {
                        EncuestaActual = null;
                    }
                }

                await ConstruirFormulario();
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass =
                    "mensajeError";

                lblMensaje.Text =
                    "Error al cargar la encuesta: "
                    + ex.Message;
            }
        }

        private async System.Threading.Tasks.Task ConstruirFormulario()
        {
            var preguntas =
                await api.GetAsync<List<PreguntaDTO>>(
                    "Catalogo/preguntas"
                );

            var html = new StringBuilder();

            foreach (var pregunta in preguntas)
            {
                html.Append("<div class='pregunta'>");

                html.Append(
                    $"<label>{pregunta.IdPregunta}. " +
                    $"{HttpUtility.HtmlEncode(pregunta.Texto)}</label>"
                );

                html.Append(await CrearControl(pregunta));

                html.Append("</div>");
            }

            phEncuesta.Controls.Clear();

            phEncuesta.Controls.Add(
                new System.Web.UI.LiteralControl(
                    html.ToString()
                )
            );
        }

        private async System.Threading.Tasks.Task<string>
            CrearControl(PreguntaDTO pregunta)
        {
            string name = "preg_" + pregunta.IdPregunta;

            string valorActual =
                ObtenerValorPregunta(pregunta.IdPregunta);

            if (pregunta.TipoControl == "TextBox")
            {
                if (pregunta.IdPregunta == 20)
                {
                    return
                        $"<textarea class='campo' " +
                        $"name='{name}' rows='4'>" +
                        $"{HttpUtility.HtmlEncode(valorActual)}" +
                        "</textarea>";
                }

                return
                    $"<input class='campo' " +
                    $"type='text' " +
                    $"name='{name}' " +
                    $"value='{HttpUtility.HtmlAttributeEncode(valorActual)}' />";
            }

            if (pregunta.TipoControl == "DropDownList")
            {
                return await CrearSelect(
                    name,
                    pregunta.IdPregunta,
                    valorActual
                );
            }

            if (pregunta.TipoControl == "RadioButtonList")
            {
                return await CrearRadio(
                    name,
                    pregunta.IdPregunta,
                    valorActual
                );
            }

            if (pregunta.TipoControl == "CheckBoxList")
            {
                return await CrearCheckbox(
                    name,
                    pregunta.IdPregunta
                );
            }

            return
                $"<input class='campo' " +
                $"type='text' " +
                $"name='{name}' " +
                $"value='{HttpUtility.HtmlAttributeEncode(valorActual)}' />";
        }

        private async System.Threading.Tasks.Task<string>
            CrearSelect(
                string name,
                int idPregunta,
                string valorActual)
        {
            var opciones =
                await ObtenerOpciones(idPregunta);

            var html = new StringBuilder();

            html.Append(
                $"<select class='campo' name='{name}'>"
            );

            html.Append(
                "<option value=''>Seleccione una opción</option>"
            );

            foreach (var item in opciones)
            {
                string selected =
                    item.Value == valorActual
                        ? " selected='selected'"
                        : "";

                html.Append(
                    $"<option value='{HttpUtility.HtmlAttributeEncode(item.Value)}'" +
                    $"{selected}>" +
                    $"{HttpUtility.HtmlEncode(item.Text)}" +
                    "</option>"
                );
            }

            html.Append("</select>");

            return html.ToString();
        }

        private async System.Threading.Tasks.Task<string>
            CrearRadio(
                string name,
                int idPregunta,
                string valorActual)
        {
            var opciones =
                await ObtenerOpciones(idPregunta);

            var html = new StringBuilder();

            foreach (var item in opciones)
            {
                string marcado =
                    item.Value == valorActual
                        ? " checked='checked'"
                        : "";

                html.Append("<div class='opcion'>");

                html.Append(
                    $"<input type='radio' " +
                    $"name='{name}' " +
                    $"value='{HttpUtility.HtmlAttributeEncode(item.Value)}'" +
                    $"{marcado} /> " +
                    $"{HttpUtility.HtmlEncode(item.Text)}"
                );

                html.Append("</div>");
            }

            return html.ToString();
        }

        private async System.Threading.Tasks.Task<string>
            CrearCheckbox(
                string name,
                int idPregunta)
        {
            var opciones =
                await ObtenerOpciones(idPregunta);

            var html = new StringBuilder();

            var seleccionados = new List<int>();

            if (EncuestaActual?.medios != null)
            {
                seleccionados =
                    EncuestaActual.medios
                        .Select(x => x.idMedio)
                        .ToList();
            }

            foreach (var item in opciones)
            {
                int idMedio = 0;
                int.TryParse(item.Value, out idMedio);

                string marcado =
                    seleccionados.Contains(idMedio)
                        ? " checked='checked'"
                        : "";

                html.Append("<div class='opcion'>");

                html.Append(
                    $"<input type='checkbox' " +
                    $"name='{name}' " +
                    $"value='{HttpUtility.HtmlAttributeEncode(item.Value)}'" +
                    $"{marcado} /> " +
                    $"{HttpUtility.HtmlEncode(item.Text)}"
                );

                html.Append("</div>");
            }

            return html.ToString();
        }

        private string ObtenerValorPregunta(int idPregunta)
        {
            if (EncuestaActual == null ||
                EncuestaActual.encuesta == null)
            {
                return "";
            }

            var e = EncuestaActual.encuesta;

            switch (idPregunta)
            {
                case 1:
                    return e.cliente ?? "";

                case 2:
                    return e.email ?? "";

                case 3:
                    return e.telefono ?? "";

                case 4:
                    return e.idGenero.ToString();

                case 5:
                    return e.idRangoEdad.ToString();

                case 6:
                    return e.idNivelEducativo.ToString();

                case 7:
                    return e.idRangoIngresos.ToString();

                case 8:
                    return e.idCiudad.ToString();

                case 9:
                    return e.idOcupacion.ToString();

                case 10:
                    return e.idTipoPrestamo.ToString();

                case 11:
                    return e.idMontoRango.ToString();

                case 12:
                    return e.idPlazo.ToString();

                case 13:
                    return e.idTasaRango.ToString();

                case 14:
                    return e.idCapacidadPago.ToString();

                case 15:
                    return e.idHistorial.ToString();

                case 17:
                    return "";

                case 20:
                    return e.observacion ?? "";
            }

            var respuesta =
                EncuestaActual.respuestas?
                    .FirstOrDefault(
                        x => x.idPregunta == idPregunta
                    );

            return respuesta?.valorTexto ?? "";
        }

        private async System.Threading.Tasks.Task<List<OpcionDTO>>
            ObtenerOpciones(int idPregunta)
        {
            switch (idPregunta)
            {
                case 4:
                    return await CargarCatalogo(
                        "Catalogo/generos",
                        "IdGenero",
                        "Nombre"
                    );

                case 5:
                    return await CargarCatalogo(
                        "Catalogo/rangos-edad",
                        "IdRangoEdad",
                        "Descripcion"
                    );

                case 6:
                    return await CargarCatalogo(
                        "Catalogo/niveles-educativos",
                        "IdNivelEducativo",
                        "Nombre"
                    );

                case 7:
                    return await CargarCatalogo(
                        "Catalogo/rangos-ingresos",
                        "IdRangoIngresos",
                        "Descripcion"
                    );

                case 8:
                    return await CargarCatalogo(
                        "Catalogo/ciudades",
                        "IdCiudad",
                        "Nombre"
                    );

                case 9:
                    return await CargarCatalogo(
                        "Catalogo/ocupaciones",
                        "IdOcupacion",
                        "Nombre"
                    );

                case 10:
                    return await CargarCatalogo(
                        "Catalogo/tipos-prestamo",
                        "IdTipoPrestamo",
                        "Nombre"
                    );

                case 11:
                    return await CargarCatalogo(
                        "Catalogo/montos-rango",
                        "IdMontoRango",
                        "Descripcion"
                    );

                case 12:
                    return await CargarCatalogo(
                        "Catalogo/plazos",
                        "IdPlazo",
                        "Meses"
                    );

                case 13:
                    return await CargarCatalogo(
                        "Catalogo/tasas-interes",
                        "IdTasaRango",
                        "Descripcion"
                    );

                case 14:
                    return await CargarCatalogo(
                        "Catalogo/capacidades-pago",
                        "IdCapacidadPago",
                        "Descripcion"
                    );

                case 15:
                    return await CargarCatalogo(
                        "Catalogo/historiales-crediticios",
                        "IdHistorial",
                        "Descripcion"
                    );

                case 16:
                case 18:
                    return new List<OpcionDTO>
                    {
                        new OpcionDTO
                        {
                            Value = "Sí",
                            Text = "Sí"
                        },

                        new OpcionDTO
                        {
                            Value = "No",
                            Text = "No"
                        }
                    };

                case 17:
                    return await CargarCatalogo(
                        "Catalogo/medios-contratacion",
                        "IdMedio",
                        "Nombre"
                    );

                case 19:
                    return new List<OpcionDTO>
                    {
                        new OpcionDTO
                        {
                            Value = "Tasa de interés",
                            Text = "Tasa de interés"
                        },

                        new OpcionDTO
                        {
                            Value = "Plazo",
                            Text = "Plazo"
                        },

                        new OpcionDTO
                        {
                            Value = "Monto aprobado",
                            Text = "Monto aprobado"
                        },

                        new OpcionDTO
                        {
                            Value = "Facilidad del trámite",
                            Text = "Facilidad del trámite"
                        }
                    };

                default:
                    return new List<OpcionDTO>();
            }
        }

        private async System.Threading.Tasks.Task<List<OpcionDTO>>
            CargarCatalogo(
                string endpoint,
                string campoId,
                string campoTexto)
        {
            var datos =
                await api.GetAsync<
                    List<Dictionary<string, object>>
                >(endpoint);

            return datos.Select(
                x => new OpcionDTO
                {
                    Value = x[campoId].ToString(),
                    Text = x[campoTexto].ToString()
                }
            ).ToList();
        }

        protected async void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int idUsuarioAuditoria =
                    Convert.ToInt32(
                        Session["IdUsuario"]
                    );

                if (IdEncuestaEdicion.HasValue)
                {
                    var encuesta =
                        new ActualizarEncuestaDTO
                        {
                            idEncuesta =
                                IdEncuestaEdicion.Value,

                            nombreCliente =
                                Request.Form["preg_1"],

                            email =
                                Request.Form["preg_2"],

                            telefono =
                                Request.Form["preg_3"],

                            idGenero =
                                Convert.ToInt32(
                                    Request.Form["preg_4"]
                                ),

                            idRangoEdad =
                                Convert.ToInt32(
                                    Request.Form["preg_5"]
                                ),

                            idNivelEducativo =
                                Convert.ToInt32(
                                    Request.Form["preg_6"]
                                ),

                            idRangoIngresos =
                                Convert.ToInt32(
                                    Request.Form["preg_7"]
                                ),

                            idCiudad =
                                Convert.ToInt32(
                                    Request.Form["preg_8"]
                                ),

                            idOcupacion =
                                Convert.ToInt32(
                                    Request.Form["preg_9"]
                                ),

                            idTipoPrestamo =
                                Convert.ToInt32(
                                    Request.Form["preg_10"]
                                ),

                            idMontoRango =
                                Convert.ToInt32(
                                    Request.Form["preg_11"]
                                ),

                            idPlazo =
                                Convert.ToInt32(
                                    Request.Form["preg_12"]
                                ),

                            idTasaRango =
                                Convert.ToInt32(
                                    Request.Form["preg_13"]
                                ),

                            idCapacidadPago =
                                Convert.ToInt32(
                                    Request.Form["preg_14"]
                                ),

                            idHistorial =
                                Convert.ToInt32(
                                    Request.Form["preg_15"]
                                ),

                            idUsuarioAuditoria =
                                idUsuarioAuditoria,

                            observacion =
                                Request.Form["preg_20"],

                            respuestas =
                                ObtenerRespuestas(),

                            medios =
                                ObtenerMedios()
                        };

                    await api.PutAsync<object>(
                        "Encuesta/" + IdEncuestaEdicion.Value,
                        encuesta,
                        idUsuarioAuditoria
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Encuesta actualizada correctamente.";

                    EncuestaActual =
                        await api.GetAsync<EncuestaDetalleDTO>(
                            "Encuesta/" +
                            IdEncuestaEdicion.Value
                        );
                }
                else
                {
                    var encuesta =
                        new CrearEncuestaDTO
                        {
                            nombreCliente =
                                Request.Form["preg_1"],

                            email =
                                Request.Form["preg_2"],

                            telefono =
                                Request.Form["preg_3"],

                            idGenero =
                                Convert.ToInt32(
                                    Request.Form["preg_4"]
                                ),

                            idRangoEdad =
                                Convert.ToInt32(
                                    Request.Form["preg_5"]
                                ),

                            idNivelEducativo =
                                Convert.ToInt32(
                                    Request.Form["preg_6"]
                                ),

                            idRangoIngresos =
                                Convert.ToInt32(
                                    Request.Form["preg_7"]
                                ),

                            idCiudad =
                                Convert.ToInt32(
                                    Request.Form["preg_8"]
                                ),

                            idOcupacion =
                                Convert.ToInt32(
                                    Request.Form["preg_9"]
                                ),

                            idTipoPrestamo =
                                Convert.ToInt32(
                                    Request.Form["preg_10"]
                                ),

                            idMontoRango =
                                Convert.ToInt32(
                                    Request.Form["preg_11"]
                                ),

                            idPlazo =
                                Convert.ToInt32(
                                    Request.Form["preg_12"]
                                ),

                            idTasaRango =
                                Convert.ToInt32(
                                    Request.Form["preg_13"]
                                ),

                            idCapacidadPago =
                                Convert.ToInt32(
                                    Request.Form["preg_14"]
                                ),

                            idHistorial =
                                Convert.ToInt32(
                                    Request.Form["preg_15"]
                                ),

                            idUsuarioCreador =
                                idUsuarioAuditoria,

                            identificadorSesion =
                                Guid.NewGuid().ToString(),

                            direccionIP =
                                Request.UserHostAddress,

                            observacion =
                                Request.Form["preg_20"],

                            respuestas =
                                ObtenerRespuestas(),

                            medios =
                                ObtenerMedios()
                        };

                    await api.PostAsync<object>(
                        "Encuesta",
                        encuesta,
                        idUsuarioAuditoria
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Encuesta registrada correctamente.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass =
                    "mensajeError";

                lblMensaje.Text =
                    "Error: " + ex.Message;
            }
        }

        private List<RespuestaEncuestaDTO>
            ObtenerRespuestas()
        {
            var respuestas =
                new List<RespuestaEncuestaDTO>();

            for (int i = 1; i <= 20; i++)
            {
                if (i == 17)
                {
                    var valores =
                        Request.Form.GetValues(
                            "preg_17"
                        );

                    string texto =
                        valores == null
                            ? ""
                            : string.Join(
                                ",",
                                valores
                            );

                    respuestas.Add(
                        new RespuestaEncuestaDTO
                        {
                            idPregunta = i,
                            valorTexto = texto,
                            valorEntero = null,
                            valorDecimal = null
                        }
                    );
                }
                else
                {
                    respuestas.Add(
                        new RespuestaEncuestaDTO
                        {
                            idPregunta = i,

                            valorTexto =
                                Request.Form[
                                    "preg_" + i
                                ],

                            valorEntero = null,
                            valorDecimal = null
                        }
                    );
                }
            }

            return respuestas;
        }

        private List<int> ObtenerMedios()
        {
            var valores =
                Request.Form.GetValues(
                    "preg_17"
                );

            if (valores == null)
            {
                return new List<int>();
            }

            return valores
                .Select(
                    x => Convert.ToInt32(x)
                )
                .ToList();
        }

        private class OpcionDTO
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
}