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

        public frmArticulo()
        {
            InitializeComponent();
            _gestorArticulo = new GestorArticulo();
            ConfigurarGrilla();
            CargarDatos();
        }

        private void ConfigurarGrilla()
        {
            dgvVentas.AutoGenerateColumns = false;
            dgvVentas.Columns.Clear();

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ArticuloId",
                HeaderText = "ID",
                Visible = false
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Codigo",
                HeaderText = "Código",
                Width = 100
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Artículo",
                Width = 300
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioCompra",
                HeaderText = "Precio Compra",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioVenta",
                HeaderText = "Precio Venta",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Stock",
                Width = 80
            });
        }

        private void CargarDatos()
        {
            try
            {
                var articulos = _gestorArticulo.Listar();
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = articulos;
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
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtArticulo.Text.Trim()))
            {
                MessageBox.Show("Debe ingresar una descripción para el artículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtArticulo.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                MessageBox.Show("Debe ingresar un código para el artículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
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
                    Stock = int.Parse(txtStock.Text)
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
                if (!articuloIdSeleccionado.HasValue)
                {
                    MessageBox.Show("Debe seleccionar un artículo para eliminar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("¿Está seguro que desea eliminar este artículo?", "Confirmación", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _gestorArticulo.Eliminar(articuloIdSeleccionado.Value);
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

        private void dgvVentas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var articulo = (Articulo)dgvVentas.Rows[e.RowIndex].DataBoundItem;
                    articuloIdSeleccionado = articulo.ArticuloId;
                    txtArticulo.Text = articulo.Descripcion;
                    txtCodigo.Text = articulo.Codigo;
                    txtPrecioCompra.Text = articulo.PrecioCompra.ToString();
                    txtPrecioVenta.Text = articulo.PrecioVenta.ToString();
                    txtStock.Text = articulo.Stock.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
