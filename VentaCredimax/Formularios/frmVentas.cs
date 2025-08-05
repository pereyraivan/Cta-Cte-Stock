using CEntidades;
using CEntidades.DTOs;
using CLogica;
using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmVentas : Form
    {
        private string id = null;
        private bool esModificacion;
        private int idCliente;
        GestorCliente _gestorCliente = new GestorCliente();
        GestorVenta _gestorVenta = new GestorVenta();
        GestorArticulo _gestorArticulo = new GestorArticulo();
        private GestorReportes _gestorReportes = new GestorReportes();
        public frmVentas()
        {
            InitializeComponent();

        }
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            frmSeleccionarCliente frmBusqueda = new frmSeleccionarCliente(this); // Pasa la referencia del formulario padre
            //frmBusqueda.FormClosed += FrmSeleccionCliente_FormClosed;
            frmBusqueda.ShowDialog();
        }
        public enum FormaDePago
        {
            Mensual = 1,
            Quincenal = 2,
            Semanal = 3
        }
        private void CargarComboCliente()
        {
            cbSeleccionCliente.DataSource = null; // Limpiar antes de asignar nuevos datos
            cbSeleccionCliente.DisplayMember = "NombreCompleto"; // Muestra Apellido, Nombre (DNI)
            cbSeleccionCliente.ValueMember = "IdCliente"; // Identificador real del cliente
            cbSeleccionCliente.DataSource = _gestorCliente.CargarComboCliente();
            // CargarVendedores(); // Obsoleto - Los vendedores ya no se usan
        }

        private void CargarComboArticulos()
        {
            cbArticulo.DataSource = null; // Limpiar antes de asignar nuevos datos
            cbArticulo.DisplayMember = "Descripcion"; // Muestra la descripción del artículo
            cbArticulo.ValueMember = "ArticuloId"; // Identificador real del artículo
            cbArticulo.DataSource = _gestorArticulo.Listar().Where(a => a.FechaAnulacion == null).ToList();
        }
        public void ClienteSeleccionado(string nombreCliente, string apellidoCliente, int idClienteSeleccionado, int dni)
        {
            // Formato: "Apellido, Nombre (DNI)"
            string clienteFormato = $"{apellidoCliente}, {nombreCliente} ({dni})";

            CargarComboCliente();

            // Buscar el cliente por su Id en el DataSource
            var clientes = (List<ClienteDTO>)cbSeleccionCliente.DataSource;

            // Encontrar el cliente en la lista del ComboBox
            var cliente = clientes.FirstOrDefault(c => c.IdCliente == idClienteSeleccionado);

            if (cliente != null)
            {
                // Selecciona el cliente en el ComboBox
                cbSeleccionCliente.SelectedValue = cliente.IdCliente;
            }
            else
            {
                // Si no está, lo agregamos al ComboBox
                clientes.Add(new ClienteDTO { IdCliente = idClienteSeleccionado, NombreCompleto = clienteFormato });
                cbSeleccionCliente.DataSource = null;
                cbSeleccionCliente.DataSource = clientes;
                cbSeleccionCliente.DisplayMember = "NombreCompleto";
                cbSeleccionCliente.ValueMember = "IdCliente";
                cbSeleccionCliente.SelectedValue = idClienteSeleccionado; // Seleccionamos el nuevo cliente
            }
        }

        public void ArticuloSeleccionado(string descripcionArticulo, int idArticuloSeleccionado, decimal precioVenta)
        {
            // Cargar los artículos en el ComboBox si no están cargados
            CargarComboArticulos();

            // Buscar el artículo por su Id en el DataSource
            var articulos = (List<Articulo>)cbArticulo.DataSource;

            // Encontrar el artículo en la lista del ComboBox
            var articulo = articulos.FirstOrDefault(a => a.ArticuloId == idArticuloSeleccionado);

            if (articulo != null)
            {
                // Selecciona el artículo en el ComboBox
                cbArticulo.SelectedValue = articulo.ArticuloId;
            }
            else
            {
                // Si no está en la lista, simplemente establecer el texto
                cbArticulo.Text = descripcionArticulo;
            }

            // Establecer automáticamente el precio de venta
            txtPrecio.Text = precioVenta.ToString("N2", new System.Globalization.CultureInfo("es-AR"));
        }

        private void RegistrarVenta()
        {
            decimal precio;
            int cantidad;

            if (cbSeleccionCliente.Text == "" || cbArticulo.Text == "" || txtPrecio.Text == "" || txtCantidad.Text == "" || txtCuotas.Text == "")
            {
                if (cbSeleccionCliente.Text == "")
                    errorProvider1.SetError(cbSeleccionCliente, "Campo obligatorio");
                if (cbArticulo.Text == "")
                    errorProvider1.SetError(cbArticulo, "Campo obligatorio");
                if (txtPrecio.Text == "")
                    errorProvider1.SetError(txtPrecio, "Campo obligatorio");
                if (txtCantidad.Text == "")
                    errorProvider1.SetError(txtCantidad, "Campo obligatorio");
                if (txtCuotas.Text == "")
                    errorProvider1.SetError(txtCuotas, "Campo obligatorio");

                return; // Si hay campos vacíos, no se ejecuta el resto del código
            }

            var venta = new Venta();

            venta.ClientId = (int)cbSeleccionCliente.SelectedValue;
            // Buscar el ID del artículo por su descripción
            if (cbArticulo.SelectedValue != null)
            {
                venta.IdArticulo = (int)cbArticulo.SelectedValue;
            }
            else
            {
                // Si no se seleccionó de la lista, buscar por texto
                var gestorArticulo = new GestorArticulo();
                var articulosBusqueda = gestorArticulo.BuscarPorNombre(cbArticulo.Text);
                if (articulosBusqueda.Any())
                {
                    venta.IdArticulo = articulosBusqueda.First().ArticuloId;
                }
                else
                {
                    MessageBox.Show("No se encontró el artículo especificado.");
                    return;
                }
            }
            FormaDePago formaDePagoSeleccionada = (FormaDePago)cbFormaPago.SelectedItem;
            venta.FormaDePagoId = (int)formaDePagoSeleccionada;

            venta.Precio = Convert.ToDecimal(txtPrecio.Text);
            venta.Cuotas = Convert.ToInt32(txtCuotas.Text);
            venta.FechaDeInicio = dtfechaVenta.Value;
            venta.FechaDeCancelacion = dtpFechaCancelacion.Value;
            venta.Cantidad = Convert.ToInt32(txtCantidad.Text);
            
            // Procesar el anticipo
            decimal anticipo = 0;
            if (!string.IsNullOrEmpty(txtAnticipo.Text))
            {
                if (decimal.TryParse(txtAnticipo.Text, out anticipo))
                {
                    venta.Anticipo = anticipo;
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un valor válido para el anticipo.");
                    return;
                }
            }
            else
            {
                venta.Anticipo = 0; // Si no hay anticipo, asignar 0
            }
            
            if (decimal.TryParse(txtPrecio.Text, out precio) && int.TryParse(txtCantidad.Text, out cantidad))
            {
                decimal subtotal = precio * cantidad;
                venta.Subtotal = subtotal; // Ahora el campo existe en la tabla
                
                // Validar que el anticipo no sea mayor al subtotal
                if (anticipo > subtotal)
                {
                    MessageBox.Show($"El anticipo no puede ser mayor al subtotal de la venta. Subtotal: {subtotal:C2}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Calcular el total como Subtotal - Anticipo
                decimal total = subtotal - anticipo;
                venta.Total = total;
            }
            else
            {
                MessageBox.Show("Por favor, ingresa valores válidos en los campos de precio y cantidad.");
                return;
            }
            venta.FechaAnulacion = null;

            // Control de stock - verificar disponibilidad antes de registrar la venta
            var articulos = _gestorArticulo.Listar();
            var articulo = articulos.FirstOrDefault(a => a.ArticuloId == venta.IdArticulo);
            
            if (articulo == null)
            {
                MessageBox.Show("El artículo seleccionado no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificar stock disponible
            if (articulo.Stock < venta.Cantidad)
            {
                MessageBox.Show($"Stock insuficiente. Stock disponible: {articulo.Stock}, Cantidad solicitada: {venta.Cantidad}", 
                               "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Registrar la venta
            _gestorVenta.RegistrarVenta(venta);

            // Actualizar stock - disminuir la cantidad vendida
            try
            {
                _gestorArticulo.ActualizarStock(venta.IdArticulo, (int)venta.Cantidad, "Salida");
                int nuevoStock = articulo.Stock - (int)venta.Cantidad;
                MessageBox.Show($"Venta registrada exitosamente. Stock actualizado: {nuevoStock}", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Venta registrada, pero error al actualizar stock: {ex.Message}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            LimpiarCampos();
            ListarVentas();
            errorProvider1.Clear();
        }
        private void CargarFormaDePagoComboBox()
        {
            // Asignar los nombres de las opciones del enum al ComboBox
            cbFormaPago.DataSource = Enum.GetValues(typeof(FormaDePago));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var esdemo = _gestorVenta.EsDemo();
            if (esdemo)
            {
               var contarVentas = dgvVentas.Rows.Count;
                if(contarVentas > 5)
                {
                    MessageBox.Show("Está utilizando una versión de prueba. Adquiera la versión completa para disfrutar de todas las funciones sin restricciones.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            RegistrarVenta();
            ListarVentas();
        }
        private void LimpiarCampos()
        {
            cbFormaPago.SelectedIndexChanged -= cbFormaPago_SelectedIndexChanged;
            cbArticulo.SelectedIndexChanged -= cbArticulo_SelectedIndexChanged;

            cbSeleccionCliente.SelectedIndex = -1;
            cbArticulo.Text = "";
            cbFormaPago.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtCuotas.Text = "";
            txtAnticipo.Text = "";
            dtpFechaCancelacion.Value = DateTime.Now;
            dtfechaVenta.Value = DateTime.Now;
            txtCantidad.Text = "";
            lblTotal.Text = "";

            cbFormaPago.SelectedIndexChanged += cbFormaPago_SelectedIndexChanged;
            cbArticulo.SelectedIndexChanged += cbArticulo_SelectedIndexChanged;
        }
        private void ListarVentas()
        {
            dgvVentas.DataSource = _gestorVenta.ListarVentasFormularioVentas(txtBuscar.Text);
            OcultarColumnas();
            FormatoColumnasDataGrid();
        }
        private void OcultarColumnas()
        {
            // Verificar si las columnas existen antes de intentar ocultarlas
            if (dgvVentas.Columns.Contains("VentaId"))
                dgvVentas.Columns["VentaId"].Visible = false;
            if (dgvVentas.Columns.Contains("IdCliente"))
                dgvVentas.Columns["IdCliente"].Visible = false;
            if (dgvVentas.Columns.Contains("IdArticulo"))
                dgvVentas.Columns["IdArticulo"].Visible = false;
            if (dgvVentas.Columns.Contains("FechaAnulacion"))
                dgvVentas.Columns["FechaAnulacion"].Visible = false;
            if (dgvVentas.Columns.Contains("CuotasVencidas"))
                dgvVentas.Columns["CuotasVencidas"].Visible = false;
            // dgvVentas.Columns["IdDiaSemana"].Visible = false; // Obsoleto - ya no existe
        }
        private void dgvVentas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVentas.SelectedRows.Count > 0)
            {
                btnEditarVenta.Enabled = true;
                button1.Enabled = false;

                // Obtener el ID del cliente de la venta seleccionada en el DataGridView
                idCliente = (int)dgvVentas.CurrentRow.Cells["IdCliente"].Value;

                // Asignar el valor al ComboBox
                cbSeleccionCliente.SelectedValue = idCliente;
                cbSeleccionCliente.Text = dgvVentas.CurrentRow.Cells["NombreCliente"].Value?.ToString();
                // Seleccionar el artículo por su ID
                int idArticulo = (int)dgvVentas.CurrentRow.Cells["IdArticulo"].Value;
                cbArticulo.SelectedValue = idArticulo;
                // También establecer el texto del artículo para que sea visible
                cbArticulo.Text = dgvVentas.CurrentRow.Cells["Articulo"].Value?.ToString();
                cbFormaPago.Text = dgvVentas.CurrentRow.Cells["FormaDePago"].Value?.ToString();
                txtPrecio.Text = Convert.ToDecimal(dgvVentas.CurrentRow.Cells["Precio"].Value).ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                txtCuotas.Text = dgvVentas.CurrentRow.Cells["Cuotas"].Value?.ToString();
                txtCantidad.Text = dgvVentas.CurrentRow.Cells["Cantidad"].Value?.ToString();
                
                // Cargar el anticipo si existe
                decimal anticipo = 0;
                if (dgvVentas.Columns.Contains("Anticipo") && dgvVentas.CurrentRow.Cells["Anticipo"].Value != null)
                {
                    anticipo = Convert.ToDecimal(dgvVentas.CurrentRow.Cells["Anticipo"].Value);
                    txtAnticipo.Text = anticipo.ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                }
                else
                {
                    txtAnticipo.Text = "0,00";
                }
                
                // El Total del grid ya es Subtotal - Anticipo, mostrar directamente
                decimal totalFinal = Convert.ToDecimal(dgvVentas.CurrentRow.Cells["Total"].Value);
                lblTotal.Text = totalFinal.ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                dtpFechaCancelacion.Value = DateTime.Parse(dgvVentas.CurrentRow.Cells["FechaDeCancelacion"].Value?.ToString() ?? DateTime.Now.ToString());
                dtfechaVenta.Value = DateTime.Parse(dgvVentas.CurrentRow.Cells["FechaDeInicio"].Value?.ToString() ?? DateTime.Now.ToString());

                id = dgvVentas.CurrentRow.Cells["VentaId"].Value.ToString();
            }
            esModificacion = true;
        }
        private void ModificarVenta()
        {
            if (cbSeleccionCliente.Text != "" && cbArticulo.Text != "")
            {
                try
                {
                    var venta = new Venta();
                    venta.VentaId = Convert.ToInt32(id);
                    venta.ClientId = (int)cbSeleccionCliente.SelectedValue; // Tomar el cliente seleccionado actualmente en el ComboBox
                    // Buscar el ID del artículo por su descripción
                    if (cbArticulo.SelectedValue != null)
                    {
                        venta.IdArticulo = (int)cbArticulo.SelectedValue;
                    }
                    else
                    {
                        // Si no se seleccionó de la lista, buscar por texto
                        var gestorArticulo = new GestorArticulo();
                        var articulosModificacion = gestorArticulo.BuscarPorNombre(cbArticulo.Text);
                        if (articulosModificacion.Any())
                        {
                            venta.IdArticulo = articulosModificacion.First().ArticuloId;
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el artículo especificado.");
                            return;
                        }
                    }
                    venta.FormaDePagoId = (int)cbFormaPago.SelectedValue;
                    
                    venta.Precio = string.IsNullOrEmpty(txtPrecio.Text) ? (decimal?)null : Convert.ToDecimal(txtPrecio.Text);
                    venta.Cuotas = Convert.ToInt32(txtCuotas.Text);
                    venta.Cantidad = Convert.ToInt32(txtCantidad.Text);
                    
                    // Procesar el anticipo en la modificación
                    decimal anticipo = 0;
                    if (!string.IsNullOrEmpty(txtAnticipo.Text))
                    {
                        if (decimal.TryParse(txtAnticipo.Text, out anticipo))
                        {
                            venta.Anticipo = anticipo;
                        }
                        else
                        {
                            MessageBox.Show("Por favor, ingresa un valor válido para el anticipo.");
                            return;
                        }
                    }
                    else
                    {
                        venta.Anticipo = 0;
                    }
                    
                    // Calcular subtotal y total
                    decimal precio = Convert.ToDecimal(txtPrecio.Text);
                    int cantidad = Convert.ToInt32(txtCantidad.Text);
                    decimal subtotal = precio * cantidad;
                    venta.Subtotal = subtotal; // Ahora el campo existe en la tabla
                    
                    // Validar que el anticipo no sea mayor al subtotal
                    if (anticipo > subtotal)
                    {
                        MessageBox.Show($"El anticipo no puede ser mayor al subtotal de la venta. Subtotal: {subtotal:C2}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Calcular el total como Subtotal - Anticipo
                    decimal total = subtotal - anticipo;
                    venta.Total = total;
                    
                    venta.FechaDeInicio = dtfechaVenta.Value;
                    venta.FechaDeCancelacion = dtpFechaCancelacion.Value;
                    venta.FechaAnulacion = null;


                    _gestorVenta.ModificarVenta(venta);
                    MessageBox.Show("Operación Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    errorProvider1.Clear();
                    btnEditarVenta.Enabled = false;
                    button1.Enabled = true;

                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                if (cbSeleccionCliente.Text == "")
                {
                    errorProvider1.SetError(cbSeleccionCliente, "Campo obligatorio");
                }
                if (cbArticulo.Text == "")
                {
                    errorProvider1.SetError(cbArticulo, "Campo obligatorio");
                }
            }
        }
        private void btnEditarVenta_Click(object sender, EventArgs e)
        {
            ModificarVenta();
            LimpiarCampos();
            ListarVentas();
        }
        private void EliminarVenta()
        {
            if (dgvVentas.SelectedRows.Count > 0)
            {
                var preguntar = MessageBox.Show("Al eliminar esta venta, se cancelarán todas las cuotas pendientes asociadas y se restaurará el stock. ¿Desea continuar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (preguntar == DialogResult.Yes)
                {
                    id = dgvVentas.CurrentRow.Cells["VentaId"].Value.ToString();
                    
                    // Obtener información de la venta antes de eliminarla para restaurar el stock
                    int idArticulo = (int)dgvVentas.CurrentRow.Cells["IdArticulo"].Value;
                    int cantidadVendida = (int)dgvVentas.CurrentRow.Cells["Cantidad"].Value;
                    
                    // Obtener el artículo para actualizar su stock
                    var articulos = _gestorArticulo.Listar();
                    var articulo = articulos.FirstOrDefault(a => a.ArticuloId == idArticulo);
                    
                    if (articulo != null)
                    {
                        // Eliminar la venta
                        _gestorVenta.EliminarVenta(Convert.ToInt32(id));
                        
                        // Restaurar stock - sumar la cantidad que se había vendido
                        try
                        {
                            _gestorArticulo.ActualizarStock(idArticulo, cantidadVendida, "Entrada");
                            int stockRestaurado = articulo.Stock + cantidadVendida;
                            MessageBox.Show($"Venta eliminada exitosamente. Stock restaurado: {stockRestaurado}", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Venta eliminada, pero error al restaurar stock: {ex.Message}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // Si no encontramos el artículo, igual eliminamos la venta
                        _gestorVenta.EliminarVenta(Convert.ToInt32(id));
                        MessageBox.Show("Venta eliminada exitosamente", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
        }
        private void btnAnularVenta_Click(object sender, EventArgs e)
        {
            EliminarVenta();
            ListarVentas();
            LimpiarCampos();
        }
        private void FrmSeleccionCliente_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Recargar el ComboBox con los clientes actualizados
            CargarComboCliente();
        }
        private void FormatoColumnasDataGrid()
        {
            // Verificar si las columnas existen antes de formatearlas
            if (dgvVentas.Columns.Contains("NombreCliente"))
                dgvVentas.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
            if (dgvVentas.Columns.Contains("Articulo"))
                dgvVentas.Columns["Articulo"].HeaderText = "Artículo";
            if (dgvVentas.Columns.Contains("FormaDePago"))
                dgvVentas.Columns["FormaDePago"].HeaderText = "Forma de pago";
            if (dgvVentas.Columns.Contains("FechaDeInicio"))
                dgvVentas.Columns["FechaDeInicio"].HeaderText = "Fecha Venta";
            if (dgvVentas.Columns.Contains("FechaDeCancelacion"))
                dgvVentas.Columns["FechaDeCancelacion"].HeaderText = "Cancelación pagos";
            // dgvVentas.Columns["VendedorNombre"].HeaderText = "Vendedor"; // Obsoleto
            // dgvVentas.Columns["DiaSemanaNombre"].HeaderText = "Dia Pago"; // Obsoleto
            if (dgvVentas.Columns.Contains("Precio"))
                dgvVentas.Columns["Precio"].DefaultCellStyle.Format = "N2";
            if (dgvVentas.Columns.Contains("Subtotal"))
            {
                dgvVentas.Columns["Subtotal"].HeaderText = "Subtotal";
                dgvVentas.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
            }
            if (dgvVentas.Columns.Contains("Anticipo"))
            {
                dgvVentas.Columns["Anticipo"].HeaderText = "Anticipo";
                dgvVentas.Columns["Anticipo"].DefaultCellStyle.Format = "N2";
            }
            if (dgvVentas.Columns.Contains("Total"))
                dgvVentas.Columns["Total"].DefaultCellStyle.Format = "N2";
            
            // Configurar para que las columnas ocupen todo el ancho disponible
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Configurar ancho mínimo para la columna del artículo para que se lea completo
            if (dgvVentas.Columns.Contains("Articulo"))
            {
                dgvVentas.Columns["Articulo"].MinimumWidth = 200;
                dgvVentas.Columns["Articulo"].FillWeight = 150; // Dar más peso a esta columna
            }
            
            // Ajustar el peso de otras columnas para que la columna Artículo tenga más espacio
            if (dgvVentas.Columns.Contains("NombreCliente"))
                dgvVentas.Columns["NombreCliente"].FillWeight = 120;
            if (dgvVentas.Columns.Contains("FormaDePago"))
                dgvVentas.Columns["FormaDePago"].FillWeight = 80;
            if (dgvVentas.Columns.Contains("Precio"))
                dgvVentas.Columns["Precio"].FillWeight = 70;
            if (dgvVentas.Columns.Contains("Subtotal"))
                dgvVentas.Columns["Subtotal"].FillWeight = 75;
            if (dgvVentas.Columns.Contains("Anticipo"))
                dgvVentas.Columns["Anticipo"].FillWeight = 70;
            if (dgvVentas.Columns.Contains("Total"))
                dgvVentas.Columns["Total"].FillWeight = 70;
            if (dgvVentas.Columns.Contains("Cuotas"))
                dgvVentas.Columns["Cuotas"].FillWeight = 60;
            if (dgvVentas.Columns.Contains("Cantidad"))
                dgvVentas.Columns["Cantidad"].FillWeight = 60;
            
            // Configurar el estilo del encabezado - color Azure
            dgvVentas.EnableHeadersVisualStyles = false;
            dgvVentas.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Azure;
            dgvVentas.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvVentas.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            dgvVentas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            frmCalcularCuotas frmCalcularCuotas = new frmCalcularCuotas();
            frmCalcularCuotas.Show();
        }
        private void CalcularFechaFinalizacionPago()
        {
            if (cbFormaPago.SelectedItem == null || string.IsNullOrWhiteSpace(cbFormaPago.SelectedItem.ToString()))
            {
                return;  // No hacer nada si el ComboBox está vacío
            }
            if (string.IsNullOrWhiteSpace(txtCuotas.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                return;
            }
            // Obtener la fecha de venta
            DateTime fechaVenta = dtfechaVenta.Value;

            // Obtener la cantidad de cuotas
            int cantidadCuotas;
            if (!int.TryParse(txtCuotas.Text, out cantidadCuotas))
            {
                MessageBox.Show("Introduce una cantidad de cuotas válida.");
                return;
            }
            if (cbFormaPago.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una frecuencia de pago válida.");
                return;
            }

            // Obtener la frecuencia de pago seleccionada
            string frecuenciaPago = cbFormaPago.SelectedItem.ToString();

            // Calcular la cantidad de días a sumar por cada cuota según la frecuencia de pago
            int diasPorCuota = 0;

            switch (frecuenciaPago)
            {
                case "Mensual":
                    diasPorCuota = 30;  // Aproximadamente 1 mes
                    break;
                case "Quincenal":
                    diasPorCuota = 15;  // 15 días por quincena
                    break;
                case "Semanal":
                    diasPorCuota = 7;   // 7 días por semana
                    break;
                default:
                    MessageBox.Show("Selecciona una frecuencia de pago válida.");
                    return;
            }

            // Calcular la fecha de finalización del pago
            DateTime fechaFinalizacionPago = fechaVenta.AddDays(diasPorCuota * cantidadCuotas);

            dtpFechaCancelacion.Value = fechaFinalizacionPago;

        }
        private void txtCuotas_Leave(object sender, EventArgs e)
        {
            CalcularFechaFinalizacionPago();
        }
        private void CancelarVenta()
        {
            LimpiarCampos();
            btnEditarVenta.Enabled = false;
            button1.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            CancelarVenta();
        }
        private void FiltrarVentasPorCliente()
        {
            dgvVentas.DataSource = _gestorVenta.FiltrarVentasPorCliente(txtBuscar.Text);
        }
        private void FiltrarVentasPorArticulo()
        {
            dgvVentas.DataSource = _gestorVenta.FiltrarVentasPorArticulo(txtBuscar.Text);
        }
        private void Buscar()
        {
            // Si no hay texto de búsqueda, mostrar todas las ventas
            if (string.IsNullOrEmpty(txtBuscar.Text))
            {
                ListarVentas();
                return;
            }

            if (cbBuscarPor.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un criterio de búsqueda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedValue = cbBuscarPor.SelectedItem.ToString();
            switch (selectedValue)
            {
                case "Cliente":
                    FiltrarVentasPorCliente();
                    break;
                case "Articulo":
                    FiltrarVentasPorArticulo();
                    break;
            }
            // Aplicar formato después de cualquier búsqueda
            OcultarColumnas();
            FormatoColumnasDataGrid();
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }
        private void cbFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtCuotas.Text != "" && cbFormaPago.SelectedItem != null)
            {
                CalcularFechaFinalizacionPago();
            }
            esModificacion = false;
        }
        private decimal CalcularSubtotal()
        {
            decimal precio = 0;
            int cantidad;
            decimal subtotal = 0;
            if (decimal.TryParse(txtPrecio.Text, out precio) && int.TryParse(txtCantidad.Text, out cantidad))
            {
                subtotal = precio * cantidad;
            }
            return subtotal;
        }
        
        private void ActualizarTotalConAnticipo()
        {
            decimal subtotal = CalcularSubtotal();
            decimal anticipo = 0;
            
            if (!string.IsNullOrEmpty(txtAnticipo.Text))
            {
                decimal.TryParse(txtAnticipo.Text, out anticipo);
            }
            
            // Validar que el anticipo no sea mayor al subtotal
            if (anticipo > subtotal && subtotal > 0)
            {
                MessageBox.Show($"El anticipo no puede ser mayor al subtotal de la venta. Subtotal: {subtotal:C2}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnticipo.Focus();
                return;
            }
            
            // Mostrar el total final (Subtotal - Anticipo)
            decimal totalFinal = subtotal - anticipo;
            lblTotal.Text = totalFinal.ToString("N2");
        }
        private void frmVentas_Load(object sender, EventArgs e)
        {
            // CargarComboDiasSemana(); // Obsoleto - Los días de semana ya no se usan
            CargarComboCliente();
            CargarComboArticulos();
            CargarFormaDePagoComboBox();
            ListarVentas();
            btnEditarVenta.Enabled = false;
            
            // Suscribir eventos
            cbArticulo.SelectedIndexChanged += cbArticulo_SelectedIndexChanged;
            txtAnticipo.Leave += txtAnticipo_Leave;
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if (txtPrecio.Text != "" && txtCantidad.Text != "")
            {
                ActualizarTotalConAnticipo();
            }
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            if (txtPrecio.Text != "" && txtCantidad.Text != "")
            {
                ActualizarTotalConAnticipo();
            }
        }

        private void txtAnticipo_Leave(object sender, EventArgs e)
        {
            ActualizarTotalConAnticipo();
        }

        private void ImprimirReporteComprobanteDeVenta(int ventaId)
        {
            
            List<ComprobanteDePago_Result> listaComprobanteDeVenta = _gestorReportes.DatosComprobanteDeVenta(ventaId);

            if (listaComprobanteDeVenta.Any())
            {
                // Cargar la plantilla HTML
                string textoHtml = Properties.Resources.ComprobanteDeVenta.ToString();

                textoHtml = textoHtml.Replace("@nombre", listaComprobanteDeVenta.FirstOrDefault().Nombre);
                textoHtml = textoHtml.Replace("@apellido", listaComprobanteDeVenta.FirstOrDefault().Apellido);
                textoHtml = textoHtml.Replace("@fecha", listaComprobanteDeVenta.FirstOrDefault().FechaDeInicio.ToString("dd/MM/yyyy"));
                textoHtml = textoHtml.Replace("@dni", listaComprobanteDeVenta.FirstOrDefault().DNI.ToString());
                textoHtml = textoHtml.Replace("@telefono", listaComprobanteDeVenta.FirstOrDefault().Telefono);
                textoHtml = textoHtml.Replace("@direccion", listaComprobanteDeVenta.FirstOrDefault().Direccion);

                //tabla
                textoHtml = textoHtml.Replace("@cantidad", listaComprobanteDeVenta.FirstOrDefault().Cantidad.ToString());
                textoHtml = textoHtml.Replace("@articulo", listaComprobanteDeVenta.FirstOrDefault().Articulo);
                textoHtml = textoHtml.Replace("@marca", listaComprobanteDeVenta.FirstOrDefault().Marca ?? "");
                textoHtml = textoHtml.Replace("@medida", listaComprobanteDeVenta.FirstOrDefault().Medida ?? "");
                textoHtml = textoHtml.Replace("@tipoconector", listaComprobanteDeVenta.FirstOrDefault().TipoConector ?? "");

                decimal precio = listaComprobanteDeVenta.FirstOrDefault().Precio.Value;
                string montoPrecioFormateado = string.Format("{0:#,##0.00}", precio).Replace(",", "X").Replace(".", ",").Replace("X", ".");
                textoHtml = textoHtml.Replace("@precio", montoPrecioFormateado);

                // Manejar Subtotal - si es null, calcularlo como precio * cantidad
                decimal subtotal = listaComprobanteDeVenta.FirstOrDefault().Subtotal ?? 
                                  (precio * (listaComprobanteDeVenta.FirstOrDefault().Cantidad ?? 1));
                string subtotalFormateado = string.Format("{0:#,##0.00}", subtotal).Replace(",", "X").Replace(".", ",").Replace("X", ".");
                textoHtml = textoHtml.Replace("@subtotal", subtotalFormateado);

                decimal anticipo = listaComprobanteDeVenta.FirstOrDefault().Anticipo ?? 0;
                string anticipoFormateado = string.Format("{0:#,##0.00}", anticipo).Replace(",", "X").Replace(".", ",").Replace("X", ".");
                textoHtml = textoHtml.Replace("@anticipo", anticipoFormateado);

                // Manejar Total - si es null, calcularlo como subtotal - anticipo
                decimal total = listaComprobanteDeVenta.FirstOrDefault().Total ?? (subtotal - anticipo);
                string totalFormateado = string.Format("{0:#,##0.00}", total).Replace(",", "X").Replace(".", ",").Replace("X", ".");
                textoHtml = textoHtml.Replace("@total", totalFormateado);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = $"comprobante-venta-{listaComprobanteDeVenta.FirstOrDefault().Nombre +'-'+ listaComprobanteDeVenta.FirstOrDefault().Apellido +'-'+ listaComprobanteDeVenta.FirstOrDefault().Articulo}.pdf";
                saveFile.Filter = "Pdf Files|*.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // Crear el conversor de HTML a PDF
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

                    // Convertir el HTML a PDF
                    SelectPdf.PdfDocument doc = converter.ConvertHtmlString(textoHtml);

                    // Guardar el PDF
                    doc.Save(saveFile.FileName);
                    doc.Close();

                    MessageBox.Show("Documento Generado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void btnImprimirVenta_Click(object sender, EventArgs e)
        {
            if (dgvVentas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para imprimir el recibo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int ventaId = Convert.ToInt32(dgvVentas.CurrentRow.Cells["VentaId"].Value);
            ImprimirReporteComprobanteDeVenta(ventaId);
        }

        private void btnBuscarArticulo_Click(object sender, EventArgs e)
        {
            frmListaArticulos seleccionarArticulo = new frmListaArticulos(this); // Pasa la referencia del formulario padre
            
            if (seleccionarArticulo.ShowDialog() == DialogResult.OK)
            {
                // Si se seleccionó un artículo, obtenerlo y configurar el ComboBox
                var articuloSeleccionado = seleccionarArticulo.ArticuloSeleccionado;
                if (articuloSeleccionado != null)
                {
                    ArticuloSeleccionado(articuloSeleccionado.Descripcion, articuloSeleccionado.ArticuloId, articuloSeleccionado.PrecioVenta);
                }
            }
        }

        private void cbArticulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Solo procesar si hay un artículo seleccionado y no es durante la carga
            if (cbArticulo.SelectedValue != null && cbArticulo.DataSource != null)
            {
                try
                {
                    int articuloId = (int)cbArticulo.SelectedValue;
                    
                    // Buscar el artículo en el DataSource para obtener su precio
                    var articulos = (List<Articulo>)cbArticulo.DataSource;
                    var articuloSeleccionado = articulos.FirstOrDefault(a => a.ArticuloId == articuloId);
                    
                    if (articuloSeleccionado != null)
                    {
                        // Establecer automáticamente el precio de venta
                        txtPrecio.Text = articuloSeleccionado.PrecioVenta.ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                    }
                }
                catch
                {
                    // Si hay algún error, no hacer nada para evitar excepciones durante la carga
                }
            }
        }
    }
}
