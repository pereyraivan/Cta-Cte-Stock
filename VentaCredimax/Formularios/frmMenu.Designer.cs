namespace VentaCredimax.Formularios
{
    partial class frmMenu
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
            this.pTituloSistema = new System.Windows.Forms.Panel();
            this.pMenuLateral = new System.Windows.Forms.Panel();
            this.btnNuevaVenta = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pMenuLateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pTituloSistema
            // 
            this.pTituloSistema.BackColor = System.Drawing.SystemColors.Highlight;
            this.pTituloSistema.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTituloSistema.Location = new System.Drawing.Point(0, 0);
            this.pTituloSistema.Name = "pTituloSistema";
            this.pTituloSistema.Size = new System.Drawing.Size(1669, 58);
            this.pTituloSistema.TabIndex = 1;
            // 
            // pMenuLateral
            // 
            this.pMenuLateral.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pMenuLateral.Controls.Add(this.panel1);
            this.pMenuLateral.Controls.Add(this.pictureBox1);
            this.pMenuLateral.Controls.Add(this.btnNuevaVenta);
            this.pMenuLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.pMenuLateral.Location = new System.Drawing.Point(0, 58);
            this.pMenuLateral.Name = "pMenuLateral";
            this.pMenuLateral.Size = new System.Drawing.Size(268, 653);
            this.pMenuLateral.TabIndex = 2;
            // 
            // btnNuevaVenta
            // 
            this.btnNuevaVenta.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
            this.btnNuevaVenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaVenta.Font = new System.Drawing.Font("OCR A Extended", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevaVenta.Location = new System.Drawing.Point(12, 159);
            this.btnNuevaVenta.Name = "btnNuevaVenta";
            this.btnNuevaVenta.Size = new System.Drawing.Size(256, 93);
            this.btnNuevaVenta.TabIndex = 0;
            this.btnNuevaVenta.Text = "Nueva Venta";
            this.btnNuevaVenta.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 143);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Location = new System.Drawing.Point(0, 159);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(16, 93);
            this.panel1.TabIndex = 2;
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1669, 711);
            this.Controls.Add(this.pMenuLateral);
            this.Controls.Add(this.pTituloSistema);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMenu";
            this.Text = "frmMenu";
            this.pMenuLateral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTituloSistema;
        private System.Windows.Forms.Panel pMenuLateral;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnNuevaVenta;
        private System.Windows.Forms.Panel panel1;
    }
}