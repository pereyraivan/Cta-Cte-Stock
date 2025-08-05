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
    public partial class frmListaArticulos : Form
    {
        private GestorArticulo _gestorArticulo = new GestorArticulo();
        public Articulo ArticuloSeleccionado { get; private set; }
        private frmVentas formularioVenta;
        public frmListaArticulos(frmVentas frmPadre)
        {
            InitializeComponent();
            this.formularioVenta = frmPadre;
        }

        private void frmListaArticulos_Load(object sender, EventArgs e)
        {
            CargarArticulos();
            ConfigurarGrilla();
        }

        private void CargarArticulos()
        {
            try
            {
                // Usar el nuevo método que incluye los nombres
                var articulosVista = _gestorArticulo.ListarConDetalles();

                dgvArticulos.DataSource = articulosVista;
                PintarFilasStockMinimo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrilla()
        {
            try
            {
                if (dgvArticulos.Columns.Count > 0)
                {
                    // Configurar encabezados de forma segura
                    if (dgvArticulos.Columns["ArticuloId"] != null)
                        dgvArticulos.Columns["ArticuloId"].HeaderText = "ID";

                    if (dgvArticulos.Columns["Codigo"] != null)
                        dgvArticulos.Columns["Codigo"].HeaderText = "Código";

                    if (dgvArticulos.Columns["Descripcion"] != null)
                        dgvArticulos.Columns["Descripcion"].HeaderText = "Descripción";

                    if (dgvArticulos.Columns["Marca"] != null)
                        dgvArticulos.Columns["Marca"].HeaderText = "Marca";

                    if (dgvArticulos.Columns["Medida"] != null)
                        dgvArticulos.Columns["Medida"].HeaderText = "Medida";

                    if (dgvArticulos.Columns["TipoConector"] != null)
                        dgvArticulos.Columns["TipoConector"].HeaderText = "Tipo Conector";

                    if (dgvArticulos.Columns["PrecioCompra"] != null)
                    {
                        dgvArticulos.Columns["PrecioCompra"].HeaderText = "Precio Compra";
                        dgvArticulos.Columns["PrecioCompra"].DefaultCellStyle.Format = "C2";
                    }

                    if (dgvArticulos.Columns["PrecioVenta"] != null)
                    {
                        dgvArticulos.Columns["PrecioVenta"].HeaderText = "Precio Venta";
                        dgvArticulos.Columns["PrecioVenta"].DefaultCellStyle.Format = "C2";
                    }

                    if (dgvArticulos.Columns["Stock"] != null)
                        dgvArticulos.Columns["Stock"].HeaderText = "Stock";

                    if (dgvArticulos.Columns["StockMinimo"] != null)
                        dgvArticulos.Columns["StockMinimo"].HeaderText = "Stock Mínimo";

                    if (dgvArticulos.Columns["FechaAlta"] != null)
                    {
                        dgvArticulos.Columns["FechaAlta"].HeaderText = "Fecha Alta";
                        dgvArticulos.Columns["FechaAlta"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }

                    // Configurar scroll y ajuste de columnas
                    dgvArticulos.ScrollBars = ScrollBars.Both;
                    dgvArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Permitir que el usuario redimensione las columnas
                    dgvArticulos.AllowUserToResizeColumns = true;
                    dgvArticulos.AllowUserToResizeRows = false;

                    // Configurar anchos mínimos para columnas principales
                    if (dgvArticulos.Columns["ArticuloId"] != null)
                        dgvArticulos.Columns["ArticuloId"].MinimumWidth = 50;

                    if (dgvArticulos.Columns["Codigo"] != null)
                        dgvArticulos.Columns["Codigo"].MinimumWidth = 80;

                    if (dgvArticulos.Columns["Descripcion"] != null)
                        dgvArticulos.Columns["Descripcion"].MinimumWidth = 200;

                    if (dgvArticulos.Columns["Marca"] != null)
                        dgvArticulos.Columns["Marca"].MinimumWidth = 100;

                    if (dgvArticulos.Columns["Medida"] != null)
                        dgvArticulos.Columns["Medida"].MinimumWidth = 80;

                    if (dgvArticulos.Columns["TipoConector"] != null)
                        dgvArticulos.Columns["TipoConector"].MinimumWidth = 120;

                    if (dgvArticulos.Columns["PrecioCompra"] != null)
                        dgvArticulos.Columns["PrecioCompra"].MinimumWidth = 100;
                    
                    if (dgvArticulos.Columns["PrecioVenta"] != null)
                        dgvArticulos.Columns["PrecioVenta"].MinimumWidth = 100;
                    
                    if (dgvArticulos.Columns["Stock"] != null)
                        dgvArticulos.Columns["Stock"].MinimumWidth = 60;

                    if (dgvArticulos.Columns["StockMinimo"] != null)
                        dgvArticulos.Columns["StockMinimo"].MinimumWidth = 90;

                    if (dgvArticulos.Columns["FechaAlta"] != null)
                        dgvArticulos.Columns["FechaAlta"].MinimumWidth = 100;

                }

                // Configurar para que no se seleccione automáticamente la primera fila
                dgvArticulos.ClearSelection();
                dgvArticulos.CurrentCell = null;
            }
            catch (Exception ex)
            {
                // En caso de error en la configuración, continuar sin formateo
                System.Diagnostics.Debug.WriteLine($"Error al configurar grilla: {ex.Message}");
            }
        }

        private void PintarFilasStockMinimo()
        {
            try
            {
                foreach (DataGridViewRow row in dgvArticulos.Rows)
                {
                    if (row.DataBoundItem != null)
                    {
                        // Obtener los valores de Stock y StockMinimo del objeto anónimo
                        var stock = Convert.ToInt32(row.Cells["Stock"].Value);
                        var stockMinimo = Convert.ToInt32(row.Cells["StockMinimo"].Value);

                        // Pintar de rojo si el stock actual es menor o igual al stock mínimo
                        if (stock <= stockMinimo)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            row.DefaultCellStyle.ForeColor = Color.DarkRed;
                        }
                        else
                        {
                            // Restaurar colores normales
                            row.DefaultCellStyle.BackColor = Color.White;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }

                // Asegurar que no haya ninguna fila seleccionada
                dgvArticulos.ClearSelection();
                dgvArticulos.CurrentCell = null;
            }
            catch (Exception ex)
            {
                // En caso de error, continuar sin colorear
                System.Diagnostics.Debug.WriteLine($"Error al pintar filas: {ex.Message}");
            }
        }

        private void Buscar()
        {
            try
            {
                var textoBusqueda = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    // Si no hay texto de búsqueda, cargar todos los artículos con detalles
                    CargarArticulos();
                }
                else
                {
                    // Usar el método de búsqueda con detalles
                    var articulosVista = _gestorArticulo.BuscarConDetalles(textoBusqueda);
                    dgvArticulos.DataSource = articulosVista;
                }

                PintarFilasStockMinimo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.SelectedRows.Count > 0)
            {
                // Obtener el ID del artículo seleccionado
                var articuloId = Convert.ToInt32(dgvArticulos.SelectedRows[0].Cells["ArticuloId"].Value);
                
                // Buscar el artículo completo en la lista
                var articulos = _gestorArticulo.Listar();
                ArticuloSeleccionado = articulos.FirstOrDefault(a => a.ArticuloId == articuloId);
                
                if (ArticuloSeleccionado != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo obtener el artículo seleccionado.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un artículo.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvArticulos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Permitir seleccionar haciendo doble clic en una fila
            if (e.RowIndex >= 0)
            {
                dgvArticulos.Rows[e.RowIndex].Selected = true;
                btnSeleccionar_Click(sender, e);
            }
        }

        private void dgvArticulos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Aplicar formato después de que se complete el data binding
            ConfigurarGrilla();
            PintarFilasStockMinimo();
        }
    }
}
