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
    public partial class frmComprobanteDeVenta : Form
    {
        private int _ventaId;
        public frmComprobanteDeVenta(int ventaId)
        {
            InitializeComponent();
            _ventaId = ventaId;
            imprimirComprobanteDeVentaPdf();
        }

        private void frmComprobanteDeVenta_Load(object sender, EventArgs e)
        {
            this.rvReporteComprobanteVenta.RefreshReport();
        }
        private void imprimirComprobanteDeVentaPdf()
        {
            try
            {
                // Configurar en modo remoto (Server Report)
                rvReporteComprobanteVenta.ProcessingMode = ProcessingMode.Remote;
                rvReporteComprobanteVenta.ServerReport.ReportServerUrl = new Uri("http://localhost/reportserver");
                rvReporteComprobanteVenta.ServerReport.ReportPath = "/Reportes/ComprobanteDeVenta";

                // Pasar parámetros al reporte
                ReportParameter[] parametros = new ReportParameter[]
                {
                     new ReportParameter("VentaId", _ventaId.ToString()),
                };

                rvReporteComprobanteVenta.ServerReport.SetParameters(parametros);

                // Renderizar el reporte en formato PDF
                string mimeType, encoding, fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] pdfBytes = rvReporteComprobanteVenta.ServerReport.Render(
                    "PDF", // Formato de exportación
                    null,  // Configuración del dispositivo
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                // Guardar el PDF en un archivo temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(), "ComprobanteDeVenta.pdf");
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
            this.rvReporteComprobanteVenta.RefreshReport();
        }
    }
}
