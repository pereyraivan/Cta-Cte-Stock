namespace VentaCredimax.Formularios
{
    partial class frmReportReciboDePago
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
            this.rvReciboDePago = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rvReciboDePago
            // 
            this.rvReciboDePago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvReciboDePago.Location = new System.Drawing.Point(0, 0);
            this.rvReciboDePago.Name = "rvReciboDePago";
            this.rvReciboDePago.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            this.rvReciboDePago.ServerReport.BearerToken = null;
            this.rvReciboDePago.ServerReport.ReportPath = "Reportes/Recibo";
            this.rvReciboDePago.Size = new System.Drawing.Size(823, 546);
            this.rvReciboDePago.TabIndex = 0;
            // 
            // frmReportReciboDePago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 546);
            this.Controls.Add(this.rvReciboDePago);
            this.Name = "frmReportReciboDePago";
            this.Text = "frmReportReciboDePago";
            this.Load += new System.EventHandler(this.frmReportReciboDePago_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvReciboDePago;
    }
}