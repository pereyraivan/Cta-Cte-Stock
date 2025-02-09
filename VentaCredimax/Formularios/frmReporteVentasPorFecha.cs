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
    public partial class frmReporteVentasPorFecha : Form
    {
        public frmReporteVentasPorFecha()
        {
            InitializeComponent();
        }

        private void frmReporteVentasPorFecha_Load(object sender, EventArgs e)
        {
            this.rvInformeVentas.RefreshReport();
        }

        private void btnImprimirComprobante_Click(object sender, EventArgs e)
        {
            if (dtpDesde.Value == null || dtpHasta.Value == null)
            {
                MessageBox.Show("Debe ingresar las fechas de inicio y fin para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DateTime fechaDesde = dtpDesde.Value;
                DateTime fechaHasta = dtpHasta.Value;
                if(fechaDesde > fechaHasta)
                {
                    MessageBox.Show("Debe ingresar las fechas de inicio y fin para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    imprimirReporteVentasPdf(fechaDesde, fechaHasta);
                }
                
            }
        }
        private void imprimirReporteVentasPdf(DateTime fechaDesde , DateTime fechaHasta)
        {
            try
            {
                // Configurar en modo remoto (Server Report)
                rvInformeVentas.ProcessingMode = ProcessingMode.Remote;
                rvInformeVentas.ServerReport.ReportServerUrl = new Uri("http://localhost/reportserver");
                rvInformeVentas.ServerReport.ReportPath = "/Reportes/VentasPorFecha";

                // Pasar parámetros al reporte
                ReportParameter[] parametros = new ReportParameter[]
                {
                new ReportParameter("FechaDesde", fechaDesde.ToString()),
                new ReportParameter("FechaHasta", fechaHasta.ToString())
                };

                rvInformeVentas.ServerReport.SetParameters(parametros);

                // Renderizar el reporte en formato PDF
                string mimeType, encoding, fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] pdfBytes = rvInformeVentas.ServerReport.Render(
                    "PDF", // Formato de exportación
                    null,  // Configuración del dispositivo
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                // Guardar el PDF en un archivo temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(), "ReporteInformeVentas.pdf");
                File.WriteAllBytes(tempFilePath, pdfBytes);

                // Abrir el PDF automáticamente
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.rvInformeVentas.RefreshReport();
        }
    }
}
