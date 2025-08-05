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
        private GestorMarca _gestorMarca;
        private GestorMedida _gestorMedida;
        private GestorTipoConector _gestorTipoConector;
        private int? articuloIdSeleccionado = null;
        private List<Articulo> _articulosCompletos; // Lista completa para filtrado

        public frmArticulo()
        {
            InitializeComponent();
            _gestorArticulo = new GestorArticulo();
            _gestorMarca = new GestorMarca();
            _gestorMedida = new GestorMedida();
            _gestorTipoConector = new GestorTipoConector();
            ConfigurarGrilla();
            ConfigurarCampos();
            ConfigurarComboBox();
            CargarDatos();
            CargarComboBoxes();
            
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

        private void ConfigurarComboBox()
        {
            // Configurar AutoComplete para cbMarca
            cbMarca.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbMarca.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbMarca.DropDownStyle = ComboBoxStyle.DropDown;
            
            // Configurar AutoComplete para cbMedida
            cbMedida.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbMedida.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbMedida.DropDownStyle = ComboBoxStyle.DropDown;
            
            // Configurar AutoComplete para cbTipoConector
            cbTipoConector.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbTipoConector.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbTipoConector.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void CargarComboBoxes()
        {
            try
            {
                // Cargar ComboBox de Marcas
                var marcas = _gestorMarca.ListarMarcas();
                cbMarca.DataSource = marcas;
                cbMarca.DisplayMember = "NombreMarca";
                cbMarca.ValueMember = "IdMarca";
                cbMarca.SelectedIndex = -1; // No seleccionar ningún elemento por defecto

                // Cargar ComboBox de Medidas
                var medidas = _gestorMedida.ListarMedidas();
                cbMedida.DataSource = medidas;
                cbMedida.DisplayMember = "NombreMedida";
                cbMedida.ValueMember = "IdMedida";
                cbMedida.SelectedIndex = -1; // No seleccionar ningún elemento por defecto

                // Cargar ComboBox de Tipo Conector
                var tiposConector = _gestorTipoConector.ListarTipoConectores();
                cbTipoConector.DataSource = tiposConector;
                cbTipoConector.DisplayMember = "NombreTipoConector";
                cbTipoConector.ValueMember = "IdTipoConector";
                cbTipoConector.SelectedIndex = -1; // No seleccionar ningún elemento por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los ComboBoxes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                DataPropertyName = "NombreMarca",
                HeaderText = "Marca",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMedida",
                HeaderText = "Medida",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreTipoConector",
                HeaderText = "Tipo Conector",
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioCompra",
                HeaderText = "Precio Compra",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioVenta",
                HeaderText = "Precio Venta",
                Width = 120,
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
                try
                {
                    var stock = Convert.ToInt32(row.Cells["Stock"].Value);
                    var stockMinimo = Convert.ToInt32(row.Cells["StockMinimo"].Value);

                    if (stock <= stockMinimo)
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
                catch
                {
                    // En caso de error, usar estilo por defecto
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
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
                
                // Crear lista con datos extendidos para mostrar en la grilla
                var articulosParaGrilla = _articulosCompletos.Select(a => new
                {
                    ArticuloId = a.ArticuloId,
                    Codigo = a.Codigo,
                    Descripcion = a.Descripcion,
                    NombreMarca = ObtenerNombreMarca(a.IdMarca),
                    NombreMedida = ObtenerNombreMedida(a.IdMedida),
                    NombreTipoConector = ObtenerNombreTipoConector(a.IdTipoConector),
                    PrecioCompra = a.PrecioCompra,
                    PrecioVenta = a.PrecioVenta,
                    Stock = a.Stock,
                    StockMinimo = a.StockMinimo
                }).ToList();

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = articulosParaGrilla;

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

        private string ObtenerNombreMarca(int? idMarca)
        {
            if (!idMarca.HasValue) return "Sin marca";
            try
            {
                var marcas = _gestorMarca.ListarMarcas();
                var marca = marcas.FirstOrDefault(m => m.IdMarca == idMarca.Value);
                return marca?.NombreMarca ?? "Sin marca";
            }
            catch
            {
                return "Sin marca";
            }
        }

        private string ObtenerNombreMedida(int? idMedida)
        {
            if (!idMedida.HasValue) return "Sin medida";
            try
            {
                var medidas = _gestorMedida.ListarMedidas();
                var medida = medidas.FirstOrDefault(m => m.IdMedida == idMedida.Value);
                return medida?.NombreMedida ?? "Sin medida";
            }
            catch
            {
                return "Sin medida";
            }
        }

        private string ObtenerNombreTipoConector(int? idTipoConector)
        {
            if (!idTipoConector.HasValue) return "Sin tipo conector";
            try
            {
                var tiposConector = _gestorTipoConector.ListarTipoConectores();
                var tipoConector = tiposConector.FirstOrDefault(tc => tc.IdTipoConector == idTipoConector.Value);
                return tipoConector?.NombreTipoConector ?? "Sin tipo conector";
            }
            catch
            {
                return "Sin tipo conector";
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
            cbMarca.SelectedIndex = -1;
            cbMedida.SelectedIndex = -1;
            cbTipoConector.SelectedIndex = -1;
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

            if (cbMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbMarca.Focus();
                return false;
            }

            // La medida y el tipo de conector no son obligatorios
            // Dependiendo del tipo de artículo, se puede usar uno u otro o ambos

            // Validar que no exista un artículo duplicado
            if (!ValidarArticuloNoDuplicado())
            {
                return false;
            }

            return true;
        }

        private bool ValidarArticuloNoDuplicado()
        {
            try
            {
                // Obtener los valores actuales del formulario
                string descripcion = txtArticulo.Text.Trim();
                int? idMarca = cbMarca.SelectedIndex != -1 ? (int?)cbMarca.SelectedValue : null;
                int? idMedida = cbMedida.SelectedIndex != -1 ? (int?)cbMedida.SelectedValue : null;
                int? idTipoConector = cbTipoConector.SelectedIndex != -1 ? (int?)cbTipoConector.SelectedValue : null;

                // Buscar artículos existentes con la misma combinación
                var articulosExistentes = _gestorArticulo.Listar();
                
                var articuloDuplicado = articulosExistentes.FirstOrDefault(a =>
                    a.Descripcion.Trim().Equals(descripcion, StringComparison.OrdinalIgnoreCase) &&
                    a.IdMarca == idMarca &&
                    a.IdMedida == idMedida &&
                    a.IdTipoConector == idTipoConector &&
                    a.FechaAnulacion == null && // Solo considerar artículos activos
                    (!articuloIdSeleccionado.HasValue || a.ArticuloId != articuloIdSeleccionado.Value) // Excluir el artículo actual si estamos editando
                );

                if (articuloDuplicado != null)
                {
                    string marcaNombre = idMarca.HasValue ? ObtenerNombreMarca(idMarca) : "Sin marca";
                    string medidaNombre = idMedida.HasValue ? ObtenerNombreMedida(idMedida) : "Sin medida";
                    string tipoConectorNombre = idTipoConector.HasValue ? ObtenerNombreTipoConector(idTipoConector) : "Sin tipo conector";
                    
                    string mensaje = $"Ya existe un artículo con la siguiente combinación:\n\n" +
                                   $"Descripción: {descripcion}\n" +
                                   $"Marca: {marcaNombre}\n" +
                                   $"Medida: {medidaNombre}\n" +
                                   $"Tipo Conector: {tipoConectorNombre}\n\n" +
                                   $"Código existente: {articuloDuplicado.Codigo}\n\n" +
                                   $"Por favor, modifique alguno de estos valores para crear un artículo único.";
                    
                    MessageBox.Show(mensaje, "Artículo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtArticulo.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar duplicados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
                    FechaAlta = DateTime.Now,
                    IdMarca = (int)cbMarca.SelectedValue,
                    IdMedida = cbMedida.SelectedIndex != -1 ? (int?)cbMedida.SelectedValue : null,
                    IdTipoConector = cbTipoConector.SelectedIndex != -1 ? (int?)cbTipoConector.SelectedValue : null
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
                    IdMarca = (int)cbMarca.SelectedValue,
                    IdMedida = cbMedida.SelectedIndex != -1 ? (int?)cbMedida.SelectedValue : null,
                    IdTipoConector = cbTipoConector.SelectedIndex != -1 ? (int?)cbTipoConector.SelectedValue : null
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
                    var objetoFila = dgvArticulos.SelectedRows[0].DataBoundItem;
                    var articuloId = (int)objetoFila.GetType().GetProperty("ArticuloId").GetValue(objetoFila);
                    articuloIdAEliminar = articuloId;
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
                    // Obtener el artículo usando el objeto del DataBoundItem
                    var objetoFila = dgvArticulos.Rows[e.RowIndex].DataBoundItem;
                    var articuloId = (int)objetoFila.GetType().GetProperty("ArticuloId").GetValue(objetoFila);
                    var articulo = _articulosCompletos.FirstOrDefault(a => a.ArticuloId == articuloId);
                    
                    if (articulo != null)
                    {
                        articuloIdSeleccionado = articulo.ArticuloId;
                        txtArticulo.Text = articulo.Descripcion;
                        txtCodigo.Text = articulo.Codigo;
                        txtPrecioCompra.Text = articulo.PrecioCompra.ToString();
                        txtPrecioVenta.Text = articulo.PrecioVenta.ToString();
                        txtStock.Text = articulo.Stock.ToString();
                        txtStockMinimo.Text = articulo.StockMinimo.ToString();
                        
                        // Cargar valores en los ComboBox
                        if (articulo.IdMarca.HasValue)
                            cbMarca.SelectedValue = articulo.IdMarca.Value;
                        else
                            cbMarca.SelectedIndex = -1;
                            
                        if (articulo.IdMedida.HasValue)
                            cbMedida.SelectedValue = articulo.IdMedida.Value;
                        else
                            cbMedida.SelectedIndex = -1;
                            
                        if (articulo.IdTipoConector.HasValue)
                            cbTipoConector.SelectedValue = articulo.IdTipoConector.Value;
                        else
                            cbTipoConector.SelectedIndex = -1;
                        
                        // Establecer modo edición
                        EstablecerModoEdicion();
                    }
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
                    CargarDatos();
                }
                else
                {
                    // Filtrar por código o descripción
                    var articulosFiltrados = _articulosCompletos.Where(a =>
                        (a.Codigo != null && a.Codigo.ToLower().Contains(filtro)) ||
                        (a.Descripcion != null && a.Descripcion.ToLower().Contains(filtro))
                    ).ToList();

                    // Crear lista con datos extendidos para los artículos filtrados
                    var articulosParaGrilla = articulosFiltrados.Select(a => new
                    {
                        ArticuloId = a.ArticuloId,
                        Codigo = a.Codigo,
                        Descripcion = a.Descripcion,
                        NombreMarca = ObtenerNombreMarca(a.IdMarca),
                        NombreMedida = ObtenerNombreMedida(a.IdMedida),
                        NombreTipoConector = ObtenerNombreTipoConector(a.IdTipoConector),
                        PrecioCompra = a.PrecioCompra,
                        PrecioVenta = a.PrecioVenta,
                        Stock = a.Stock,
                        StockMinimo = a.StockMinimo
                    }).ToList();

                    dgvArticulos.DataSource = null;
                    dgvArticulos.DataSource = articulosParaGrilla;
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

        private void btnNuevaMarca_Click(object sender, EventArgs e)
        {
            frmGestionMarca frmGestionMarca = new frmGestionMarca(); 
            frmGestionMarca.ShowDialog();
            // Recargar ComboBox después de cerrar el formulario
            CargarComboBoxes();
        }

        private void btnNuevaMedida_Click(object sender, EventArgs e)
        {
            frmGestionMedida frmGestionMedida= new frmGestionMedida();
            frmGestionMedida.ShowDialog();
            // Recargar ComboBox después de cerrar el formulario
            CargarComboBoxes();
        }

        private void btnNuevoConector_Click(object sender, EventArgs e)
        {
            frmGestionTipoConector frmGestionTipoConector = new frmGestionTipoConector();
            frmGestionTipoConector.ShowDialog();
            // Recargar ComboBox después de cerrar el formulario
            CargarComboBoxes();
        }
    }
}
