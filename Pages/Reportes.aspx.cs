using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class Reportes : System.Web.UI.Page
    {
        private readonly ApiService api =
            new ApiService();


        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(
                    "~/Pages/Login.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            string rol =
                Session["Rol"]
                    ?.ToString()
                    ?.Trim();


            bool esAdministrador =
                string.Equals(
                    rol,
                    "Administrador",
                    StringComparison.OrdinalIgnoreCase
                );

            bool esConsultor =
                string.Equals(
                    rol,
                    "Consultor",
                    StringComparison.OrdinalIgnoreCase
                );


            // Solo Administrador y Consultor
            if (!esAdministrador &&
                !esConsultor)
            {
                Response.Redirect(
                    "~/Pages/Dashboard.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            // ===============================
            // MENÚ POR ROL
            // ===============================

            lnkDashboard.Visible = true;

            lnkEncuesta.Visible = true;

            lnkGestionEncuestas.Visible =
                esAdministrador;

            lnkCatalogos.Visible =
                esAdministrador;

            lnkUsuarios.Visible =
                esAdministrador;

            lnkReportes.Visible = true;


            if (!IsPostBack)
            {
                CargarComboReportes();
            }
        }


        // =====================================================
        // COMBO DE REPORTES
        // =====================================================

        private void CargarComboReportes()
        {
            ddlReporte.Items.Clear();

            AgregarReporte(
                1,
                "Tipo de préstamo más solicitado por rango de edad"
            );

            AgregarReporte(
                2,
                "Plazo más frecuente entre quienes ganan más de $2000"
            );

            AgregarReporte(
                3,
                "Porcentaje que prefiere plazos mayores a 36 meses"
            );

            AgregarReporte(
                4,
                "Monto promedio solicitado por tipo de préstamo"
            );

            AgregarReporte(
                5,
                "Personas con ingresos bajos que aceptarían tasa mayor a 10%"
            );

            AgregarReporte(
                6,
                "Relación entre nivel educativo y préstamo para estudios"
            );

            AgregarReporte(
                7,
                "Medio de contratación preferido por jóvenes 18-25"
            );

            AgregarReporte(
                8,
                "Encuestados con morosidad que aún solicitan préstamos"
            );

            AgregarReporte(
                9,
                "Capacidad de pago promedio por rango de edad"
            );

            AgregarReporte(
                10,
                "Combinación tipo préstamo + plazo más frecuente"
            );

            AgregarReporte(
                11,
                "Tasa de interés promedio aceptada por rango de ingreso"
            );

            AgregarReporte(
                12,
                "Tipo de préstamo más aceptado entre universitarios"
            );

            AgregarReporte(
                13,
                "Cantidad que prefiere pagar menos de $200 mensuales"
            );

            AgregarReporte(
                14,
                "Correlación aproximada entre edad y monto solicitado"
            );

            AgregarReporte(
                15,
                "Rango de ingresos que solicita montos más altos"
            );

            AgregarReporte(
                16,
                "Medio más usado por personas que han morado"
            );

            AgregarReporte(
                17,
                "Porcentaje de mujeres que prefieren préstamos personales"
            );

            AgregarReporte(
                18,
                "Personas que prefieren plazos menores o iguales a 24 meses"
            );

            AgregarReporte(
                19,
                "Distribución de montos por tipo de préstamo"
            );

            AgregarReporte(
                20,
                "Nivel educativo con mayor morosidad declarada"
            );

            AgregarReporte(
                21,
                "Aceptación de tasas mayores a 12% si plazo mayor a 36 meses"
            );

            AgregarReporte(
                22,
                "Tipo de préstamo más popular entre 30-45 años"
            );

            AgregarReporte(
                23,
                "Capacidad de pago promedio por nivel educativo"
            );

            AgregarReporte(
                24,
                "Plazo preferido por quienes tienen morosidad previa"
            );

            AgregarReporte(
                25,
                "Ingresos altos que prefieren préstamos para vehículos"
            );

            AgregarReporte(
                26,
                "Combinación monto + plazo más solicitada"
            );

            AgregarReporte(
                27,
                "Relación entre tasa aceptable y capacidad de pago"
            );

            AgregarReporte(
                28,
                "Medio preferido por quienes nunca tuvieron préstamos"
            );

            AgregarReporte(
                29,
                "Personas que prefieren pagar menos de 12 meses"
            );

            AgregarReporte(
                30,
                "Perfil más común edad + ingreso + tipo de préstamo"
            );
        }


        private void AgregarReporte(
            int numero,
            string descripcion)
        {
            ddlReporte.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    $"Reporte {numero:D2} - {descripcion}",
                    numero.ToString()
                )
            );
        }


        // =====================================================
        // CONSULTAR
        // =====================================================

        protected async void btnConsultar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                int numero =
                    Convert.ToInt32(
                        ddlReporte.SelectedValue
                    );


                // =====================================
                // TABLA
                // =====================================

                var datosReporte =
                    await api.GetAsync<
                        List<Dictionary<string, object>>
                    >(
                        "Reporte/" + numero
                    );


                DataTable tabla =
                    ConvertirADataTable(
                        datosReporte
                    );


                gvReporte.DataSource =
                    tabla;

                gvReporte.DataBind();


                // =====================================
                // GRÁFICO
                // =====================================

                var datosGrafico =
                    await api.GetAsync<
                        List<Dictionary<string, object>>
                    >(
                        "Grafico/" + numero
                    );


                PrepararGrafico(
                    numero,
                    datosGrafico
                );


                lblMensaje.Text =
                    "Reporte y gráfico cargados correctamente.";

                lblMensaje.CssClass =
                    "mensajeCorrecto";


                ClientScript.RegisterStartupScript(
                    GetType(),
                    "cargarGrafico",
                    "cargarGrafico();",
                    true
                );
            }
            catch
            {
                lblMensaje.Text =
                    "No se pudo cargar el reporte. " +
                    "Revise la información e intente nuevamente.";

                lblMensaje.CssClass =
                    "mensajeError";
            }
        }


        // =====================================================
        // CONVERTIR RESULTADO A DATATABLE
        // =====================================================

        private DataTable ConvertirADataTable(
            List<Dictionary<string, object>> datos)
        {
            DataTable tabla =
                new DataTable();


            if (datos == null ||
                datos.Count == 0)
            {
                return tabla;
            }


            foreach (
                string columna
                in datos[0].Keys)
            {
                tabla.Columns.Add(
                    TraducirColumna(
                        columna
                    )
                );
            }


            foreach (var fila in datos)
            {
                DataRow row =
                    tabla.NewRow();

                int index = 0;


                foreach (
                    object valor
                    in fila.Values)
                {
                    row[index] =
                        valor ?? "";

                    index++;
                }


                tabla.Rows.Add(
                    row
                );
            }


            return tabla;
        }


        // =====================================================
        // PREPARAR GRÁFICO
        // =====================================================

        private void PrepararGrafico(
            int numero,
            List<Dictionary<string, object>> datos)
        {
            if (datos == null ||
                datos.Count == 0)
            {
                hfLabelsGrafico.Value =
                    "[]";

                hfValoresGrafico.Value =
                    "[]";

                hfTituloGrafico.Value =
                    ddlReporte.SelectedItem.Text;

                hfTipoGrafico.Value =
                    ObtenerTipoGrafico(
                        numero
                    );

                return;
            }


            var primeraFila =
                datos.First();


            var columnas =
                primeraFila.Keys
                    .ToList();


            List<string> etiquetas =
                new List<string>();


            List<decimal> valores =
                new List<decimal>();


            // =====================================
            // SOLO UNA COLUMNA
            // =====================================

            if (columnas.Count == 1)
            {
                etiquetas.Add(
                    "Resultado"
                );

                valores.Add(
                    ConvertirNumero(
                        primeraFila[
                            columnas[0]
                        ]
                    )
                );
            }

            // =====================================
            // DOS O MÁS COLUMNAS
            // =====================================

            else
            {
                string columnaEtiqueta =
                    columnas[0];


                string columnaValor =
                    BuscarColumnaNumerica(
                        datos,
                        columnas
                    );


                foreach (var fila in datos)
                {
                    string etiqueta =
                        fila[columnaEtiqueta]
                            ?.ToString()
                        ?? "";


                    decimal valor =
                        ConvertirNumero(
                            fila[columnaValor]
                        );


                    etiquetas.Add(
                        etiqueta
                    );

                    valores.Add(
                        valor
                    );
                }
            }


            hfLabelsGrafico.Value =
                JsonConvert.SerializeObject(
                    etiquetas
                );


            hfValoresGrafico.Value =
                JsonConvert.SerializeObject(
                    valores
                );


            hfTituloGrafico.Value =
                ddlReporte.SelectedItem.Text;


            hfTipoGrafico.Value =
                ObtenerTipoGrafico(
                    numero
                );
        }


        // =====================================================
        // BUSCAR COLUMNA NUMÉRICA
        // =====================================================

        private string BuscarColumnaNumerica(
            List<Dictionary<string, object>> datos,
            List<string> columnas)
        {
            // Se busca desde la última columna
            // porque normalmente allí viene
            // Cantidad, Promedio, Porcentaje, etc.

            for (
                int i = columnas.Count - 1;
                i >= 0;
                i--)
            {
                string columna =
                    columnas[i];


                foreach (var fila in datos)
                {
                    object valor =
                        fila[columna];


                    decimal numero;


                    if (valor != null &&
                        decimal.TryParse(
                            valor.ToString(),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out numero
                        ))
                    {
                        return columna;
                    }


                    if (valor != null &&
                        decimal.TryParse(
                            valor.ToString(),
                            out numero
                        ))
                    {
                        return columna;
                    }
                }
            }


            // Si no encuentra una columna numérica,
            // usa la última.

            return columnas[
                columnas.Count - 1
            ];
        }


        // =====================================================
        // CONVERTIR A NÚMERO
        // =====================================================

        private decimal ConvertirNumero(
            object valor)
        {
            if (valor == null ||
                valor == DBNull.Value)
            {
                return 0;
            }


            decimal numero;


            if (decimal.TryParse(
                valor.ToString(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out numero
            ))
            {
                return numero;
            }


            if (decimal.TryParse(
                valor.ToString(),
                out numero
            ))
            {
                return numero;
            }


            return 0;
        }


        // =====================================================
        // TIPO DE GRÁFICO
        // =====================================================

        private string ObtenerTipoGrafico(
            int numero)
        {
            /*
             * Utilizamos al menos
             * tres tipos diferentes:
             *
             * bar
             * pie
             * line
             */

            int tipo =
                numero % 3;


            if (tipo == 1)
            {
                return "bar";
            }


            if (tipo == 2)
            {
                return "pie";
            }


            return "line";
        }


        // =====================================================
        // TRADUCIR COLUMNAS
        // =====================================================

        private string TraducirColumna(
            string columna)
        {
            switch (columna)
            {
                case "Count":
                    return "Cantidad";

                case "RangoEdad":
                    return "Rango de Edad";

                case "TipoPrestamo":
                    return "Tipo de Préstamo";

                case "Cantidad":
                    return "Cantidad";

                case "PlazoMeses":
                    return "Plazo en Meses";

                case "MontoSolicitado":
                    return "Monto Solicitado";

                case "TasaAceptable":
                    return "Tasa Aceptable";

                case "CapacidadPago":
                    return "Capacidad de Pago";

                case "RangoIngresos":
                    return "Rango de Ingresos";

                case "NivelEducativo":
                    return "Nivel Educativo";

                case "MedioContratacion":
                    return "Medio de Contratación";

                case "Porcentaje":
                    return "Porcentaje";

                case "Promedio":
                    return "Promedio";

                default:
                    return columna;
            }
        }
    }
}