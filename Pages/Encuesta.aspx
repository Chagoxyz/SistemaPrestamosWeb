<%@ Page Language="C#"
    AutoEventWireup="true"
    Async="true"
    CodeBehind="Encuesta.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.Encuesta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Encuesta</title>

    <link href="../styles/Encuesta.css"
        rel="stylesheet"
        type="text/css" />

    <link href="../styles/Layout.css"
        rel="stylesheet"
        type="text/css" />

</head>

<body>

<form id="form1" runat="server">

   <div class="menu">

    <h2>Préstamos</h2>

    <a id="lnkDashboard"
       runat="server"
       href="Dashboard.aspx">
        Dashboard
    </a>

    <a id="lnkEncuesta"
       runat="server"
       href="Encuesta.aspx">
        Nueva Encuesta
    </a>

    <a id="lnkGestionEncuestas"
       runat="server"
       href="GestionEncuestas.aspx">
        Gestionar Encuestas
    </a>

    <a id="lnkCatalogos"
       runat="server"
       href="GestionCRUD.aspx">
        Catálogos
    </a>

    <a id="lnkUsuarios"
       runat="server"
       href="Usuarios.aspx">
        Usuarios
    </a>

    <a id="lnkReportes"
       runat="server"
       href="Reportes.aspx">
        Reportes
    </a>

    <a href="Login.aspx">
        Cerrar Sesión
    </a>

</div>


    <div class="contenido">

        <div class="encabezado">

            <h2>Formulario de Encuesta</h2>

            <p>
                Complete las preguntas y envíe la información a la API.
            </p>

        </div>


        <div class="card">

            <asp:PlaceHolder
                ID="phEncuesta"
                runat="server">
            </asp:PlaceHolder>

            <br />

            <asp:Button
                ID="btnGuardar"
                runat="server"
                Text="Guardar Encuesta"
                CssClass="boton"
                OnClick="btnGuardar_Click" />

            <br />
            <br />

            <asp:Label
                ID="lblMensaje"
                runat="server">
            </asp:Label>

        </div>

    </div>

</form>

</body>

</html>