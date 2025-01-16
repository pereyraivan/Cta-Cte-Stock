namespace VentaCredimax.Formularios
{
    partial class frmReporteVentasPorCliente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rvHistorialVentasPorCliente = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rvHistorialVentasPorCliente
            // 
            this.rvHistorialVentasPorCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvHistorialVentasPorCliente.Location = new System.Drawing.Point(0, 0);
            this.rvHistorialVentasPorCliente.Name = "rvHistorialVentasPorCliente";
            this.rvHistorialVentasPorCliente.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            this.rvHistorialVentasPorCliente.ServerReport.BearerToken = null;
            this.rvHistorialVentasPorCliente.ServerReport.ReportPath = "Reportes/VentasPorCliente";
            this.rvHistorialVentasPorCliente.Size = new System.Drawing.Size(1083, 655);
            this.rvHistorialVentasPorCliente.TabIndex = 0;
            // 
            // frmReporteVentasPorCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 655);
            this.Controls.Add(this.rvHistorialVentasPorCliente);
            this.Name = "frmReporteVentasPorCliente";
            this.Text = "frmReporteVentasPorCliente";
            this.Load += new System.EventHandler(this.frmReporteVentasPorCliente_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvHistorialVentasPorCliente;
    }
}