<%@ Page Language="C#"
    AutoEventWireup="true"
    Async="true"
    CodeBehind="Reportes.aspx.cs"
    Inherits="SistemaPrestamosWeb.Pages.Reportes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Reportes</title>

    <link href="../styles/Reportes.css"
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


    <!-- CONTENIDO -->

    <div class="contenido">

        <div class="encabezado">

            <h2>Reportes</h2>

            <p>
                Seleccione un reporte para visualizar
                la información registrada.
            </p>

        </div>


        <!-- SELECCIÓN -->

        <div class="card">

            <label>
                Seleccione el reporte:
            </label>

            <br />

            <asp:DropDownList
                ID="ddlReporte"
                runat="server"
                CssClass="campo">
            </asp:DropDownList>

            <br />
            <br />

            <asp:Button
                ID="btnConsultar"
                runat="server"
                Text="Consultar"
                CssClass="boton"
                OnClick="btnConsultar_Click" />

            <br />
            <br />

            <asp:Label
                ID="lblMensaje"
                runat="server">
            </asp:Label>

        </div>


        <br />


        <!-- TABLA -->

        <div class="card">

            <h3>
                Resultado del reporte
            </h3>

            <asp:GridView
                ID="gvReporte"
                runat="server"
                CssClass="tabla"
                AutoGenerateColumns="True">
            </asp:GridView>

        </div>


        <br />


        <!-- GRÁFICO -->

        <div class="card">

            <h3>
                Gráfico del reporte
            </h3>

            <div style="
                width: 100%;
                max-width: 900px;
                height: 450px;
                margin: auto;">

                <canvas id="graficoReporte">
                </canvas>

            </div>

        </div>


        <!-- DATOS PARA JAVASCRIPT -->

        <asp:HiddenField
            ID="hfLabelsGrafico"
            runat="server" />

        <asp:HiddenField
            ID="hfValoresGrafico"
            runat="server" />

        <asp:HiddenField
            ID="hfTituloGrafico"
            runat="server" />

        <asp:HiddenField
            ID="hfTipoGrafico"
            runat="server" />

    </div>

</form>


<!-- CHART JS -->

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<script type="text/javascript">

    let graficoActual = null;

    function cargarGrafico() {

        const labelsRaw =
            document.getElementById(
                '<%= hfLabelsGrafico.ClientID %>'
            ).value;

        const valoresRaw =
            document.getElementById(
                '<%= hfValoresGrafico.ClientID %>'
            ).value;

        const titulo =
            document.getElementById(
                '<%= hfTituloGrafico.ClientID %>'
            ).value;

        const tipo =
            document.getElementById(
                '<%= hfTipoGrafico.ClientID %>'
            ).value || 'bar';


        if (!labelsRaw ||
            !valoresRaw) {

            return;
        }


        const labels =
            JSON.parse(labelsRaw);

        const valores =
            JSON.parse(valoresRaw);


        if (labels.length === 0 ||
            valores.length === 0) {

            return;
        }


        const canvas =
            document.getElementById(
                'graficoReporte'
            );


        if (!canvas) {

            return;
        }


        if (graficoActual !== null) {

            graficoActual.destroy();
        }


        graficoActual =
            new Chart(
                canvas,
                {
                    type: tipo,

                    data: {

                        labels: labels,

                        datasets: [
                            {
                                label: titulo,
                                data: valores,
                                borderWidth: 2
                            }
                        ]
                    },

                    options: {

                        responsive: true,

                        maintainAspectRatio: false,

                        plugins: {

                            title: {
                                display: true,
                                text: titulo
                            },

                            legend: {
                                display: true
                            },

                            tooltip: {
                                enabled: true
                            }
                        },

                        scales:
                            tipo === 'pie'
                                ? {}
                                : {
                                    y: {
                                        beginAtZero: true
                                    }
                                }
                    }
                }
            );
    }


    window.addEventListener(
        'load',
        cargarGrafico
    );

</script>

</body>

</html>