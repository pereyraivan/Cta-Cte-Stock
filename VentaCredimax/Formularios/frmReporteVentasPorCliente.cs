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
    public partial class frmReporteVentasPorCliente : Form
    {
        private int _ClientId;
        public frmReporteVentasPorCliente(int clientId)
        {
            InitializeComponent();
            _ClientId = clientId;
            imprimirHistorialVentaPorClientePdf();
        }

        private void frmReporteVentasPorCliente_Load(object sender, EventArgs e)
        {
            this.rvHistorialVentasPorCliente.RefreshReport();
        }
        private void imprimirHistorialVentaPorClientePdf()
        {
            try
            {
                // Configurar en modo remoto (Server Report)
                rvHistorialVentasPorCliente.ProcessingMode = ProcessingMode.Remote;
                rvHistorialVentasPorCliente.ServerReport.ReportServerUrl = new Uri("http://localhost/reportserver");
                rvHistorialVentasPorCliente.ServerReport.ReportPath = "/Reportes/VentasPorCliente";

                // Pasar parámetros al reporte
                ReportParameter[] parametros = new ReportParameter[]
                {
                     new ReportParameter("ClientId", _ClientId.ToString()),
                };

                rvHistorialVentasPorCliente.ServerReport.SetParameters(parametros);

                // Renderizar el reporte en formato PDF
                string mimeType, encoding, fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] pdfBytes = rvHistorialVentasPorCliente.ServerReport.Render(
                    "PDF", // Formato de exportación
                    null,  // Configuración del dispositivo
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                // Guardar el PDF en un archivo temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(), "VentasPorCliente.pdf");
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
            this.rvHistorialVentasPorCliente.RefreshReport();
        }
    }
}
