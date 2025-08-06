using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CLogica;
using CEntidades;

namespace VentaCredimax.Formularios
{
    public partial class frmAireAcondicionado : Form
    {
        private GestorAireAcondicionado gestor;
        private TrabajoAireAcondicionado trabajoSeleccionado;
        private bool modoEdicion = false;

        public frmAireAcondicionado()
        {
            InitializeComponent();
        }

        private void frmAireAcondicionado_Load(object sender, EventArgs e)
        {
            gestor = new GestorAireAcondicionado();
            CargarGrilla();
            LimpiarCampos();
        }

        private void CargarGrilla()
        {
            dgvTrabajos.DataSource = gestor.ListarTrabajos();
            ConfigurarGrilla();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var trabajo = new TrabajoAireAcondicionado
                {
                    IdTrabajo = modoEdicion ? trabajoSeleccionado.IdTrabajo : 0,
                    DescripcionTrabajo = txtDescripcion.Text.Trim(),
                    Precio = string.IsNullOrEmpty(txtPrecio.Text) ? (decimal?)null : Convert.ToDecimal(txtPrecio.Text)
                };

                bool resultado = modoEdicion ? gestor.ModificarTrabajo(trabajo) : gestor.GuardarTrabajo(trabajo);
                
                if (resultado)
                {
                    MessageBox.Show("Trabajo " + (modoEdicion ? "modificado" : "guardado") + " exitosamente");
                    CargarGrilla();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(gestor.GetUltimoError(), "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTrabajos.SelectedRows.Count > 0)
            {
                trabajoSeleccionado = (TrabajoAireAcondicionado)dgvTrabajos.SelectedRows[0].DataBoundItem;
                txtDescripcion.Text = trabajoSeleccionado.DescripcionTrabajo;
                txtCliente.Text = trabajoSeleccionado.Cliente ?? "";
                txtPrecio.Text = trabajoSeleccionado.Precio?.ToString() ?? "";
            }
            else
            {
                MessageBox.Show("Debe seleccionar un trabajo para editar");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTrabajos.SelectedRows.Count > 0)
            {
                var trabajo = (TrabajoAireAcondicionado)dgvTrabajos.SelectedRows[0].DataBoundItem;
                
                if (MessageBox.Show("¿Está seguro de eliminar el trabajo '" + trabajo.DescripcionTrabajo + "'?", 
                                   "Confirmar eliminación", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (gestor.EliminarTrabajo(trabajo.IdTrabajo))
                    {
                        MessageBox.Show("Trabajo eliminado exitosamente");
                        CargarGrilla();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show(gestor.GetUltimoError(), "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un trabajo para eliminar");
            }
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtCliente.Clear();
            btnGuardar.Text = "Guardar";
            trabajoSeleccionado = null;
        }

        private void ConfigurarGrilla()
        {
            if (dgvTrabajos.Columns.Count > 0)
            {
                dgvTrabajos.Columns["IdTrabajo"].HeaderText = "ID";
                dgvTrabajos.Columns["DescripcionTrabajo"].HeaderText = "Descripción";
                dgvTrabajos.Columns["Cliente"].HeaderText = "Cliente";
                dgvTrabajos.Columns["Precio"].HeaderText = "Precio";
                
                dgvTrabajos.Columns["IdTrabajo"].Width = 30;
                dgvTrabajos.Columns["DescripcionTrabajo"].Width = 300;
                dgvTrabajos.Columns["Cliente"].Width = 100; 
                dgvTrabajos.Columns["Precio"].Width = 100;
            }
        }

        private void dgvTrabajos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                trabajoSeleccionado = (TrabajoAireAcondicionado)dgvTrabajos.Rows[e.RowIndex].DataBoundItem;
                txtDescripcion.Text = trabajoSeleccionado.DescripcionTrabajo;
                txtCliente.Text = trabajoSeleccionado.Cliente ?? ""; 
                txtPrecio.Text = trabajoSeleccionado.Precio?.ToString() ?? "";
                
                modoEdicion = true;
              
                
                txtDescripcion.Focus();
            }
        }
    }
}
