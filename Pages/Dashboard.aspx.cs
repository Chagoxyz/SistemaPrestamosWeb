using System;

namespace SistemaPrestamosWeb.Pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            string rol =
                Session["Rol"]?.ToString()?.Trim();

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

            bool esEncuestador =
                string.Equals(
                    rol,
                    "Encuestador",
                    StringComparison.OrdinalIgnoreCase
                );

            // Todos
            lnkDashboard.Visible = true;

            // Todos los roles registrados
            lnkEncuesta.Visible =
                esAdministrador ||
                esConsultor ||
                esEncuestador;

            // Solo administrador
            lnkGestionEncuestas.Visible =
                esAdministrador;

            lnkCatalogos.Visible =
                esAdministrador;

            lnkUsuarios.Visible =
                esAdministrador;

            // Administrador y consultor
            lnkReportes.Visible =
                esAdministrador ||
                esConsultor;

            lblUsuario.Text =
                "Usuario: " +
                Session["Usuario"] +
                " | Rol: " +
                rol;
        }


        protected void btnCerrarSesion_Click(
            object sender,
            EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect(
                "Login.aspx",
                false
            );

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}