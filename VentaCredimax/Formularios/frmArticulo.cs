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
    public partial class frmArticulo : Form
    {
        private GestorArticulo _gestorArticulo;
        private int? articuloIdSeleccionado = null;
        private List<Articulo> _articulosCompletos; // Lista completa para filtrado

        public frmArticulo()
        {
            InitializeComponent();
            _gestorArticulo = new GestorArticulo();
            ConfigurarGrilla();
            ConfigurarCampos();
            CargarDatos();
            
            // Suscribirse al evento para pintar las filas
            dgvArticulos.DataBindingComplete += DgvArticulos_DataBindingComplete;
            
            // Suscribirse al evento de selección de fila
            dgvArticulos.SelectionChanged += DgvArticulos_SelectionChanged;
            
            // Suscribirse al evento de búsqueda en tiempo real
            textBox1.TextChanged += textBox1_TextChanged;
            
            // Configurar estado inicial de los botones
            ConfigurarEstadoBotones();
        }

        private void ConfigurarCampos()
        {
            // Configurar tooltip para el campo código
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(txtCodigo, "Campo opcional - Si se deja vacío, se generará automáticamente de forma correlativa");
            
            // Configurar tooltip para el campo de búsqueda
            tooltip.SetToolTip(textBox1, "Escriba aquí para buscar artículos por código o descripción en tiempo real");
        }

        private void ConfigurarGrilla()
        {
            dgvArticulos.AutoGenerateColumns = false;
            dgvArticulos.Columns.Clear();

            // Configurar el estilo del encabezado
            dgvArticulos.EnableHeadersVisualStyles = false;
            dgvArticulos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(164, 196, 231);
            dgvArticulos.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvArticulos.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            dgvArticulos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Configurar el estilo de las filas
            dgvArticulos.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F);
            dgvArticulos.RowHeadersVisible = false; // Ocultar la primera columna (row headers)
            dgvArticulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArticulos.MultiSelect = false;

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ArticuloId",
                HeaderText = "ID",
                Visible = false
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Codigo",
                HeaderText = "Código",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Artículo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioCompra",
                HeaderText = "Precio Compra",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioVenta",
                HeaderText = "Precio Venta",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Stock",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockMinimo",
                HeaderText = "Stock Mínimo",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
        }

        private void DgvArticulos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Pintar las filas donde el stock sea menor o igual al stock mínimo
            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                if (row.DataBoundItem is Articulo articulo)
                {
                    if (articulo.Stock <= articulo.StockMinimo)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void DgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            // Solo actualizar el estado del botón eliminar si no estamos en modo edición
            if (!articuloIdSeleccionado.HasValue)
            {
                btnEliminarArticulo.Enabled = dgvArticulos.SelectedRows.Count > 0;
            }
        }

        private void ConfigurarEstadoBotones()
        {
            // Estado inicial - modo nuevo artículo
            btnGuardarArticulo.Enabled = true;
            btnEditarArticulo.Enabled = false;
            btnEliminarArticulo.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void EstablecerModoNuevo()
        {
            btnGuardarArticulo.Enabled = true;
            btnEditarArticulo.Enabled = false;
            btnEliminarArticulo.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void EstablecerModoEdicion()
        {
            btnGuardarArticulo.Enabled = false;
            btnEditarArticulo.Enabled = true;
            btnEliminarArticulo.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void CargarDatos()
        {
            try
            {
                _articulosCompletos = _gestorArticulo.Listar();
                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = _articulosCompletos;

                // Quitar la selección automática de la primera fila
                dgvArticulos.ClearSelection();
                
                // Si no estamos en modo edición, deshabilitar el botón eliminar
                if (!articuloIdSeleccionado.HasValue)
                {
                    btnEliminarArticulo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            articuloIdSeleccionado = null;
            txtArticulo.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtPrecioCompra.Text = string.Empty;
            txtPrecioVenta.Text = string.Empty;
            txtStock.Text = "0";
            txtStockMinimo.Text = "0";
            dtpFechaCancelacion.Value = DateTime.Now;
            txtArticulo.Focus();
            
            // Establecer modo nuevo artículo
            EstablecerModoNuevo();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtArticulo.Text.Trim()))
            {
                MessageBox.Show("Debe ingresar una descripción para el artículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtArticulo.Focus();
                return false;
            }

            // El código es opcional - se genera automáticamente si está vacío
            // Solo validar si se ingresó un código manualmente
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                // Aquí podrías agregar validaciones adicionales para el formato del código si es necesario
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra < 0)
            {
                MessageBox.Show("Debe ingresar un precio de compra válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta) || precioVenta < 0)
            {
                MessageBox.Show("Debe ingresar un precio de venta válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus();
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Debe ingresar una cantidad de stock válida", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            return true;
        }

        private void frmArticulo_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnGuardarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                var articulo = new Articulo
                {
                    Descripcion = txtArticulo.Text.Trim(),
                    Codigo = txtCodigo.Text.Trim(),
                    PrecioCompra = decimal.Parse(txtPrecioCompra.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    Stock = int.Parse(txtStock.Text),
                    StockMinimo = int.Parse(txtStockMinimo.Text),
                    FechaAlta = DateTime.Now
                };

                _gestorArticulo.Guardar(articulo);
                MessageBox.Show("Artículo guardado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!articuloIdSeleccionado.HasValue)
                {
                    MessageBox.Show("Debe seleccionar un artículo para editar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos()) return;

                var articulo = new Articulo
                {
                    ArticuloId = articuloIdSeleccionado.Value,
                    Descripcion = txtArticulo.Text.Trim(),
                    Codigo = txtCodigo.Text.Trim(),
                    PrecioCompra = decimal.Parse(txtPrecioCompra.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    Stock = int.Parse(txtStock.Text),
                    StockMinimo = int.Parse(txtStockMinimo.Text),
                };

                _gestorArticulo.Modificar(articulo);
                MessageBox.Show("Artículo modificado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                int? articuloIdAEliminar = null;
                
                // Si estamos en modo edición, usar el artículo seleccionado
                if (articuloIdSeleccionado.HasValue)
                {
                    articuloIdAEliminar = articuloIdSeleccionado.Value;
                }
                // Si no estamos en modo edición, verificar si hay una fila seleccionada
                else if (dgvArticulos.SelectedRows.Count > 0)
                {
                    var articulo = (Articulo)dgvArticulos.SelectedRows[0].DataBoundItem;
                    articuloIdAEliminar = articulo.ArticuloId;
                }
                
                if (!articuloIdAEliminar.HasValue)
                {
                    MessageBox.Show("Debe seleccionar un artículo para eliminar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("¿Está seguro que desea eliminar este artículo?", "Confirmación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _gestorArticulo.Eliminar(articuloIdAEliminar.Value);
                    MessageBox.Show("Artículo eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarCampos();
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvArticulos_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var articulo = (Articulo)dgvArticulos.Rows[e.RowIndex].DataBoundItem;
                    articuloIdSeleccionado = articulo.ArticuloId;
                    txtArticulo.Text = articulo.Descripcion;
                    txtCodigo.Text = articulo.Codigo;
                    txtPrecioCompra.Text = articulo.PrecioCompra.ToString();
                    txtPrecioVenta.Text = articulo.PrecioVenta.ToString();
                    txtStock.Text = articulo.Stock.ToString();
                    txtStockMinimo.Text = articulo.StockMinimo.ToString();
                    
                    // Establecer modo edición
                    EstablecerModoEdicion();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FiltrarArticulos();
        }

        private void FiltrarArticulos()
        {
            try
            {
                if (_articulosCompletos == null || _articulosCompletos.Count == 0)
                    return;

                string filtro = textBox1.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(filtro))
                {
                    // Si no hay filtro, mostrar todos los artículos
                    dgvArticulos.DataSource = null;
                    dgvArticulos.DataSource = _articulosCompletos;
                }
                else
                {
                    // Filtrar por código o descripción
                    var articulosFiltrados = _articulosCompletos.Where(a =>
                        (a.Codigo != null && a.Codigo.ToLower().Contains(filtro)) ||
                        (a.Descripcion != null && a.Descripcion.ToLower().Contains(filtro))
                    ).ToList();

                    dgvArticulos.DataSource = null;
                    dgvArticulos.DataSource = articulosFiltrados;
                }

                // Quitar la selección después del filtrado
                dgvArticulos.ClearSelection();

                // Si no estamos en modo edición, deshabilitar el botón eliminar
                if (!articuloIdSeleccionado.HasValue)
                {
                    btnEliminarArticulo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
