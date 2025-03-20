namespace VentaCredimax.Formularios
{
    partial class frmControlPagos
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.cbBuscarPor = new System.Windows.Forms.ComboBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSeleccionarVenta = new System.Windows.Forms.Button();
            this.cbOrdenarPor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTodasVentas = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnImpDetalleDeVenta = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvVentas
            // 
            this.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVentas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVentas.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvVentas.Location = new System.Drawing.Point(12, 64);
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.ReadOnly = true;
            this.dgvVentas.RowHeadersWidth = 51;
            this.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentas.Size = new System.Drawing.Size(1148, 499);
            this.dgvVentas.TabIndex = 30;
            this.dgvVentas.DoubleClick += new System.EventHandler(this.dgvVentas_DoubleClick);
            // 
            // cbBuscarPor
            // 
            this.cbBuscarPor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBuscarPor.FormattingEnabled = true;
            this.cbBuscarPor.ItemHeight = 20;
            this.cbBuscarPor.Items.AddRange(new object[] {
            "Cliente",
            "Articulo"});
            this.cbBuscarPor.Location = new System.Drawing.Point(144, 21);
            this.cbBuscarPor.Name = "cbBuscarPor";
            this.cbBuscarPor.Size = new System.Drawing.Size(135, 28);
            this.cbBuscarPor.TabIndex = 33;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscar.Location = new System.Drawing.Point(291, 20);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(262, 29);
            this.txtBuscar.TabIndex = 34;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 24);
            this.label5.TabIndex = 35;
            this.label5.Text = "Buscar por:";
            // 
            // btnSeleccionarVenta
            // 
            this.btnSeleccionarVenta.BackColor = System.Drawing.Color.Azure;
            this.btnSeleccionarVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarVenta.Location = new System.Drawing.Point(960, 12);
            this.btnSeleccionarVenta.Name = "btnSeleccionarVenta";
            this.btnSeleccionarVenta.Size = new System.Drawing.Size(200, 46);
            this.btnSeleccionarVenta.TabIndex = 36;
            this.btnSeleccionarVenta.Text = "&Seleccionar venta";
            this.toolTip1.SetToolTip(this.btnSeleccionarVenta, "presione seleccionar o hacer doble clic sobre la venta");
            this.btnSeleccionarVenta.UseVisualStyleBackColor = false;
            this.btnSeleccionarVenta.Click += new System.EventHandler(this.btnSeleccionarVenta_Click);
            // 
            // cbOrdenarPor
            // 
            this.cbOrdenarPor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOrdenarPor.FormattingEnabled = true;
            this.cbOrdenarPor.ItemHeight = 20;
            this.cbOrdenarPor.Items.AddRange(new object[] {
            "Fecha ",
            "Cuotas Vencidas "});
            this.cbOrdenarPor.Location = new System.Drawing.Point(726, 21);
            this.cbOrdenarPor.Name = "cbOrdenarPor";
            this.cbOrdenarPor.Size = new System.Drawing.Size(199, 28);
            this.cbOrdenarPor.TabIndex = 43;
            this.cbOrdenarPor.SelectedIndexChanged += new System.EventHandler(this.cbOrdenarPor_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(591, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 42;
            this.label1.Text = "Ordenar por:";
            // 
            // cbTodasVentas
            // 
            this.cbTodasVentas.AutoSize = true;
            this.cbTodasVentas.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTodasVentas.Location = new System.Drawing.Point(730, 588);
            this.cbTodasVentas.Name = "cbTodasVentas";
            this.cbTodasVentas.Size = new System.Drawing.Size(429, 32);
            this.cbTodasVentas.TabIndex = 44;
            this.cbTodasVentas.Text = "Mostrar ventas canceladas y anuladas";
            this.toolTip1.SetToolTip(this.cbTodasVentas, "Muestra las ventas canceladas o anuladas");
            this.cbTodasVentas.UseVisualStyleBackColor = true;
            this.cbTodasVentas.CheckedChanged += new System.EventHandler(this.cbTodasVentas_CheckedChanged);
            // 
            // btnImpDetalleDeVenta
            // 
            this.btnImpDetalleDeVenta.BackColor = System.Drawing.Color.Azure;
            this.btnImpDetalleDeVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImpDetalleDeVenta.Location = new System.Drawing.Point(12, 571);
            this.btnImpDetalleDeVenta.Name = "btnImpDetalleDeVenta";
            this.btnImpDetalleDeVenta.Size = new System.Drawing.Size(295, 62);
            this.btnImpDetalleDeVenta.TabIndex = 45;
            this.btnImpDetalleDeVenta.Text = "&Imprimir detalle de venta";
            this.toolTip1.SetToolTip(this.btnImpDetalleDeVenta, "Imprime el detalle de venta seleccionado");
            this.btnImpDetalleDeVenta.UseVisualStyleBackColor = false;
            this.btnImpDetalleDeVenta.Click += new System.EventHandler(this.btnImpDetalleDeVenta_Click);
            // 
            // frmControlPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1173, 640);
            this.Controls.Add(this.btnImpDetalleDeVenta);
            this.Controls.Add(this.cbTodasVentas);
            this.Controls.Add(this.cbOrdenarPor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSeleccionarVenta);
            this.Controls.Add(this.cbBuscarPor);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvVentas);
            this.MaximumSize = new System.Drawing.Size(1189, 679);
            this.MinimumSize = new System.Drawing.Size(1189, 679);
            this.Name = "frmControlPagos";
            this.Text = "Ventas activas";
            this.Load += new System.EventHandler(this.frmControlPagos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.ComboBox cbBuscarPor;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSeleccionarVenta;
        private System.Windows.Forms.ComboBox cbOrdenarPor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbTodasVentas;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnImpDetalleDeVenta;
    }
}