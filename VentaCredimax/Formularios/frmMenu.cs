using CEntidades;
using CEntidades.DTOs;
using CLogica;
using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmMenu : Form
    {
        private GestorVenta _gestorVenta = new GestorVenta();
        private GestorReportes _gestorReportes = new GestorReportes();
        public frmMenu()
        {
            InitializeComponent();
      
        }
        private void frmMenu_Load(object sender, EventArgs e)
        {
            
            cbOrdenarPor.SelectedIndex = 0;
            ListarVentas();
            OcultarColumnas();
            EstiloDataGrid();
            FormatearFilas();   
        }

        private void btnGestionCliente_Click(object sender, EventArgs e)
        {
            frmCliente frmCliente = new frmCliente();
            frmCliente.ShowDialog();
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            pSubMenuInformes.Visible = true;
        }

        private void btnInformeCuotas_Click(object sender, EventArgs e)
        {
            // Obtener el IdCliente de la columna "IdCliente"
            int idCliente = Convert.ToInt32(dgvVentasMenu?.CurrentRow?.Cells["IdCliente"]?.Value);
            if (idCliente > 0)
            {
                pSubMenuInformes.Visible = false;
                ImprimirHistorialVentasPorCliente(idCliente);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una venta del cliente para generar su historial de reporte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnInformeVentas_Click(object sender, EventArgs e)
        {
            pSubMenuInformes.Visible = false;
            frmVerVentasPorFecha frmReporteVentasPorFecha = new frmVerVentasPorFecha();
            frmReporteVentasPorFecha.ShowDialog();
        }

        private void ListarVentas()
        {
            string criterioSeleccionado = "";
            if (cbOrdenarPor.Items.Count > 0)
            {
                criterioSeleccionado = cbOrdenarPor.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un criterio de ordenación.");
            }
            dgvVentasMenu.DataSource = _gestorVenta.ListarVentasMenu(criterioSeleccionado);
            OcultarColumnas();
            FormatearFilas();
            EstiloDataGrid();
            
        }

        private void FiltrarVentasPorCliente()
        {
            dgvVentasMenu.DataSource = _gestorVenta.FiltrarVentasPorCliente(txtBuscar.Text);
            OcultarColumnas();
        }

        private void FiltrarVentasPorArticulo()
        {
            dgvVentasMenu.DataSource = _gestorVenta.FiltrarVentasPorArticulo(txtBuscar.Text);
            OcultarColumnas();
        }

        private void Buscar()
       {
            if (cbBuscarPor.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un criterio de búsqueda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedValue = cbBuscarPor.SelectedItem.ToString();
            switch (selectedValue)
            {
                case "Cliente":
                    FiltrarVentasPorCliente();
                    break;
                case "Articulo":
                    FiltrarVentasPorArticulo();
                    break;
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void OcultarColumnas()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                dgvVentasMenu.Columns["VentaId"].Visible = false;
                dgvVentasMenu.Columns["IdCliente"].Visible = false;
                dgvVentasMenu.Columns["IdArticulo"].Visible = false;
                dgvVentasMenu.Columns["FechaAnulacion"].Visible = false;
                dgvVentasMenu.Columns["CuotasVencidas"].Visible = false;
            }
        }

        private void EstiloDataGrid()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                dgvVentasMenu.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
                dgvVentasMenu.Columns["FormaDePago"].HeaderText = "Frecuencia de pago";
                dgvVentasMenu.Columns["FechaDeInicio"].HeaderText = "Fecha compra";
                dgvVentasMenu.Columns["FechaDeCancelacion"].HeaderText = "Cancelacion compra";
                dgvVentasMenu.Columns["Articulo"].HeaderText = "Articulo";
            }
        }

        private void dgvVentasMenu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validar que haya una fila seleccionada
            if (dgvVentasMenu.CurrentRow != null)
            {
                // Obtener el ID de la venta desde la fila seleccionada
                int ventaId = Convert.ToInt32(dgvVentasMenu.CurrentRow.Cells["VentaId"].Value);

                // Abrir el formulario frmControlCuotas pasando el ID de la venta
                frmControlCuotas controlCuotas = new frmControlCuotas(ventaId);
                controlCuotas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila válida antes de hacer doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            frmVentas venta = new frmVentas();
            venta.FormClosed += (s, args) => ListarVentas(); // Refresca la lista al cerrar el formulario
            venta.ShowDialog();
        }

        private void btnGestionPagos_Click(object sender, EventArgs e)
        {
            frmControlPagos controlPagos = new frmControlPagos();
            controlPagos.ShowDialog();
        }

        private void FormatearFilas()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvVentasMenu.Rows)
                {
                    var venta = row.DataBoundItem as VentaDTO;
                    if (venta != null && venta.CuotasVencidas)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
        }

        private void cbOrdenarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void ImprimirHistorialVentasPorCliente(int clientId)
        {
            // Verificar si hay una fila seleccionada en el DataGridView
            if (dgvVentasMenu.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione una venta del cliente para generar su historial de reporte.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            List<sp_GetVentasByClientId_Result> listaVentasPorCliente = _gestorReportes.DatosVentasPorCliente(clientId);

            if (listaVentasPorCliente.Any())
            {
                // Cargar la plantilla HTML
                string textoHtml = Properties.Resources.VentasPorCliente.ToString();

                textoHtml = textoHtml.Replace("@nombre", listaVentasPorCliente.FirstOrDefault().NombreCliente);
                textoHtml = textoHtml.Replace("@apellido", listaVentasPorCliente.FirstOrDefault().ApellidoCliente);
                textoHtml = textoHtml.Replace("@dni", listaVentasPorCliente.FirstOrDefault().DNICliente.ToString());
                textoHtml = textoHtml.Replace("@direccion", listaVentasPorCliente.FirstOrDefault().DireccionCliente);
                textoHtml = textoHtml.Replace("@telefono", listaVentasPorCliente.FirstOrDefault().TelefonoCliente);

                // Generar las filas de la tabla agrupando por VentaId
                string filasTabla = string.Join("", listaVentasPorCliente
                    .GroupBy(v => v.VentaId) // Agrupar por VentaId
                    .SelectMany(grupo => grupo.Select((venta, index) => 
                    {
                        // Determinar el estado de la cuota y su clase CSS
                        string claseEstado = "";
                        if (venta.FechaQuePagoCuota.HasValue)
                        {
                            claseEstado = "pagada"; // Cuota pagada
                        }
                        else if (venta.FechaProgramadaDeCuota < DateTime.Now)
                        {
                            claseEstado = "vencida"; // Cuota vencida
                        }
                        else
                        {
                            claseEstado = "pendiente"; // Cuota pendiente
                        }

                        return $@"
                    <tr class='{claseEstado}'>
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Articulo}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{string.Format("{0:#,##0.00}", venta.Precio).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Cuotas}</td>" : "")}                   
                        <td>{venta.FechaDeVenta.ToString("dd/MM/yyyy")}</td>
                        <td>{venta.NumeroDeCuota}</td>
                        <td>{string.Format("{0:#,##0.00}", venta.MontoCuota).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>
                        <td>{venta.FechaProgramadaDeCuota.ToString("dd/MM/yyyy")}</td>
                        <td>{(venta.FechaQuePagoCuota.HasValue ? venta.FechaQuePagoCuota.Value.ToString("dd/MM/yyyy") : "")}</td>
                    </tr>";
                    })));

                // Reemplazar en la plantilla HTML
                textoHtml = textoHtml.Replace("@filasTabla", filasTabla);

                // Calcular estadísticas de cuotas
                var totalCuotas = listaVentasPorCliente.Count;
                var cuotasPagadas = listaVentasPorCliente.Count(c => c.FechaQuePagoCuota.HasValue);
                var cuotasVencidas = listaVentasPorCliente.Count(c => !c.FechaQuePagoCuota.HasValue && c.FechaProgramadaDeCuota < DateTime.Now);
                var cuotasPendientes = listaVentasPorCliente.Count(c => !c.FechaQuePagoCuota.HasValue && c.FechaProgramadaDeCuota >= DateTime.Now);

                // Crear resumen de estadísticas
                string resumenEstadisticas = $@"
                <div style='margin-top: 20px; padding: 10px; border: 1px solid #ccc; background-color: #f9f9f9;'>
                    <h3>Resumen de Cuotas:</h3>
                    <p><strong>Total de cuotas:</strong> {totalCuotas}</p>
                    <p><strong>Cuotas pagadas:</strong> {cuotasPagadas} ({(totalCuotas > 0 ? (cuotasPagadas * 100.0 / totalCuotas).ToString("F1") : "0")}%)</p>
                    <p><strong>Cuotas vencidas:</strong> {cuotasVencidas} ({(totalCuotas > 0 ? (cuotasVencidas * 100.0 / totalCuotas).ToString("F1") : "0")}%)</p>
                    <p><strong>Cuotas pendientes:</strong> {cuotasPendientes} ({(totalCuotas > 0 ? (cuotasPendientes * 100.0 / totalCuotas).ToString("F1") : "0")}%)</p>
                </div>";

                // Insertar el resumen antes de la leyenda
                textoHtml = textoHtml.Replace("<div class=\"divider\" style=\"margin-top: 20px;\"></div>", resumenEstadisticas + "\n        <div class=\"divider\" style=\"margin-top: 20px;\"></div>");
       
                textoHtml = textoHtml.Replace("@fecha-cancelacion-cuota", listaVentasPorCliente.FirstOrDefault()?.FechaCancelacionCuotas?.ToString("dd/MM/yyyy") ?? "");
                textoHtml = textoHtml.Replace("@fecha-programada-cuota", listaVentasPorCliente.FirstOrDefault()?.FechaProgramadaDeCuota.ToString("dd/MM/yyyy"));
                textoHtml = textoHtml.Replace("@fecha-venta", listaVentasPorCliente.FirstOrDefault()?.FechaDeVenta.ToString("dd/MM/yyyy"));
                textoHtml = textoHtml.Replace("@frecuencia-pago", listaVentasPorCliente.FirstOrDefault().FormaDePago.ToString());

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = $"ventas-por-cliente_{listaVentasPorCliente.FirstOrDefault().NombreCliente + listaVentasPorCliente.FirstOrDefault().ApellidoCliente}-{DateTime.Now.ToString("dd_MM_yyyy")}.pdf";
                saveFile.Filter = "Pdf Files|*.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // Crear el conversor de HTML a PDF
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

                    // Convertir el HTML a PDF
                    SelectPdf.PdfDocument doc = converter.ConvertHtmlString(textoHtml);

                    // Guardar el PDF
                    doc.Save(saveFile.FileName);
                    doc.Close();

                    MessageBox.Show("Documento Generado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnSeleccionarVenta_Click(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmArticulo articulo = new frmArticulo();
            articulo.FormClosed += (s, args) => ListarVentas(); // Refresca la lista al cerrar el formulario
            articulo.ShowDialog();
        }

        private void btnGestionVendedor_Click(object sender, EventArgs e)
        {
            frmVerVentasPorFecha verVentasPorFecha = new frmVerVentasPorFecha();
            verVentasPorFecha.ShowDialog();
        }

        private void btnAiresAcondicionados_Click(object sender, EventArgs e)
        {
            frmAireAcondicionado frmAireAcondicionado = new frmAireAcondicionado();
            frmAireAcondicionado.ShowDialog();
        }

        private void btnGestionCompras_Click(object sender, EventArgs e)
        {
            frmCompras frmCompras = new frmCompras();
            frmCompras.ShowDialog();
        }

        private void btnIngresosEgresos_Click(object sender, EventArgs e)
        {
            frmIngresosEgresos frmIngresosEgresos = new frmIngresosEgresos();
            frmIngresosEgresos.ShowDialog();
        }   
        private void marcasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmGestionMarca frmGestionMarca = new frmGestionMarca();
            frmGestionMarca.ShowDialog();
        }

        private void conectoresToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmGestionTipoConector frmGestionTipoConector = new frmGestionTipoConector();
            frmGestionTipoConector.ShowDialog();
        }

        private void medidasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmGestionMedida frmGestionMedida= new frmGestionMedida();
            frmGestionMedida.ShowDialog();
        }
    }
}
