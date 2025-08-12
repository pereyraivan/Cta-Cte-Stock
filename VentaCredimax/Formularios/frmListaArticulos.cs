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
        private GestorMarca _gestorMarca = new GestorMarca();
        private GestorMedida _gestorMedida = new GestorMedida();
        private GestorTipoConector _gestorTipoConector = new GestorTipoConector();
        public Articulo ArticuloSeleccionado { get; private set; }
        private frmVentas formularioVenta;
        private List<dynamic> _articulosCompletos; // Lista completa para filtrado
        public frmListaArticulos(frmVentas frmPadre)
        {
            InitializeComponent();
            this.formularioVenta = frmPadre;
        }

        private void frmListaArticulos_Load(object sender, EventArgs e)
        {
            CargarArticulos();
            ConfigurarGrilla();
            ConfigurarComboFiltro();
        }

        private void CargarArticulos()
        {
            try
            {
                // Usar el nuevo método que incluye los nombres
                var articulosVista = _gestorArticulo.ListarConDetalles();
                
                // Guardar la lista completa para filtrado
                _articulosCompletos = articulosVista.ToList();

                dgvArticulos.DataSource = articulosVista;
                PintarFilasStockMinimo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarComboFiltro()
        {
            // Configurar el ComboBox de filtros
            try
            {
                cbFiltrar.Items.Clear();
                cbFiltrar.Items.Add("Descripción"); // Filtro por defecto
                cbFiltrar.Items.Add("Código");
                cbFiltrar.Items.Add("Marca");
                cbFiltrar.Items.Add("Medida");
                cbFiltrar.Items.Add("Tipo Conector");
                cbFiltrar.SelectedIndex = 0; // Seleccionar "Descripción" por defecto
                
                // Suscribirse al evento de cambio
                cbFiltrar.SelectedIndexChanged += CbFiltrar_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar combo filtro: {ex.Message}");
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
                    {
                        dgvArticulos.Columns["ArticuloId"].HeaderText = "ID";
                        dgvArticulos.Columns["ArticuloId"].Visible = false; // Ocultar columna ID
                    }

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
                    dgvArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Permitir que el usuario redimensione las columnas
                    dgvArticulos.AllowUserToResizeColumns = true;
                    dgvArticulos.AllowUserToResizeRows = false;

                    // Configurar anchos mínimos para columnas principales
                    if (dgvArticulos.Columns["ArticuloId"] != null)
                        dgvArticulos.Columns["ArticuloId"].MinimumWidth = 50;

                    if (dgvArticulos.Columns["Codigo"] != null)
                        dgvArticulos.Columns["Codigo"].MinimumWidth = 80;

                    if (dgvArticulos.Columns["Descripcion"] != null)
                    {
                        dgvArticulos.Columns["Descripcion"].MinimumWidth = 200;
                        dgvArticulos.Columns["Descripcion"].FillWeight = 200; // Dar más peso a la descripción
                    }

                    if (dgvArticulos.Columns["Marca"] != null)
                        dgvArticulos.Columns["Marca"].MinimumWidth = 100;

                    if (dgvArticulos.Columns["Medida"] != null)
                        dgvArticulos.Columns["Medida"].MinimumWidth = 80;

                    if (dgvArticulos.Columns["TipoConector"] != null)
                        dgvArticulos.Columns["TipoConector"].MinimumWidth = 120;

                    if (dgvArticulos.Columns["PrecioCompra"] != null)
                    {
                        dgvArticulos.Columns["PrecioCompra"].MinimumWidth = 100;
                        dgvArticulos.Columns["PrecioCompra"].FillWeight = 80; // Peso menor para precios
                    }
                    
                    if (dgvArticulos.Columns["PrecioVenta"] != null)
                    {
                        dgvArticulos.Columns["PrecioVenta"].MinimumWidth = 100;
                        dgvArticulos.Columns["PrecioVenta"].FillWeight = 80; // Peso menor para precios
                    }
                    
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
                var textoBusqueda = txtBuscar.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    // Si no hay texto de búsqueda, mostrar todos los artículos
                    dgvArticulos.DataSource = _articulosCompletos;
                }
                else
                {
                    // Determinar el criterio de filtrado seleccionado
                    string criterioFiltro = "Descripción"; // Valor por defecto
                    try
                    {
                        if (cbFiltrar.SelectedItem != null)
                        {
                            criterioFiltro = cbFiltrar.SelectedItem.ToString();
                        }
                    }
                    catch
                    {
                        criterioFiltro = "Descripción";
                    }

                    // Aplicar filtro según el criterio seleccionado
                    var articulosFiltrados = FiltrarArticulos(textoBusqueda, criterioFiltro);
                    dgvArticulos.DataSource = articulosFiltrados;
                }

                PintarFilasStockMinimo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<dynamic> FiltrarArticulos(string filtro, string criterio)
        {
            if (_articulosCompletos == null) return new List<dynamic>();

            switch (criterio)
            {
                case "Código":
                    return _articulosCompletos.Where(a => 
                        GetPropertyValue(a, "Codigo")?.ToString().ToLower().Contains(filtro) == true).ToList();
                        
                case "Marca":
                    return _articulosCompletos.Where(a => 
                        GetPropertyValue(a, "Marca")?.ToString().ToLower().Contains(filtro) == true).ToList();
                        
                case "Medida":
                    return _articulosCompletos.Where(a => 
                        GetPropertyValue(a, "Medida")?.ToString().ToLower().Contains(filtro) == true).ToList();
                        
                case "Tipo Conector":
                    return _articulosCompletos.Where(a => 
                        GetPropertyValue(a, "TipoConector")?.ToString().ToLower().Contains(filtro) == true).ToList();
                        
                case "Descripción":
                default:
                    return _articulosCompletos.Where(a => 
                        GetPropertyValue(a, "Descripcion")?.ToString().ToLower().Contains(filtro) == true).ToList();
            }
        }

        private object GetPropertyValue(dynamic obj, string propertyName)
        {
            try
            {
                var type = obj.GetType();
                var property = type.GetProperty(propertyName);
                return property?.GetValue(obj);
            }
            catch
            {
                return null;
            }
        }

        private void CbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Aplicar filtro inmediatamente cuando cambia el criterio
            Buscar();
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
