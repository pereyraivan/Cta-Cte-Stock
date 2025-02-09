namespace VentaCredimax.Formularios
{
    partial class frmReporteVentasPorFecha
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
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImprimirComprobante = new System.Windows.Forms.Button();
            this.rvInformeVentas = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // dtpDesde
            // 
            this.dtpDesde.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(227, 42);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(183, 29);
            this.dtpDesde.TabIndex = 0;
            // 
            // dtpHasta
            // 
            this.dtpHasta.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(643, 43);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(200, 29);
            this.dtpHasta.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(43, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 29);
            this.label5.TabIndex = 36;
            this.label5.Text = "Fecha desde:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(472, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 29);
            this.label1.TabIndex = 37;
            this.label1.Text = "Fecha hasta:";
            // 
            // btnImprimirComprobante
            // 
            this.btnImprimirComprobante.BackColor = System.Drawing.Color.Azure;
            this.btnImprimirComprobante.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirComprobante.Location = new System.Drawing.Point(1054, 25);
            this.btnImprimirComprobante.Name = "btnImprimirComprobante";
            this.btnImprimirComprobante.Size = new System.Drawing.Size(252, 70);
            this.btnImprimirComprobante.TabIndex = 38;
            this.btnImprimirComprobante.Text = "&Imprimir ventas";
            this.btnImprimirComprobante.UseVisualStyleBackColor = false;
            this.btnImprimirComprobante.Click += new System.EventHandler(this.btnImprimirComprobante_Click);
            // 
            // rvInformeVentas
            // 
            this.rvInformeVentas.Location = new System.Drawing.Point(1, 109);
            this.rvInformeVentas.Name = "rvInformeVentas";
            this.rvInformeVentas.ServerReport.BearerToken = null;
            this.rvInformeVentas.Size = new System.Drawing.Size(1318, 584);
            this.rvInformeVentas.TabIndex = 39;
            // 
            // frmReporteVentasPorFecha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 692);
            this.Controls.Add(this.rvInformeVentas);
            this.Controls.Add(this.btnImprimirComprobante);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.dtpDesde);
            this.Name = "frmReporteVentasPorFecha";
            this.Text = "frmReporteVentasPorFecha";
            this.Load += new System.EventHandler(this.frmReporteVentasPorFecha_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImprimirComprobante;
        private Microsoft.Reporting.WinForms.ReportViewer rvInformeVentas;
    }
}