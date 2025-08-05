using CEntidades;
using CLogica;
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
    public partial class frmGestionTipoConector : Form
    {
        private GestorTipoConector _gestorTipoConector = new GestorTipoConector();

        public frmGestionTipoConector()
        {
            InitializeComponent();
            this.Load += frmGestionTipoConector_Load;
        }

        private void frmGestionTipoConector_Load(object sender, EventArgs e)
        {
            CargarTipoConectores();
            EstiloDataGridView();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvTipoConector.DataBindingComplete += dgvTipoConector_DataBindingComplete;
        }

        private void CargarTipoConectores()
        {
            List<TipoConector> tipoConectores = _gestorTipoConector.ListarTipoConectores();
            dgvTipoConector.DataSource = tipoConectores;
        }

        private void EstiloDataGridView()
        {
            if (dgvTipoConector.Rows.Count > 0)
            {
                dgvTipoConector.Columns["IdTipoConector"].HeaderText = "ID";
                dgvTipoConector.Columns["NombreTipoConector"].HeaderText = "Nombre Conector";
                dgvTipoConector.Columns["IdTipoConector"].Width = 80;
                dgvTipoConector.Columns["NombreTipoConector"].Width = 250;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (var formInput = new frmInputTipoConector())
            {
                if (formInput.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _gestorTipoConector.RegistrarTipoConector(formInput.TipoConectorResultado);
                        MessageBox.Show("Tipo de conector registrado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarTipoConectores();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTipoConector.CurrentRow != null)
            {
                int idTipoConector = Convert.ToInt32(dgvTipoConector.CurrentRow.Cells["IdTipoConector"].Value);
                string nombreTipoConector = dgvTipoConector.CurrentRow.Cells["NombreTipoConector"].Value.ToString();

                using (var formInput = new frmInputTipoConector(idTipoConector, nombreTipoConector))
                {
                    if (formInput.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            _gestorTipoConector.ModificarTipoConector(formInput.TipoConectorResultado);
                            MessageBox.Show("Tipo de conector modificado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarTipoConectores();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al modificar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de conector para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTipoConector.CurrentRow != null)
            {
                int idTipoConector = Convert.ToInt32(dgvTipoConector.CurrentRow.Cells["IdTipoConector"].Value);
                string nombreTipoConector = dgvTipoConector.CurrentRow.Cells["NombreTipoConector"].Value.ToString();

                DialogResult resultado = MessageBox.Show($"¿Está seguro de eliminar el tipo de conector '{nombreTipoConector}'?", 
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        _gestorTipoConector.EliminarTipoConector(idTipoConector);
                        MessageBox.Show("Tipo de conector eliminado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarTipoConectores();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de conector para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvTipoConector_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvTipoConector.ClearSelection();
            dgvTipoConector.CurrentCell = null;
        }

        private void frmGestionTipoConector_Load_1(object sender, EventArgs e)
        {

        }
    }

    // Formulario modal para entrada de datos
    public partial class frmInputTipoConector : Form
    {
        public TipoConector TipoConectorResultado { get; private set; }

        private TextBox txtNombreTipoConector;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblNombreTipoConector;

        public frmInputTipoConector()
        {
            InitializeComponent();
            this.Text = "Nuevo Tipo de Conector";
        }

        public frmInputTipoConector(int idTipoConector, string nombreTipoConector) : this()
        {
            this.Text = "Editar Tipo de Conector";
            txtNombreTipoConector.Text = nombreTipoConector;
            TipoConectorResultado = new TipoConector { IdTipoConector = idTipoConector, NombreTipoConector = nombreTipoConector };
        }

        private void InitializeComponent()
        {
            this.txtNombreTipoConector = new TextBox();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();
            this.lblNombreTipoConector = new Label();
            this.SuspendLayout();

            // lblNombreTipoConector
            this.lblNombreTipoConector.AutoSize = true;
            this.lblNombreTipoConector.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.lblNombreTipoConector.Location = new Point(12, 15);
            this.lblNombreTipoConector.Name = "lblNombreTipoConector";
            this.lblNombreTipoConector.Size = new Size(200, 20);
            this.lblNombreTipoConector.Text = "Nombre de Tipo Conector:";

            // txtNombreTipoConector
            this.txtNombreTipoConector.Font = new Font("Microsoft Sans Serif", 12F);
            this.txtNombreTipoConector.Location = new Point(12, 45);
            this.txtNombreTipoConector.Name = "txtNombreTipoConector";
            this.txtNombreTipoConector.Size = new Size(350, 26);

            // btnAceptar
            this.btnAceptar.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.btnAceptar.Location = new Point(100, 90);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new Size(100, 35);
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += btnAceptar_Click;

            // btnCancelar
            this.btnCancelar.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.btnCancelar.Location = new Point(220, 90);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(100, 35);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += btnCancelar_Click;

            // frmInputTipoConector
            this.ClientSize = new Size(380, 140);
            this.Controls.Add(this.lblNombreTipoConector);
            this.Controls.Add(this.txtNombreTipoConector);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreTipoConector.Text))
            {
                MessageBox.Show("El nombre del tipo de conector es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreTipoConector.Focus();
                return;
            }

            if (TipoConectorResultado == null)
                TipoConectorResultado = new TipoConector();

            TipoConectorResultado.NombreTipoConector = txtNombreTipoConector.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
