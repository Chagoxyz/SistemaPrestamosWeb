<%@ Page Language="C#" Async="true"
    AutoEventWireup="true"
    CodeBehind="GestionCRUD.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.GestionCRUD" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Gestión de Catálogos</title>

    <link href="../styles/GestionCRUD.css"
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

            <h2>Gestión de Catálogos</h2>


            <!-- SELECCIONAR CATÁLOGO -->

            <div class="fila">

                <asp:Label
                    ID="Label1"
                    runat="server"
                    Text="Seleccione el catálogo:">
                </asp:Label>

                <br />

                <asp:DropDownList
                    ID="ddlCatalogo"
                    runat="server"
                    CssClass="campo"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCatalogo_SelectedIndexChanged">

                    <asp:ListItem
                        Text="Géneros"
                        Value="genero" />

                    <asp:ListItem
                        Text="Ciudades"
                        Value="ciudad" />

                    <asp:ListItem
                        Text="Ocupaciones"
                        Value="ocupacion" />

                    <asp:ListItem
                        Text="Niveles educativos"
                        Value="nivel-educativo" />

                    <asp:ListItem
                        Text="Tipos de préstamo"
                        Value="tipo-prestamo" />

                    <asp:ListItem
                        Text="Medios de contratación"
                        Value="medio-contratacion" />

                    <asp:ListItem
                        Text="Plazos"
                        Value="plazo" />

                    <asp:ListItem
                        Text="Rangos de edad"
                        Value="rango-edad" />

                    <asp:ListItem
                        Text="Rangos de ingresos"
                        Value="rango-ingresos" />

                    <asp:ListItem
                        Text="Rangos de monto"
                        Value="monto-rango" />

                    <asp:ListItem
                        Text="Tasas de interés"
                        Value="tasa-interes" />

                    <asp:ListItem
                        Text="Capacidad de pago"
                        Value="capacidad-pago" />

                    <asp:ListItem
                        Text="Historial crediticio"
                        Value="historial-crediticio" />

                </asp:DropDownList>

            </div>


            <!-- INCLUIR ELIMINADOS -->

            <div class="fila">

                <asp:CheckBox
                    ID="chkIncluirEliminados"
                    runat="server"
                    Text=" Incluir eliminados"
                    AutoPostBack="true"
                    OnCheckedChanged="chkIncluirEliminados_CheckedChanged" />

            </div>


            <asp:HiddenField
                ID="hfId"
                runat="server" />


            <!-- CATÁLOGOS SIMPLES -->

            <asp:Panel
                ID="pnlNombre"
                runat="server">

                <div class="fila">

                    <asp:Label
                        ID="lblNombre"
                        runat="server"
                        Text="Nombre:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        CssClass="campo">
                    </asp:TextBox>

                </div>

            </asp:Panel>


            <!-- PLAZO -->

            <asp:Panel
                ID="pnlPlazo"
                runat="server"
                Visible="false">

                <div class="fila">

                    <asp:Label
                        ID="Label2"
                        runat="server"
                        Text="Meses:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtMeses"
                        runat="server"
                        CssClass="campo"
                        TextMode="Number">
                    </asp:TextBox>

                </div>

            </asp:Panel>


            <!-- RANGOS -->

            <asp:Panel
                ID="pnlRango"
                runat="server"
                Visible="false">

                <div class="fila">

                    <asp:Label
                        ID="Label3"
                        runat="server"
                        Text="Mínimo:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtMinimo"
                        runat="server"
                        CssClass="campo">
                    </asp:TextBox>

                </div>


                <div class="fila">

                    <asp:Label
                        ID="Label4"
                        runat="server"
                        Text="Máximo:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtMaximo"
                        runat="server"
                        CssClass="campo">
                    </asp:TextBox>

                </div>


                <div class="fila">

                    <asp:Label
                        ID="Label5"
                        runat="server"
                        Text="Descripción:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtDescripcion"
                        runat="server"
                        CssClass="campo">
                    </asp:TextBox>

                </div>

            </asp:Panel>


            <!-- HISTORIAL CREDITICIO -->

            <asp:Panel
                ID="pnlHistorial"
                runat="server"
                Visible="false">

                <div class="fila">

                    <asp:CheckBox
                        ID="chkPrestamosPrevios"
                        runat="server"
                        Text="Tiene préstamos previos" />

                </div>


                <div class="fila">

                    <asp:CheckBox
                        ID="chkHaMorado"
                        runat="server"
                        Text="Ha presentado mora" />

                </div>


                <div class="fila">

                    <asp:Label
                        ID="Label6"
                        runat="server"
                        Text="Descripción:">
                    </asp:Label>

                    <br />

                    <asp:TextBox
                        ID="txtDescripcionHistorial"
                        runat="server"
                        CssClass="campo">
                    </asp:TextBox>

                </div>

            </asp:Panel>


            <!-- BOTONES -->

            <div class="fila">

                <asp:Button
                    ID="btnGuardar"
                    runat="server"
                    Text="Guardar"
                    CssClass="boton guardar"
                    OnClick="btnGuardar_Click" />

                <asp:Button
                    ID="btnLimpiar"
                    runat="server"
                    Text="Limpiar"
                    CssClass="boton editar"
                    OnClick="btnLimpiar_Click" />

            </div>


            <!-- TABLA -->

            <asp:GridView
                ID="gvCatalogos"
                runat="server"
                AutoGenerateColumns="True"
                CssClass="tabla"
                OnRowCommand="gvCatalogos_RowCommand">

                <Columns>

                    <asp:TemplateField HeaderText="Acciones">

                        <ItemTemplate>

                            <asp:Button
                                ID="btnEditar"
                                runat="server"
                                Text="Editar"
                                CssClass="boton editar"
                                CommandName="EditarRegistro"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Visible='<%# Convert.ToBoolean(Eval("Activo")) %>' />


                            <asp:Button
                                ID="btnEliminar"
                                runat="server"
                                Text="Eliminar"
                                CssClass="boton eliminar"
                                CommandName="EliminarRegistro"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Visible='<%# Convert.ToBoolean(Eval("Activo")) %>'
                                OnClientClick="return confirm('¿Desea eliminar este registro?');" />


                            <asp:Button
                                ID="btnRestaurar"
                                runat="server"
                                Text="Restaurar"
                                CssClass="boton restaurar"
                                CommandName="RestaurarRegistro"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Visible='<%# !Convert.ToBoolean(Eval("Activo")) %>' />

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>


            <!-- MENSAJE -->

            <asp:Label
                ID="lblMensaje"
                runat="server">
            </asp:Label>


        </div>

    </div>

</form>

</body>

</html>