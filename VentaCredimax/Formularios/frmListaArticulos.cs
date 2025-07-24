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
                var articulos = _gestorArticulo.Listar();

                dgvArticulos.DataSource = articulos;
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

                    // Ocultar columnas no necesarias
                    if (dgvArticulos.Columns["FechaAnulacion"] != null)
                        dgvArticulos.Columns["FechaAnulacion"].Visible = false;

                    // Ocultar columnas de relaciones (DetalleVenta y MovimientoStock)
                    if (dgvArticulos.Columns["DetalleVenta"] != null)
                        dgvArticulos.Columns["DetalleVenta"].Visible = false;

                    if (dgvArticulos.Columns["MovimientoStock"] != null)
                        dgvArticulos.Columns["MovimientoStock"].Visible = false;

                    // Configurar que las columnas ocupen todo el ancho del DataGridView
                    dgvArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Configurar anchos específicos para columnas principales
                    if (dgvArticulos.Columns["ArticuloId"] != null)
                        dgvArticulos.Columns["ArticuloId"].FillWeight = 10; // ID más estrecho

                    if (dgvArticulos.Columns["Codigo"] != null)
                        dgvArticulos.Columns["Codigo"].FillWeight = 20; // Código estrecho

                    if (dgvArticulos.Columns["Descripcion"] != null)
                        dgvArticulos.Columns["Descripcion"].FillWeight = 50; // Descripción más ancha

                    if (dgvArticulos.Columns["PrecioCompra"] != null)
                        dgvArticulos.Columns["PrecioCompra"].FillWeight = 30;
                    
                    if (dgvArticulos.Columns["PrecioVenta"] != null)
                        dgvArticulos.Columns["PrecioVenta"].FillWeight = 30;
                    
                    if (dgvArticulos.Columns["Stock"] != null)
                        dgvArticulos.Columns["Stock"].FillWeight = 10;

                    if (dgvArticulos.Columns["StockMinimo"] != null)
                        dgvArticulos.Columns["StockMinimo"].FillWeight = 25;

                    if (dgvArticulos.Columns["FechaAlta"] != null)
                        dgvArticulos.Columns["FechaAlta"].FillWeight = 25;

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
                    if (row.DataBoundItem is Articulo articulo)
                    {
                        // Pintar de rojo si el stock actual es menor o igual al stock mínimo
                        if (articulo.Stock <= articulo.StockMinimo)
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
                List<Articulo> articulosFiltrados;

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    // Si no hay texto de búsqueda, mostrar todos los artículos activos
                    articulosFiltrados = _gestorArticulo.Listar();
                }
                else
                {
                    // Buscar por código o descripción
                    var articulosPorCodigo = _gestorArticulo.BuscarPorCodigo(textoBusqueda);
                    var articulosPorDescripcion = _gestorArticulo.BuscarPorNombre(textoBusqueda);

                    // Combinar resultados y eliminar duplicados
                    articulosFiltrados = articulosPorCodigo
                        .Union(articulosPorDescripcion)
                        .ToList();
                }

                dgvArticulos.DataSource = articulosFiltrados;
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
                ArticuloSeleccionado = (Articulo)dgvArticulos.SelectedRows[0].DataBoundItem;
                this.DialogResult = DialogResult.OK;
                this.Close();
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
