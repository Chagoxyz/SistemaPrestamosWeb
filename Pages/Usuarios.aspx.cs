using System;
using System.Collections.Generic;
using SistemaPrestamosWeb.DTOs;
using SistemaPrestamosWeb.Servicios;

namespace SistemaPrestamosWeb.Pages
{
    public partial class Usuarios : System.Web.UI.Page
    {
        private readonly ApiService api = new ApiService();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Rol"]?.ToString() != "Administrador")
            {
                Response.Redirect("~/Pages/Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                await CargarRoles();
                await CargarUsuarios();
            }
        }

        private async System.Threading.Tasks.Task CargarRoles()
        {
            var roles = await api.GetAsync<List<RolDTO>>("roles");

            ddlRol.DataSource = roles;
            ddlRol.DataTextField = "nombre";
            ddlRol.DataValueField = "idRol";
            ddlRol.DataBind();
        }

        private async System.Threading.Tasks.Task CargarUsuarios()
        {
            var usuarios = await api.GetAsync<List<UsuarioDTO>>("Usuario");

            gvUsuarios.DataSource = usuarios;
            gvUsuarios.DataBind();
        }

        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuarioAuditoria =
                    Convert.ToInt32(Session["IdUsuario"]);

                var usuario = new UsuarioRequestDTO
                {
                    Nombre = txtNombre.Text.Trim(),
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Clave = txtClave.Text.Trim(),
                    IdRol = Convert.ToInt32(ddlRol.SelectedValue)
                };

                if (string.IsNullOrEmpty(hfIdUsuario.Value))
                {
                    await api.PostAsync<object>(
                        "Usuario",
                        usuario,
                        idUsuarioAuditoria
                    );

                    lblMensaje.Text =
                        "Usuario registrado correctamente.";
                }
                else
                {
                    await api.PutAsync<object>(
                        "Usuario/" + hfIdUsuario.Value,
                        usuario,
                        idUsuarioAuditoria
                    );

                    lblMensaje.Text =
                        "Usuario actualizado correctamente.";
                }

                lblMensaje.CssClass = "mensajeCorrecto";

                Limpiar();
                await CargarUsuarios();
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "mensajeError";

                if (ex.Message.Contains("UQ_Usuario_NombreUsuario") ||
                    ex.Message.Contains("duplicate key"))
                {
                    lblMensaje.Text =
                        "Ese nombre de usuario ya está registrado. " +
                        "Por favor utilice otro.";
                }
                else
                {
                    lblMensaje.Text =
                        "No se pudo guardar el usuario. " +
                        "Revise la información e intente nuevamente.";
                }
            }
        }

        protected async void gvUsuarios_RowCommand(
            object sender,
            System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);

                int idUsuario =
                    Convert.ToInt32(
                        gvUsuarios.Rows[index].Cells[0].Text
                    );

                int idUsuarioAuditoria =
                    Convert.ToInt32(Session["IdUsuario"]);

                if (e.CommandName == "EditarUsuario")
                {
                    hfIdUsuario.Value = idUsuario.ToString();

                    txtNombre.Text =
                        gvUsuarios.Rows[index].Cells[1].Text;

                    txtNombreUsuario.Text =
                        gvUsuarios.Rows[index].Cells[2].Text;

                    txtEmail.Text =
                        gvUsuarios.Rows[index].Cells[3].Text;

                    txtClave.Text = "";

                    btnGuardar.Text = "Actualizar";
                }

                if (e.CommandName == "EliminarUsuario")
                {
                    await api.DeleteAsync(
                        "Usuario/" + idUsuario,
                        idUsuarioAuditoria
                    );

                    await CargarUsuarios();

                    lblMensaje.CssClass = "mensajeCorrecto";
                    lblMensaje.Text =
                        "Usuario eliminado correctamente.";
                }
            }
            catch
            {
                lblMensaje.CssClass = "mensajeError";
                lblMensaje.Text =
                    "Ocurrió un error al procesar la operación.";
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            hfIdUsuario.Value = "";
            txtNombre.Text = "";
            txtNombreUsuario.Text = "";
            txtEmail.Text = "";
            txtClave.Text = "";
            btnGuardar.Text = "Guardar";
        }
    }
}