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
        private int? tipoConectorIdSeleccionado = null;

        public frmGestionTipoConector()
        {
            InitializeComponent();
            ConfigurarEstadoInicial();
        }

        private void frmGestionTipoConector_Load(object sender, EventArgs e)
        {
            CargarTiposConector();
            EstiloDataGridView();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvTipoConector.DataBindingComplete += dgvTipoConector_DataBindingComplete;
            dgvTipoConector.CellDoubleClick += dgvTipoConector_CellDoubleClick;
            dgvTipoConector.SelectionChanged += dgvTipoConector_SelectionChanged;
            
            // Configurar tecla ESC para cancelar edición
            this.KeyPreview = true;
            this.KeyDown += frmGestionTipoConector_KeyDown;
        }

        private void frmGestionTipoConector_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && tipoConectorIdSeleccionado.HasValue)
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
            txtNombreTipoConector.Clear();
            txtNombreTipoConector.Focus();
        }

        private void EstablecerModoNuevo()
        {
            tipoConectorIdSeleccionado = null;
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            txtNombreTipoConector.Clear();
            txtNombreTipoConector.Focus();
            dgvTipoConector.ClearSelection();
        }

        private void EstablecerModoEdicion()
        {
            btnGuardar.Enabled = false;
            btnEditar.Enabled = true;
            btnEliminar.Enabled = false;
        }

        private void CargarTiposConector()
        {
            try
            {
                List<TipoConector> tiposConector = _gestorTipoConector.ListarTipoConectores();
                dgvTipoConector.DataSource = tiposConector;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los tipos de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreTipoConector.Text))
            {
                MessageBox.Show("El nombre del tipo de conector es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreTipoConector.Focus();
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                var tipoConector = new TipoConector
                {
                    NombreTipoConector = txtNombreTipoConector.Text.Trim()
                };

                _gestorTipoConector.RegistrarTipoConector(tipoConector);
                MessageBox.Show("Tipo de conector registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarTiposConector();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!tipoConectorIdSeleccionado.HasValue)
                {
                    MessageBox.Show("Debe seleccionar un tipo de conector para editar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos()) return;

                var tipoConector = new TipoConector
                {
                    IdTipoConector = tipoConectorIdSeleccionado.Value,
                    NombreTipoConector = txtNombreTipoConector.Text.Trim()
                };

                _gestorTipoConector.ModificarTipoConector(tipoConector);
                MessageBox.Show("Tipo de conector modificado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarTiposConector();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int? tipoConectorIdAEliminar = null;
                
                // Si estamos en modo edición, usar el tipo de conector seleccionado
                if (tipoConectorIdSeleccionado.HasValue)
                {
                    tipoConectorIdAEliminar = tipoConectorIdSeleccionado.Value;
                }
                // Si no estamos en modo edición, verificar si hay una fila seleccionada
                else if (dgvTipoConector.SelectedRows.Count > 0)
                {
                    var tipoConector = (TipoConector)dgvTipoConector.SelectedRows[0].DataBoundItem;
                    tipoConectorIdAEliminar = tipoConector.IdTipoConector;
                }
                
                if (!tipoConectorIdAEliminar.HasValue)
                {
                    MessageBox.Show("Debe seleccionar un tipo de conector para eliminar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var tipoConectorAEliminar = dgvTipoConector.SelectedRows.Count > 0 ? 
                    (TipoConector)dgvTipoConector.SelectedRows[0].DataBoundItem : 
                    ((List<TipoConector>)dgvTipoConector.DataSource).FirstOrDefault(tc => tc.IdTipoConector == tipoConectorIdAEliminar.Value);

                string nombreTipoConector = tipoConectorAEliminar?.NombreTipoConector ?? "este tipo de conector";

                if (MessageBox.Show($"¿Está seguro de eliminar el tipo de conector '{nombreTipoConector}'?", 
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _gestorTipoConector.EliminarTipoConector(tipoConectorIdAEliminar.Value);
                    MessageBox.Show("Tipo de conector eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    EstablecerModoNuevo();
                    CargarTiposConector();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTipoConector_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var tipoConector = (TipoConector)dgvTipoConector.Rows[e.RowIndex].DataBoundItem;
                    tipoConectorIdSeleccionado = tipoConector.IdTipoConector;
                    txtNombreTipoConector.Text = tipoConector.NombreTipoConector;
                    
                    // Establecer modo edición
                    EstablecerModoEdicion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el tipo de conector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTipoConector_SelectionChanged(object sender, EventArgs e)
        {
            // Solo habilitar el botón eliminar si hay una fila seleccionada y no estamos en modo edición
            if (!tipoConectorIdSeleccionado.HasValue)
            {
                btnEliminar.Enabled = dgvTipoConector.SelectedRows.Count > 0;
            }
        }

        private void dgvTipoConector_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Solo limpiar selección si no estamos en modo edición
            if (!tipoConectorIdSeleccionado.HasValue)
            {
                dgvTipoConector.ClearSelection();
                btnEliminar.Enabled = false;
            }
        }
    }
}
