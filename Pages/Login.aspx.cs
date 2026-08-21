using System;
using SistemaPrestamosWeb.DTOs;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected async void btnIngresar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                ApiService api = new ApiService();

                LoginRequestDTO login =
                    new LoginRequestDTO
                    {
                        nombreUsuario =
                            txtUsuario.Text.Trim(),

                        clave =
                            txtClave.Text.Trim()
                    };

                UsuarioDTO usuario =
                    await api.PostAsync<UsuarioDTO>(
                        "Usuario/login",
                        login
                    );

                Session["Usuario"] =
                    usuario.nombre;

                Session["IdUsuario"] =
                    usuario.idUsuario;

                Session["Rol"] =
                    usuario.rol;

                Response.Redirect(
                    "Dashboard.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch
            {
                lblMensaje.CssClass =
                    "mensajeError";

                lblMensaje.Text =
                    "Usuario o contraseña incorrectos.";
            }
        }
    }
}