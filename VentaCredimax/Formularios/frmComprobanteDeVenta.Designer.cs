namespace VentaCredimax.Formularios
{
    partial class frmComprobanteDeVenta
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
            this.rvReporteComprobanteVenta = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rvReporteComprobanteVenta
            // 
            this.rvReporteComprobanteVenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvReporteComprobanteVenta.Location = new System.Drawing.Point(0, 0);
            this.rvReporteComprobanteVenta.Name = "rvReporteComprobanteVenta";
            this.rvReporteComprobanteVenta.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            this.rvReporteComprobanteVenta.ServerReport.BearerToken = null;
            this.rvReporteComprobanteVenta.ServerReport.ReportPath = "Reportes/ComprobanteDeVenta";
            this.rvReporteComprobanteVenta.Size = new System.Drawing.Size(1028, 645);
            this.rvReporteComprobanteVenta.TabIndex = 0;
            // 
            // frmComprobanteDeVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 645);
            this.Controls.Add(this.rvReporteComprobanteVenta);
            this.Name = "frmComprobanteDeVenta";
            this.Text = "frmComprobanteDeVenta";
            this.Load += new System.EventHandler(this.frmComprobanteDeVenta_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvReporteComprobanteVenta;
    }
}