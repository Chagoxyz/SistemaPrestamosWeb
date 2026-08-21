<%@ Page Language="C#" Async="true"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Inicio de Sesión</title>

    <link href="../styles/Login.css" rel="stylesheet" type="text/css" />

</head>

<body>

<form id="form1" runat="server">

    <div class="login">

        <h2>Sistema de Préstamos</h2>

        <asp:TextBox
            ID="txtUsuario"
            runat="server"
            placeholder="Usuario">
        </asp:TextBox>

        <br />

        <asp:TextBox
            ID="txtClave"
            runat="server"
            TextMode="Password"
            placeholder="Contraseña">
        </asp:TextBox>

        <br />

        <asp:Button
            ID="btnIngresar"
            runat="server"
            CssClass="boton"
            Text="Ingresar"
            OnClick="btnIngresar_Click" />

        <br />

        <asp:Label
            ID="lblMensaje"
            runat="server">
        </asp:Label>

    </div>

</form>

</body>

</html>