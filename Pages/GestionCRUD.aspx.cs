using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class GestionCRUD : System.Web.UI.Page
    {
        private readonly ApiService api =
            new ApiService();

        protected async void Page_Load(
            object sender,
            EventArgs e)
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

            if (Session["Rol"]?.ToString()
                != "Administrador")
            {
                Response.Redirect(
                    "Dashboard.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }

            if (!IsPostBack)
            {
                ConfigurarCampos();

                await CargarCatalogo();
            }
        }


        protected async void
            ddlCatalogo_SelectedIndexChanged(
                object sender,
                EventArgs e)
        {
            Limpiar();

            ConfigurarCampos();

            await CargarCatalogo();
        }

        protected async void chkIncluirEliminados_CheckedChanged(
            object sender,
            EventArgs e)
        {
            await CargarCatalogo();
        }


        private void ConfigurarCampos()
        {
            string catalogo =
                ddlCatalogo.SelectedValue;

            pnlNombre.Visible = false;
            pnlPlazo.Visible = false;
            pnlRango.Visible = false;
            pnlHistorial.Visible = false;

            switch (catalogo)
            {
                case "genero":
                case "ciudad":
                case "ocupacion":
                case "nivel-educativo":
                case "tipo-prestamo":
                case "medio-contratacion":

                    pnlNombre.Visible = true;

                    break;

                case "plazo":

                    pnlPlazo.Visible = true;

                    break;

                case "rango-edad":
                case "rango-ingresos":
                case "monto-rango":
                case "tasa-interes":
                case "capacidad-pago":

                    pnlRango.Visible = true;

                    break;

                case "historial-crediticio":

                    pnlHistorial.Visible = true;

                    break;
            }
        }


        private string ObtenerEndpointConsulta()
        {
            switch (ddlCatalogo.SelectedValue)
            {
                case "genero":
                    return "Catalogo/generos";

                case "ciudad":
                    return "Catalogo/ciudades";

                case "ocupacion":
                    return "Catalogo/ocupaciones";

                case "nivel-educativo":
                    return "Catalogo/niveles-educativos";

                case "tipo-prestamo":
                    return "Catalogo/tipos-prestamo";

                case "medio-contratacion":
                    return "Catalogo/medios-contratacion";

                case "plazo":
                    return "Catalogo/plazos";

                case "rango-edad":
                    return "Catalogo/rangos-edad";

                case "rango-ingresos":
                    return "Catalogo/rangos-ingresos";

                case "monto-rango":
                    return "Catalogo/montos-rango";

                case "tasa-interes":
                    return "Catalogo/tasas-interes";

                case "capacidad-pago":
                    return "Catalogo/capacidades-pago";

                case "historial-crediticio":
                    return "Catalogo/historiales-crediticios";

                default:
                    return "";
            }
        }


        private async Task CargarCatalogo()
        {
            try
            {
                string endpoint =
                    ObtenerEndpointConsulta();

                if (chkIncluirEliminados.Checked)
                {
                    endpoint += "?incluirEliminados=true";
                }

                var registros =
                    await api.GetAsync<
                        List<Dictionary<string, object>>
                    >(endpoint);

                DataTable tabla =
                    ConvertirDataTable(registros);

                if (tabla.Columns.Count > 0)
                {
                    string columnaId =
                        ObtenerColumnaId(tabla);

                    if (!string.IsNullOrEmpty(
                        columnaId))
                    {
                        gvCatalogos.DataKeyNames =
                            new[] { columnaId };
                    }
                }

                gvCatalogos.DataSource = tabla;
                gvCatalogos.DataBind();

                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass =
                    "mensajeError";

                lblMensaje.Text =
                    "Error al cargar: "
                    + ex.Message;
            }
        }


        private DataTable ConvertirDataTable(
            List<Dictionary<string, object>>
                registros)
        {
            DataTable tabla =
                new DataTable();

            if (registros == null ||
                registros.Count == 0)
            {
                return tabla;
            }

            foreach (
                var columna
                in registros[0].Keys)
            {
                tabla.Columns.Add(columna);
            }

            foreach (var registro in registros)
            {
                DataRow fila =
                    tabla.NewRow();

                foreach (var dato in registro)
                {
                    fila[dato.Key] =
                        dato.Value ?? "";
                }

                tabla.Rows.Add(fila);
            }

            return tabla;
        }


        private string ObtenerColumnaId(
            DataTable tabla)
        {
            foreach (
                DataColumn columna
                in tabla.Columns)
            {
                if (columna.ColumnName
                    .StartsWith(
                        "id",
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return columna.ColumnName;
                }
            }

            return "";
        }


        protected async void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string catalogo =
                    ddlCatalogo.SelectedValue;

                int idUsuario =
                    Convert.ToInt32(
                        Session["IdUsuario"]
                    );

                object datos =
                    CrearObjetoEnvio(catalogo);

                if (string.IsNullOrEmpty(
                    hfId.Value))
                {
                    await api.PostAsync<object>(
                        "Catalogo/mantenimiento/"
                            + catalogo,
                        datos,
                        idUsuario
                    );

                    lblMensaje.Text =
                        "Registro creado correctamente.";
                }
                else
                {
                    await api.PutAsync<object>(
                        "Catalogo/mantenimiento/"
                            + catalogo
                            + "/"
                            + hfId.Value,
                        datos,
                        idUsuario
                    );

                    lblMensaje.Text =
                        "Registro actualizado correctamente.";
                }

                lblMensaje.CssClass =
                    "mensajeCorrecto";

                Limpiar();

                await CargarCatalogo();
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass =
                    "mensajeError";

                lblMensaje.Text =
                    "Error: " + ex.Message;
            }
        }


        private object CrearObjetoEnvio(
            string catalogo)
        {
            switch (catalogo)
            {
                case "genero":
                case "ciudad":
                case "ocupacion":
                case "nivel-educativo":
                case "tipo-prestamo":
                case "medio-contratacion":

                    return new
                    {
                        nombre =
                            txtNombre.Text.Trim()
                    };


                case "plazo":

                    return new
                    {
                        meses =
                            Convert.ToInt32(
                                txtMeses.Text
                            )
                    };


                case "rango-edad":

                    return new
                    {
                        minimo =
                            Convert.ToDecimal(
                                txtMinimo.Text,
                                CultureInfo
                                    .InvariantCulture
                            ),

                        maximo =
                            Convert.ToDecimal(
                                txtMaximo.Text,
                                CultureInfo
                                    .InvariantCulture
                            ),

                        descripcion =
                            txtDescripcion.Text
                                .Trim()
                    };


                case "rango-ingresos":
                case "monto-rango":
                case "tasa-interes":
                case "capacidad-pago":

                    decimal? maximo = null;

                    if (!string.IsNullOrWhiteSpace(
                        txtMaximo.Text))
                    {
                        maximo =
                            Convert.ToDecimal(
                                txtMaximo.Text,
                                CultureInfo
                                    .InvariantCulture
                            );
                    }

                    return new
                    {
                        minimo =
                            Convert.ToDecimal(
                                txtMinimo.Text,
                                CultureInfo
                                    .InvariantCulture
                            ),

                        maximo = maximo,

                        descripcion =
                            txtDescripcion.Text
                                .Trim()
                    };


                case "historial-crediticio":

                    return new
                    {
                        tienePrestamosPrevios =
                            chkPrestamosPrevios
                                .Checked,

                        haMorado =
                            chkHaMorado.Checked,

                        descripcion =
                            txtDescripcionHistorial
                                .Text.Trim()
                    };


                default:

                    throw new Exception(
                        "Catálogo no válido."
                    );
            }
        }


        protected async void
            gvCatalogos_RowCommand(
                object sender,
                System.Web.UI.WebControls
                    .GridViewCommandEventArgs e)
        {
            try
            {
                int index =
                    Convert.ToInt32(
                        e.CommandArgument
                    );

                if (gvCatalogos
                    .DataKeys[index] == null)
                {
                    return;
                }

                int id =
                    Convert.ToInt32(
                        gvCatalogos
                            .DataKeys[index]
                            .Value
                    );

                if (e.CommandName
                    == "EditarRegistro")
                {
                    CargarFilaParaEditar(
                        gvCatalogos.Rows[index]
                    );

                    hfId.Value =
                        id.ToString();

                    btnGuardar.Text =
                        "Actualizar";

                    return;
                }


                if (e.CommandName
                    == "EliminarRegistro")
                {
                    int idUsuario =
                        Convert.ToInt32(
                            Session["IdUsuario"]
                        );

                    await api.DeleteAsync(
                        "Catalogo/mantenimiento/"
                            + ddlCatalogo
                                .SelectedValue
                            + "/"
                            + id,
                        idUsuario
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Registro eliminado correctamente.";

                    await CargarCatalogo();
                }

                if (e.CommandName

                  == "RestaurarRegistro")
                {
                    int idUsuario =
                        Convert.ToInt32(
                            Session["IdUsuario"]
                        );

                    await api.PutAsync<object>(
                        "Catalogo/restaurar/"
                            + ddlCatalogo.SelectedValue
                            + "/"
                            + id,
                        new { },
                        idUsuario
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Registro restaurado correctamente.";

                    await CargarCatalogo();
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


        private void CargarFilaParaEditar(
            System.Web.UI.WebControls
                .GridViewRow fila)
        {
            var data =
                ObtenerDatosFila(fila);

            string catalogo =
                ddlCatalogo.SelectedValue;

            switch (catalogo)
            {
                case "genero":
                case "ciudad":
                case "ocupacion":
                case "nivel-educativo":
                case "tipo-prestamo":
                case "medio-contratacion":

                    txtNombre.Text =
                        ObtenerDato(
                            data,
                            "nombre"
                        );

                    break;


                case "plazo":

                    txtMeses.Text =
                        ObtenerDato(
                            data,
                            "meses"
                        );

                    break;


                case "rango-edad":
                case "rango-ingresos":
                case "monto-rango":
                case "tasa-interes":
                case "capacidad-pago":

                    txtMinimo.Text =
                        ObtenerDato(
                            data,
                            "minimo"
                        );

                    txtMaximo.Text =
                        ObtenerDato(
                            data,
                            "maximo"
                        );

                    txtDescripcion.Text =
                        ObtenerDato(
                            data,
                            "descripcion"
                        );

                    break;


                case "historial-crediticio":

                    chkPrestamosPrevios.Checked =
                        ConvertirBooleano(
                            ObtenerDato(
                                data,
                                "tienePrestamosPrevios"
                            )
                        );

                    chkHaMorado.Checked =
                        ConvertirBooleano(
                            ObtenerDato(
                                data,
                                "haMorado"
                            )
                        );

                    txtDescripcionHistorial.Text =
                        ObtenerDato(
                            data,
                            "descripcion"
                        );

                    break;
            }
        }


        private Dictionary<string, string>
            ObtenerDatosFila(
                System.Web.UI.WebControls
                    .GridViewRow fila)
        {
            var datos =
                new Dictionary<
                    string,
                    string
                >(
                    StringComparer
                        .OrdinalIgnoreCase
                );

            for (
                int i = 0;
                i < gvCatalogos.Columns.Count;
                i++)
            {
                if (gvCatalogos.Columns[i]
                    is System.Web.UI.WebControls
                        .BoundField campo)
                {
                    datos[campo.DataField] =
                        fila.Cells[i].Text;
                }
            }

            /*
             * Las columnas automáticas del GridView
             * no aparecen en Columns, así que usamos
             * el DataTable guardado en ViewState
             * en el siguiente paso si fuera necesario.
             */

            return datos;
        }


        private string ObtenerDato(
            Dictionary<string, string> datos,
            string nombre)
        {
            if (datos.ContainsKey(nombre))
            {
                return System.Web.HttpUtility
                    .HtmlDecode(datos[nombre]);
            }

            return "";
        }


        private bool ConvertirBooleano(
            string valor)
        {
            bool resultado;

            return bool.TryParse(
                valor,
                out resultado
            )
                && resultado;
        }


        protected void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            Limpiar();
        }


        private void Limpiar()
        {
            hfId.Value = "";

            txtNombre.Text = "";
            txtMeses.Text = "";

            txtMinimo.Text = "";
            txtMaximo.Text = "";
            txtDescripcion.Text = "";

            chkPrestamosPrevios.Checked =
                false;

            chkHaMorado.Checked =
                false;

            txtDescripcionHistorial.Text =
                "";

            btnGuardar.Text =
                "Guardar";
        }
    }
}