<%@ Page Language="C#"
    AutoEventWireup="true"
    Async="true"
    CodeBehind="Usuarios.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.Usuarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Gestión de Usuarios</title>

    <link href="../styles/Usuarios.css"
        rel="stylesheet"
        type="text/css" />

    <link href="../styles/Layout.css"
        rel="stylesheet"
        type="text/css" />

</head>

<body>

<form id="form1" runat="server">

    <!-- MENÚ -->

    <div class="menu">

        <h2>Préstamos</h2>

        <a href="Dashboard.aspx">
            Dashboard
        </a>

        <a href="Encuesta.aspx">
            Nueva Encuesta
        </a>

        <a href="GestionEncuestas.aspx">
            Gestionar Encuestas
        </a>

        <a href="GestionCRUD.aspx">
            Catálogos
        </a>

        <a href="Usuarios.aspx">
            Usuarios
        </a>

        <a href="Reportes.aspx">
            Reportes
        </a>

        <a href="Login.aspx">
            Cerrar Sesión
        </a>

    </div>


    <!-- CONTENIDO -->

    <div class="contenido">

        <div class="encabezado">

            <h2>Gestión de Usuarios</h2>

        </div>


        <div class="card">

            <h3>Registrar usuario</h3>

            <asp:HiddenField
                ID="hfIdUsuario"
                runat="server" />

            <asp:TextBox
                ID="txtNombre"
                runat="server"
                placeholder="Nombre completo">
            </asp:TextBox>

            <asp:TextBox
                ID="txtNombreUsuario"
                runat="server"
                placeholder="Nombre de usuario">
            </asp:TextBox>

            <asp:TextBox
                ID="txtEmail"
                runat="server"
                placeholder="Correo electrónico">
            </asp:TextBox>

            <asp:TextBox
                ID="txtClave"
                runat="server"
                TextMode="Password"
                placeholder="Contraseña">
            </asp:TextBox>

            <asp:DropDownList
                ID="ddlRol"
                runat="server">
            </asp:DropDownList>

            <br />

            <asp:Button
                ID="btnGuardar"
                runat="server"
                CssClass="boton"
                Text="Guardar"
                OnClick="btnGuardar_Click" />

            <asp:Button
                ID="btnLimpiar"
                runat="server"
                CssClass="botonSecundario"
                Text="Limpiar"
                OnClick="btnLimpiar_Click" />

            <br />
            <br />

            <asp:Label
                ID="lblMensaje"
                runat="server">
            </asp:Label>

        </div>


        <br />


        <div class="card">

            <h3>Usuarios registrados</h3>

            <asp:GridView
                ID="gvUsuarios"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="tabla"
                OnRowCommand="gvUsuarios_RowCommand">

                <Columns>

                    <asp:BoundField
                        DataField="IdUsuario"
                        HeaderText="ID" />

                    <asp:BoundField
                        DataField="Nombre"
                        HeaderText="Nombre" />

                    <asp:BoundField
                        DataField="NombreUsuario"
                        HeaderText="Usuario" />

                    <asp:BoundField
                        DataField="Email"
                        HeaderText="Correo" />

                    <asp:BoundField
                        DataField="Rol"
                        HeaderText="Rol" />


                    <asp:ButtonField
                        Text="Editar"
                        CommandName="EditarUsuario"
                        ButtonType="Button" />

                    <asp:ButtonField
                        Text="Eliminar"
                        CommandName="EliminarUsuario"
                        ButtonType="Button" />

                </Columns>

            </asp:GridView>

        </div>

    </div>

</form>

</body>

</html>