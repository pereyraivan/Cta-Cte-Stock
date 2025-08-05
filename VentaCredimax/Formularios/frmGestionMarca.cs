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
    public partial class frmGestionMarca : Form
    {
        private GestorMarca _gestorMarca = new GestorMarca();
        private int? marcaIdSeleccionada = null;

        public frmGestionMarca()
        {
            InitializeComponent();
            ConfigurarEstadoInicial();
        }

        private void frmGestionMarca_Load(object sender, EventArgs e)
        {
            CargarMarcas();
            EstiloDataGridView();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvMarcas.DataBindingComplete += dgvMarcas_DataBindingComplete;
            dgvMarcas.CellDoubleClick += dgvMarcas_CellDoubleClick;
            dgvMarcas.SelectionChanged += dgvMarcas_SelectionChanged;
            
            // Configurar tecla ESC para cancelar edición
            this.KeyPreview = true;
            this.KeyDown += frmGestionMarca_KeyDown;
        }

        private void frmGestionMarca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && marcaIdSeleccionada.HasValue)
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
            txtNombreMarca.Clear();
            txtNombreMarca.Focus();
        }

        private void EstablecerModoNuevo()
        {
            marcaIdSeleccionada = null;
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            txtNombreMarca.Clear();
            txtNombreMarca.Focus();
            dgvMarcas.ClearSelection();
        }

        private void EstablecerModoEdicion()
        {
            btnGuardar.Enabled = false;
            btnEditar.Enabled = true;
            btnEliminar.Enabled = false;
        }

        private void CargarMarcas()
        {
            try
            {
                List<Marca> marcas = _gestorMarca.ListarMarcas();
                dgvMarcas.DataSource = marcas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstiloDataGridView()
        {
            if (dgvMarcas.Rows.Count > 0)
            {
                dgvMarcas.Columns["IdMarca"].HeaderText = "ID";
                dgvMarcas.Columns["NombreMarca"].HeaderText = "Nombre de Marca";
                dgvMarcas.Columns["IdMarca"].Width = 80;
                dgvMarcas.Columns["NombreMarca"].Width = 200;
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MessageBox.Show("El nombre de la marca es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreMarca.Focus();
                return false;
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                var marca = new Marca
                {
                    NombreMarca = txtNombreMarca.Text.Trim()
                };

                _gestorMarca.RegistrarMarca(marca);
                MessageBox.Show("Marca registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la marca: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!marcaIdSeleccionada.HasValue)
                {
                    MessageBox.Show("Debe seleccionar una marca para editar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos()) return;

                var marca = new Marca
                {
                    IdMarca = marcaIdSeleccionada.Value,
                    NombreMarca = txtNombreMarca.Text.Trim()
                };

                _gestorMarca.ModificarMarca(marca);
                MessageBox.Show("Marca modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                EstablecerModoNuevo();
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar la marca: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int? marcaIdAEliminar = null;
                
                // Si estamos en modo edición, usar la marca seleccionada
                if (marcaIdSeleccionada.HasValue)
                {
                    marcaIdAEliminar = marcaIdSeleccionada.Value;
                }
                // Si no estamos en modo edición, verificar si hay una fila seleccionada
                else if (dgvMarcas.SelectedRows.Count > 0)
                {
                    var marca = (Marca)dgvMarcas.SelectedRows[0].DataBoundItem;
                    marcaIdAEliminar = marca.IdMarca;
                }
                
                if (!marcaIdAEliminar.HasValue)
                {
                    MessageBox.Show("Debe seleccionar una marca para eliminar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var marcaAEliminar = dgvMarcas.SelectedRows.Count > 0 ? 
                    (Marca)dgvMarcas.SelectedRows[0].DataBoundItem : 
                    ((List<Marca>)dgvMarcas.DataSource).FirstOrDefault(m => m.IdMarca == marcaIdAEliminar.Value);

                string nombreMarca = marcaAEliminar?.NombreMarca ?? "esta marca";

                if (MessageBox.Show($"¿Está seguro de eliminar la marca '{nombreMarca}'?", 
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _gestorMarca.EliminarMarca(marcaIdAEliminar.Value);
                    MessageBox.Show("Marca eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    EstablecerModoNuevo();
                    CargarMarcas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la marca: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMarcas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var marca = (Marca)dgvMarcas.Rows[e.RowIndex].DataBoundItem;
                    marcaIdSeleccionada = marca.IdMarca;
                    txtNombreMarca.Text = marca.NombreMarca;
                    
                    // Establecer modo edición
                    EstablecerModoEdicion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la marca: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMarcas_SelectionChanged(object sender, EventArgs e)
        {
            // Solo habilitar el botón eliminar si hay una fila seleccionada y no estamos en modo edición
            if (!marcaIdSeleccionada.HasValue)
            {
                btnEliminar.Enabled = dgvMarcas.SelectedRows.Count > 0;
            }
        }

        private void dgvMarcas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Solo limpiar selección si no estamos en modo edición
            if (!marcaIdSeleccionada.HasValue)
            {
                dgvMarcas.ClearSelection();
                btnEliminar.Enabled = false;
            }
        }
    }
}
