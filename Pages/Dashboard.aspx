<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Dashboard</title>

    <link href="../styles/Dashboard.css"
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

        <asp:LinkButton
            ID="btnCerrarSesion"
            runat="server"
            OnClick="btnCerrarSesion_Click">
            Cerrar Sesión
        </asp:LinkButton>

    </div>


    <div class="contenido">

        <div class="encabezado">

            <h2>Dashboard</h2>

        </div>


        <div class="card">

            <h3>Bienvenido al Sistema</h3>

            <asp:Label
                ID="lblUsuario"
                runat="server">
            </asp:Label>

        </div>

    </div>

</form>

</body>

</html>