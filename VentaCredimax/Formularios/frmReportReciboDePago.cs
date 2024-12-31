using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                // Configurar parámetros para el reporte
                rvReciboDePago.ProcessingMode = ProcessingMode.Remote;
                rvReciboDePago.ServerReport.ReportServerUrl = new Uri("http://localhost/ReportServer");
                rvReciboDePago.ServerReport.ReportPath = "Report Parts";

                // Pasar parámetros al reporte
                ReportParameter[] parametros = new ReportParameter[]
                {
                new ReportParameter("VentaId", _ventaId.ToString()),
                new ReportParameter("NumeroCuota", _numeroCuota.ToString())
                };

                rvReciboDePago.ServerReport.SetParameters(parametros);
                rvReciboDePago.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.rvReciboDePago.RefreshReport();
        }
    }
}
