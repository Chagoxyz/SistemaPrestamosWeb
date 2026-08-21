<%@ Page Language="C#" Async="true"
    AutoEventWireup="true"
    CodeBehind="GestionEncuestas.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.GestionEncuestas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Gestión de Encuestas</title>

    <link href="../styles/GestionEncuestas.css"
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

        <div class="contenedor">

            <h2>Gestión de Encuestas</h2>

            <p>
                Consulte y administre las encuestas registradas.
            </p>


            <asp:GridView
                ID="gvEncuestas"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="tabla"
                OnRowCommand="gvEncuestas_RowCommand">

                <Columns>

                    <asp:BoundField
                        DataField="idEncuesta"
                        HeaderText="ID" />

                    <asp:BoundField
                        DataField="fecha"
                        HeaderText="Fecha" />

                    <asp:BoundField
                        DataField="cliente"
                        HeaderText="Cliente" />

                    <asp:BoundField
                        DataField="tipoPrestamo"
                        HeaderText="Tipo de Préstamo" />


                    <asp:TemplateField HeaderText="Estado">

                        <ItemTemplate>

                            <%# Convert.ToBoolean(Eval("activo"))
                                ? "Activa"
                                : "Eliminada" %>

                        </ItemTemplate>

                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Acciones">

                        <ItemTemplate>

                            <asp:Button
                                ID="btnEditar"
                                runat="server"
                                Text="Editar"
                                CssClass="boton editar"
                                CommandName="EditarEncuesta"
                                CommandArgument='<%# Eval("idEncuesta") %>'
                                Visible='<%# Convert.ToBoolean(Eval("activo")) %>' />


                            <asp:Button
                                ID="btnEliminar"
                                runat="server"
                                Text="Eliminar"
                                CssClass="boton eliminar"
                                CommandName="EliminarEncuesta"
                                CommandArgument='<%# Eval("idEncuesta") %>'
                                Visible='<%# Convert.ToBoolean(Eval("activo")) %>'
                                OnClientClick="return confirm('¿Desea eliminar esta encuesta?');" />


                            <asp:Button
                                ID="btnRestaurar"
                                runat="server"
                                Text="Restaurar"
                                CssClass="boton restaurar"
                                CommandName="RestaurarEncuesta"
                                CommandArgument='<%# Eval("idEncuesta") %>'
                                Visible='<%# !Convert.ToBoolean(Eval("activo")) %>' />

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>


            <asp:Label
                ID="lblMensaje"
                runat="server">
            </asp:Label>

        </div>

    </div>

</form>

</body>

</html>