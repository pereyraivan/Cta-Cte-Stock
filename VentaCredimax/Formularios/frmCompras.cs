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
    public partial class frmCompras : Form
    {
        private GestorCompra gestorCompra;
        private GestorArticulo gestorArticulo;
        private Compra compraSeleccionada;
        public frmCompras()
        {
            InitializeComponent();
            gestorCompra = new GestorCompra();
            gestorArticulo = new GestorArticulo();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {
            CargarArticulos();
            CargarCompras();
            ConfigurarControles();
            dtpFechaCompra.Value = DateTime.Now;
        }

        private void CargarArticulos()
        {
            try
            {
                var articulos = gestorArticulo.Listar();
                
                cbArticulo.DataSource = null;
                cbArticulo.Items.Clear();
                
                cbArticulo.DataSource = articulos;
                cbArticulo.DisplayMember = "Descripcion";
                cbArticulo.ValueMember = "ArticuloId";
                cbArticulo.SelectedIndex = -1;
                
                // Configurar AutoComplete para el ComboBox
                cbArticulo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbArticulo.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCompras()
        {
            try
            {
                // Limpiar eventos temporalmente
                if (dgvCompras.DataSource != null)
                {
                    dgvCompras.SelectionChanged -= dgvCompras_SelectionChanged;
                    dgvCompras.DataSource = null;
                }
                
                var compras = gestorCompra.ListarComprasConArticulos();
                dgvCompras.DataSource = compras;
                ConfigurarDataGridView();
                
                // Restaurar eventos
                dgvCompras.SelectionChanged += dgvCompras_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar compras: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            if (dgvCompras.Columns.Count > 0)
            {
                // Ocultar la columna ID
                if (dgvCompras.Columns["IdCompra"] != null)
                {
                    dgvCompras.Columns["IdCompra"].Visible = false;
                }

                // Configurar encabezados con formato
                if (dgvCompras.Columns["NombreArticulo"] != null)
                {
                    dgvCompras.Columns["NombreArticulo"].HeaderText = "Artículo";
                }

                if (dgvCompras.Columns["CodigoArticulo"] != null)
                {
                    dgvCompras.Columns["CodigoArticulo"].HeaderText = "Código";
                }

                if (dgvCompras.Columns["Precio"] != null)
                {
                    dgvCompras.Columns["Precio"].HeaderText = "Precio Unit.";
                    dgvCompras.Columns["Precio"].DefaultCellStyle.Format = "C2";
                }

                if (dgvCompras.Columns["Cantidad"] != null)
                    dgvCompras.Columns["Cantidad"].HeaderText = "Cantidad";

                if (dgvCompras.Columns["Total"] != null)
                {
                    dgvCompras.Columns["Total"].HeaderText = "Total";
                    dgvCompras.Columns["Total"].DefaultCellStyle.Format = "C2";
                }

                if (dgvCompras.Columns["FechaCompra"] != null)
                {
                    dgvCompras.Columns["FechaCompra"].HeaderText = "Fecha Compra";
                    dgvCompras.Columns["FechaCompra"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }

                // Ocultar columnas innecesarias
                if (dgvCompras.Columns["IdArticulo"] != null)
                    dgvCompras.Columns["IdArticulo"].Visible = false;

                // Configurar estilo de encabezados
                dgvCompras.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                dgvCompras.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgvCompras.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dgvCompras.ColumnHeadersHeight = 30;

                dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCompras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCompras.MultiSelect = false;
                dgvCompras.ReadOnly = true;
            }
        }

        private void ConfigurarControles()
        {
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnGuardar.Enabled = true;
            btnEditar.Text = "Editar";
            compraSeleccionada = null;
            // txtBuscar.PlaceholderText = "Buscar por nombre de artículo..."; // Comentado porque no está disponible en todas las versiones
        }

        private void btnBuscarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                frmListaArticulos frmLista = new frmListaArticulos(null);
                if (frmLista.ShowDialog() == DialogResult.OK)
                {
                    if (frmLista.ArticuloSeleccionado != null)
                    {
                        // Seleccionar el artículo en el ComboBox
                        cbArticulo.SelectedValue = frmLista.ArticuloSeleccionado.ArticuloId;
                        
                        // Opcional: poner el precio de compra del artículo como sugerencia
                        if (frmLista.ArticuloSeleccionado.PrecioCompra > 0)
                        {
                            txtPrecio.Text = frmLista.ArticuloSeleccionado.PrecioCompra.ToString("F2");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la lista de artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDatos())
                    return;

                int idArticulo = (int)cbArticulo.SelectedValue;
                decimal precio = decimal.Parse(txtPrecio.Text);
                int cantidad = int.Parse(txtCantidad.Text);
                DateTime fechaCompra = dtpFechaCompra.Value;

                string resultado = gestorCompra.GuardarCompra(idArticulo, precio, cantidad, fechaCompra);

                if (resultado == "OK")
                {
                    MessageBox.Show("Compra guardada exitosamente", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LimpiarFormulario();
                    CargarCompras();
                }
                else
                {
                    MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la compra: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (cbArticulo.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un artículo", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbArticulo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Debe ingresar el precio", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser un número válido mayor a 0", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Debe ingresar la cantidad", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return false;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número entero mayor a 0", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            cbArticulo.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
            dtpFechaCompra.Value = DateTime.Now;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Editar directamente si hay una compra seleccionada
            if (compraSeleccionada != null)
            {
                try
                {
                    if (!ValidarDatos())
                        return;

                    int idArticulo = (int)cbArticulo.SelectedValue;
                    decimal precio = decimal.Parse(txtPrecio.Text);
                    int cantidad = int.Parse(txtCantidad.Text);
                    DateTime fechaCompra = dtpFechaCompra.Value;

                    string resultado = gestorCompra.ModificarCompra(compraSeleccionada.IdCompra, idArticulo, precio, cantidad, fechaCompra);

                    if (resultado == "OK")
                    {
                        MessageBox.Show("Edición exitosa", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        LimpiarFormulario();
                        CargarCompras();
                        ConfigurarControles();
                    }
                    else
                    {
                        MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al modificar la compra: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe hacer doble clic sobre una fila de la grilla para seleccionar la compra a editar", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCompras.SelectedRows.Count > 0)
            {
                try
                {
                    int idCompra = Convert.ToInt32(dgvCompras.SelectedRows[0].Cells["IdCompra"].Value);
                    string nombreArticulo = dgvCompras.SelectedRows[0].Cells["NombreArticulo"].Value.ToString();
                    
                    DialogResult result = MessageBox.Show(
                        $"¿Está seguro que desea eliminar la compra del artículo '{nombreArticulo}'?", 
                        "Confirmar eliminación", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        string resultado = gestorCompra.EliminarCompra(idCompra);
                        
                        if (resultado == "OK")
                        {
                            MessageBox.Show("Compra eliminada exitosamente", "Éxito", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarCompras();
                            LimpiarFormulario();
                        }
                        else
                        {
                            MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar la compra: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una compra para eliminar", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Usar un timer para evitar muchas consultas consecutivas
                string textoBusqueda = txtBuscar.Text.Trim();
                
                // Deshabilitar eventos temporalmente
                dgvCompras.SelectionChanged -= dgvCompras_SelectionChanged;
                
                var compras = gestorCompra.BuscarComprasPorArticulo(textoBusqueda);
                dgvCompras.DataSource = compras;
                ConfigurarDataGridView();
                
                // Restaurar eventos
                dgvCompras.SelectionChanged += dgvCompras_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar compras: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCompras_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Solo habilitar eliminar cuando hay una fila seleccionada
                if (dgvCompras.SelectedRows.Count > 0 && dgvCompras.DataSource != null)
                {
                    btnEliminar.Enabled = true;
                }
                else
                {
                    btnEliminar.Enabled = false;
                }
                
                // El botón editar solo se habilita con doble clic, no con selección simple
            }
            catch (Exception ex)
            {
                // Error silencioso para no interrumpir la interfaz
                System.Diagnostics.Debug.WriteLine($"Error en selección: {ex.Message}");
            }
        }

        private void dgvCompras_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Seleccionar la fila donde se hizo doble clic
                    dgvCompras.Rows[e.RowIndex].Selected = true;
                    
                    // Obtener el ID de la compra
                    int idCompra = Convert.ToInt32(dgvCompras.Rows[e.RowIndex].Cells["IdCompra"].Value);
                    compraSeleccionada = gestorCompra.ObtenerCompraPorId(idCompra);
                    
                    if (compraSeleccionada != null)
                    {
                        // Cargar datos en el formulario
                        cbArticulo.SelectedValue = compraSeleccionada.IdArticulo;
                        txtPrecio.Text = compraSeleccionada.Precio.ToString("F2");
                        txtCantidad.Text = compraSeleccionada.Cantidad.ToString();
                        dtpFechaCompra.Value = compraSeleccionada.FechaCompra ?? DateTime.Now;
                        
                        // Configurar botones para edición
                        btnEditar.Enabled = true;      // Habilitar editar solo con doble clic
                        btnGuardar.Enabled = false;    // Deshabilitar guardar al cargar para editar
                        btnEliminar.Enabled = true;    // Mantener eliminar habilitado
                        
                       
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la compra: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmCompras_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Limpiar formulario al presionar Escape
                LimpiarFormulario();
                ConfigurarControles();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                // Limpiar referencias para evitar memory leaks
                if (dgvCompras.DataSource != null)
                {
                    dgvCompras.DataSource = null;
                }
                
                if (cbArticulo.DataSource != null)
                {
                    cbArticulo.DataSource = null;
                }
                
                gestorCompra = null;
                gestorArticulo = null;
                compraSeleccionada = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cerrar formulario: {ex.Message}");
            }
            finally
            {
                base.OnFormClosed(e);
            }
        }
    }
}
