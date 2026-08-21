using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class GestionEncuestas : System.Web.UI.Page
    {
        private readonly ApiService api = new ApiService();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (Session["Rol"]?.ToString() != "Administrador")
            {
                Response.Redirect("Dashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                await CargarEncuestas();
            }
        }

        private async Task CargarEncuestas()
        {
            try
            {
                var encuestas =
                    await api.GetAsync<List<Dictionary<string, object>>>(
                        "Encuesta"
                    );

                DataTable tabla = new DataTable();

                tabla.Columns.Add("idEncuesta");
                tabla.Columns.Add("fecha");
                tabla.Columns.Add("cliente");
                tabla.Columns.Add("tipoPrestamo");
                tabla.Columns.Add("activo");

                foreach (var encuesta in encuestas)
                {
                    DataRow fila = tabla.NewRow();

                    fila["idEncuesta"] =
                        ObtenerValor(
                            encuesta,
                            "idEncuesta",
                            "IdEncuesta"
                        );

                    fila["fecha"] =
                        ObtenerValor(
                            encuesta,
                            "fecha",
                            "Fecha"
                        );

                    fila["cliente"] =
                        ObtenerValor(
                            encuesta,
                            "cliente",
                            "Cliente"
                        );

                    fila["tipoPrestamo"] =
                        ObtenerValor(
                            encuesta,
                            "tipoPrestamo",
                            "TipoPrestamo"
                        );

                    fila["activo"] =
                        ObtenerValor(
                            encuesta,
                            "activo",
                            "Activo"
                        );

                    tabla.Rows.Add(fila);
                }

                gvEncuestas.DataSource = tabla;
                gvEncuestas.DataBind();

                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "mensajeError";

                lblMensaje.Text =
                    "Error al cargar las encuestas: "
                    + ex.Message;
            }
        }

        private object ObtenerValor(
            Dictionary<string, object> datos,
            string nombreCamel,
            string nombrePascal)
        {
            if (datos.ContainsKey(nombreCamel))
            {
                return datos[nombreCamel];
            }

            if (datos.ContainsKey(nombrePascal))
            {
                return datos[nombrePascal];
            }

            return "";
        }

        protected async void gvEncuestas_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                int idEncuesta =
                    Convert.ToInt32(
                        e.CommandArgument
                    );

                int idUsuario =
                    Convert.ToInt32(
                        Session["IdUsuario"]
                    );

                if (e.CommandName == "EditarEncuesta")
                {
                    Response.Redirect(
                        "Encuesta.aspx?id=" + idEncuesta,
                        false
                    );

                    Context.ApplicationInstance
                        .CompleteRequest();

                    return;
                }

                if (e.CommandName == "EliminarEncuesta")
                {
                    await api.DeleteAsync(
                        "Encuesta/" + idEncuesta,
                        idUsuario
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Encuesta eliminada correctamente.";

                    await CargarEncuestas();
                }

                if (e.CommandName == "RestaurarEncuesta")
                {
                    await api.PutAsync<object>(
                        "Encuesta/restaurar/"
                            + idEncuesta,
                        new { },
                        idUsuario
                    );

                    lblMensaje.CssClass =
                        "mensajeCorrecto";

                    lblMensaje.Text =
                        "Encuesta restaurada correctamente.";

                    await CargarEncuestas();
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
    }
}