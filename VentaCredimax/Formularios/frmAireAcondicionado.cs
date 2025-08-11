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
            
            // Habilitar eventos de teclas para el formulario
            this.KeyPreview = true;
            this.KeyDown += frmAireAcondicionado_KeyDown;
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
                    IdTrabajo = 0,
                    DescripcionTrabajo = txtDescripcion.Text.Trim(),
                    Cliente = txtCliente.Text,
                    Precio = string.IsNullOrEmpty(txtPrecio.Text) ? (decimal?)null : Convert.ToDecimal(txtPrecio.Text),
                    FechaTrabajo = DateTime.Now.Date
                };

                bool resultado = gestor.GuardarTrabajo(trabajo);
                
                if (resultado)
                {
                    MessageBox.Show("Trabajo guardado exitosamente");
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
            if (!modoEdicion)
            {
                // Entrar en modo edición
                if (dgvTrabajos.SelectedRows.Count > 0)
                {
                    trabajoSeleccionado = (TrabajoAireAcondicionado)dgvTrabajos.SelectedRows[0].DataBoundItem;
                    txtDescripcion.Text = trabajoSeleccionado.DescripcionTrabajo;
                    txtCliente.Text = trabajoSeleccionado.Cliente ?? "";
                    txtPrecio.Text = trabajoSeleccionado.Precio?.ToString() ?? "";
                    
                    modoEdicion = true;
                    btnEditar.Text = "Confirmar Edición";
                    btnGuardar.Enabled = false; // Deshabilitar guardar en modo edición
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un trabajo para editar");
                }
            }
            else
            {
                // Confirmar edición
                try
                {
                    var trabajo = new TrabajoAireAcondicionado
                    {
                        IdTrabajo = trabajoSeleccionado.IdTrabajo,
                        DescripcionTrabajo = txtDescripcion.Text.Trim(),
                        Cliente = txtCliente.Text.Trim(),
                        Precio = string.IsNullOrEmpty(txtPrecio.Text) ? (decimal?)null : Convert.ToDecimal(txtPrecio.Text),
                        FechaTrabajo = trabajoSeleccionado.FechaTrabajo // Mantener la fecha original en edición
                    };

                    bool resultado = gestor.ModificarTrabajo(trabajo);
                    
                    if (resultado)
                    {
                        MessageBox.Show("Trabajo modificado exitosamente");
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
            btnEditar.Text = "Editar";
            btnGuardar.Enabled = true;
            modoEdicion = false;
            trabajoSeleccionado = null;
        }

        private void CancelarEdicion()
        {
            LimpiarCampos();
            MessageBox.Show("Edición cancelada");
        }

        private void frmAireAcondicionado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && modoEdicion)
            {
                CancelarEdicion();
            }
        }

        private void ConfigurarGrilla()
        {
            if (dgvTrabajos.Columns.Count > 0)
            {
                dgvTrabajos.Columns["IdTrabajo"].HeaderText = "ID";
                dgvTrabajos.Columns["DescripcionTrabajo"].HeaderText = "Descripción";
                dgvTrabajos.Columns["Cliente"].HeaderText = "Cliente";
                dgvTrabajos.Columns["Precio"].HeaderText = "Precio";
                
                // Verificar si existe la columna FechaTrabajo
                if (dgvTrabajos.Columns["FechaTrabajo"] != null)
                {
                    dgvTrabajos.Columns["FechaTrabajo"].HeaderText = "Fecha";
                    dgvTrabajos.Columns["FechaTrabajo"].Width = 100;
                    dgvTrabajos.Columns["FechaTrabajo"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                
                dgvTrabajos.Columns["IdTrabajo"].Width = 30;
                dgvTrabajos.Columns["DescripcionTrabajo"].Width = 250;
                dgvTrabajos.Columns["Cliente"].Width = 100; 
                dgvTrabajos.Columns["Precio"].Width = 100;
                
                // Formatear la columna de precio
                dgvTrabajos.Columns["Precio"].DefaultCellStyle.Format = "C";
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
                btnEditar.Text = "Confirmar Edición";
                btnGuardar.Enabled = false;
                
                txtDescripcion.Focus();
            }
        }
    }
}
