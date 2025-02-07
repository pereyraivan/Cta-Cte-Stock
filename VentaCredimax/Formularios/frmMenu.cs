using CEntidades;
using CEntidades.DTOs;
using CLogica;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmMenu : Form
    {
        private GestorVenta _gestorVenta = new GestorVenta();
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
            int idCliente = Convert.ToInt32(dgvVentasMenu.CurrentRow.Cells["IdCliente"].Value);
            pSubMenuInformes.Visible = false;
            ImprimirHistorialVentasPorCliente(idCliente);
        }

        private void btnInformeVentas_Click(object sender, EventArgs e)
        {
            pSubMenuInformes.Visible = false;
            frmReporteVentasPorFecha frmReporteVentasPorFecha = new frmReporteVentasPorFecha();
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
                MessageBox.Show("Seleccione una venta del cliente del cual desea imprimir el historial para generar el reporte.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Crear formulario de reporte
                frmReporteVentasPorCliente frmReporteVentasPorCliente = new frmReporteVentasPorCliente(clientId);
                //frmReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void GenerarReporteHirtorialClientePDF(int idCliente)
        {
            try
            {
                // Crear instancia de LocalReport
                LocalReport report = new LocalReport();

                // Establecer la ruta del archivo RDLC
                string reportPath = @"C:\Users\perey\OneDrive\Escritorio\VentasCred\Reportes\VentasPorCliente.rdl";

                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("No se encontró el archivo del reporte en la ruta especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                report.ReportPath = reportPath;

                // Configurar el parámetro IdCliente
                ReportParameter parametroIdCliente = new ReportParameter("ClientId", idCliente.ToString());
                report.SetParameters(parametroIdCliente);

                // Renderizar el reporte a PDF
                string mimeType, encoding, fileNameExtension;
                Warning[] warnings;
                string[] streamIds;
                byte[] pdfBytes = report.Render(
                    "PDF", null, out mimeType, out encoding, out fileNameExtension, out streamIds, out warnings);

                // Guardar el PDF en una ubicación temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(), "VentasPorCliente.pdf");
                File.WriteAllBytes(tempFilePath, pdfBytes);

                // Abrir el PDF automáticamente
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });

                MessageBox.Show("El reporte se generó correctamente y se abrió como PDF.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionarVenta_Click(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void cbBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
