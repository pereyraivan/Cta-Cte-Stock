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
    public partial class frmReportReciboDePago : Form
    {
        private int _ventaId;
        private int _numeroCuota;
        public frmReportReciboDePago(int ventaId, int numCuota)
        {
            InitializeComponent();
            this._ventaId = ventaId;
            this._numeroCuota = numCuota;
        }

        private void frmReportReciboDePago_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar en modo remoto (Server Report)
                rvReciboDePago.ProcessingMode = ProcessingMode.Remote;
                rvReciboDePago.ServerReport.ReportServerUrl = new Uri("http://localhost/reportserver");
                rvReciboDePago.ServerReport.ReportPath = "/Reportes/Recibo";

                // Pasar parámetros al reporte
                ReportParameter[] parametros = new ReportParameter[]
                {
                new ReportParameter("VentaId", _ventaId.ToString()),
                new ReportParameter("NumeroDeCuota", _numeroCuota.ToString())
                };

                rvReciboDePago.ServerReport.SetParameters(parametros);

                // Renderizar el reporte en formato PDF
                string mimeType, encoding, fileNameExtension;
                string[] streams;
                Warning[] warnings;

                byte[] pdfBytes = rvReciboDePago.ServerReport.Render(
                    "PDF", // Formato de exportación
                    null,  // Configuración del dispositivo
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                // Guardar el PDF en un archivo temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(), "ReporteRecibo.pdf");
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
            this.rvReciboDePago.RefreshReport();
        }
    }
}
