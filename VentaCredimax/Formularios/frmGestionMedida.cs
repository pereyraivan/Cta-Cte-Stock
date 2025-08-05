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
    public partial class frmGestionMedida : Form
    {
        private GestorMedida _gestorMedida = new GestorMedida();
        private int? medidaIdSeleccionada = null;

        public frmGestionMedida()
        {
            InitializeComponent();
            ConfigurarEstadoInicial();
        }

        private void frmGestionMedida_Load(object sender, EventArgs e)
        {
            CargarMedidas();
            EstiloDataGridView();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvMedidas.DataBindingComplete += dgvMedidas_DataBindingComplete;
            dgvMedidas.CellDoubleClick += dgvMedidas_CellDoubleClick;
            dgvMedidas.SelectionChanged += dgvMedidas_SelectionChanged;
            
            // Configurar tecla ESC para cancelar edición
            this.KeyPreview = true;
            this.KeyDown += frmGestionMedida_KeyDown;
        }

        private void frmGestionMedida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && medidaIdSeleccionada.HasValue)
            {
                EstablecerModoNuevo();
            }
        }

        private void ConfigurarEstadoInicial()
        {
            // Estado inicial - modo nuevo
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            txtNombreMedida.Clear();
            txtNombreMedida.Focus();
        }

        private void EstablecerModoNuevo()
        {
            medidaIdSeleccionada = null;
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            txtNombreMedida.Clear();
            txtNombreMedida.Focus();
            dgvMedidas.ClearSelection();
        }

        private void EstablecerModoEdicion()
        {
            btnGuardar.Enabled = false;
            btnEditar.Enabled = true;
            btnEliminar.Enabled = false;
        }

        private void CargarMedidas()
        {
            try
            {
                List<Medida> medidas = _gestorMedida.ListarMedidas();
                dgvMedidas.DataSource = medidas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las medidas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstiloDataGridView()
        {
            if (dgvMedidas.Rows.Count > 0)
            {
                dgvMedidas.Columns["IdMedida"].HeaderText = "ID";
                dgvMedidas.Columns["NombreMedida"].HeaderText = "Nombre de Medida";
                dgvMedidas.Columns["IdMedida"].Width = 80;
                dgvMedidas.Columns["NombreMedida"].Width = 200;
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreMedida.Text))
            {
                MessageBox.Show("El nombre de la medida es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreMedida.Focus();
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                var medida = new Medida
                {
                    NombreMedida = txtNombreMedida.Text.Trim()
                };

                _gestorMedida.RegistrarMedida(medida);
                MessageBox.Show("Medida registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarMedidas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la medida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!medidaIdSeleccionada.HasValue)
                {
                    MessageBox.Show("Debe seleccionar una medida para editar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos()) return;

                var medida = new Medida
                {
                    IdMedida = medidaIdSeleccionada.Value,
                    NombreMedida = txtNombreMedida.Text.Trim()
                };

                _gestorMedida.ModificarMedida(medida);
                MessageBox.Show("Medida modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarMedidas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar la medida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int? medidaIdAEliminar = null;
                
                // Si estamos en modo edición, usar la medida seleccionada
                if (medidaIdSeleccionada.HasValue)
                {
                    medidaIdAEliminar = medidaIdSeleccionada.Value;
                }
                // Si no estamos en modo edición, verificar si hay una fila seleccionada
                else if (dgvMedidas.SelectedRows.Count > 0)
                {
                    var medida = (Medida)dgvMedidas.SelectedRows[0].DataBoundItem;
                    medidaIdAEliminar = medida.IdMedida;
                }
                
                if (!medidaIdAEliminar.HasValue)
                {
                    MessageBox.Show("Debe seleccionar una medida para eliminar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var medidaAEliminar = dgvMedidas.SelectedRows.Count > 0 ? 
                    (Medida)dgvMedidas.SelectedRows[0].DataBoundItem : 
                    ((List<Medida>)dgvMedidas.DataSource).FirstOrDefault(m => m.IdMedida == medidaIdAEliminar.Value);

                string nombreMedida = medidaAEliminar?.NombreMedida ?? "esta medida";

                if (MessageBox.Show($"¿Está seguro de eliminar la medida '{nombreMedida}'?", 
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _gestorMedida.EliminarMedida(medidaIdAEliminar.Value);
                    MessageBox.Show("Medida eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    EstablecerModoNuevo();
                    CargarMedidas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la medida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMedidas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var medida = (Medida)dgvMedidas.Rows[e.RowIndex].DataBoundItem;
                    medidaIdSeleccionada = medida.IdMedida;
                    txtNombreMedida.Text = medida.NombreMedida;
                    
                    // Establecer modo edición
                    EstablecerModoEdicion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la medida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMedidas_SelectionChanged(object sender, EventArgs e)
        {
            // Solo habilitar el botón eliminar si hay una fila seleccionada y no estamos en modo edición
            if (!medidaIdSeleccionada.HasValue)
            {
                btnEliminar.Enabled = dgvMedidas.SelectedRows.Count > 0;
            }
        }

        private void dgvMedidas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Solo limpiar selección si no estamos en modo edición
            if (!medidaIdSeleccionada.HasValue)
            {
                dgvMedidas.ClearSelection();
                btnEliminar.Enabled = false;
            }
        }
    }
}
