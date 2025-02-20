using CEntidades;
using CEntidades.DTOs;
using CLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
        private void RegistrarVenta()
        {
            decimal precio;
            int cantidad;
            if (cbSeleccionCliente.Text != "" || txtArticulo.Text != "")
            {
                var venta = new Venta();

                venta.ClientId = (int)cbSeleccionCliente.SelectedValue;
                venta.Articulo = txtArticulo.Text;
                venta.Talle = string.IsNullOrEmpty(txtTalle.Text) ? (int?)null : Convert.ToInt32(txtTalle.Text);
                FormaDePago formaDePagoSeleccionada = (FormaDePago)cbFormaPago.SelectedItem;
                venta.FormaDePagoId = (int)formaDePagoSeleccionada;
                venta.Precio = Convert.ToDecimal(txtPrecio.Text);
                venta.Cuotas = Convert.ToInt32(txtCuotas.Text);
                venta.FechaDeInicio = DateTime.Now;
                venta.FechaDeCancelacion = dtpFechaCancelacion.Value;
                venta.Cantidad=Convert.ToInt32( txtCantidad.Text);
                if (decimal.TryParse(txtPrecio.Text, out precio) && int.TryParse(txtCantidad.Text, out cantidad))
                {
                    venta.Total = precio * cantidad;
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa valores válidos en los campos de precio y cantidad.");
                }
                venta.FechaAnulacion = null;

                _gestorVenta.RegistrarVenta(venta);
                MessageBox.Show("Operación Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.Clear();
            }
            {
                if (cbSeleccionCliente.Text == "")

                {
                    errorProvider1.SetError(cbSeleccionCliente, "Campo obligatorio");
                }
                if (txtArticulo.Text == "")
                {
                    errorProvider1.SetError(txtArticulo, "Campo obligatorio");
                }
            }
        }
        private void CargarFormaDePagoComboBox()
        {
            // Asignar los nombres de las opciones del enum al ComboBox
            cbFormaPago.DataSource = Enum.GetValues(typeof(FormaDePago));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RegistrarVenta();
            LimpiarCampos();
            ListarVentas();
        }
        private void LimpiarCampos()
        {
            cbFormaPago.SelectedIndexChanged -= cbFormaPago_SelectedIndexChanged;

            cbSeleccionCliente.SelectedIndex = -1;
            txtArticulo.Text = "";
            txtTalle.Text = "";
            cbFormaPago.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtCuotas.Text = "";
            dtpFechaCancelacion.Value = DateTime.Now;
            txtCantidad.Text = "";
            lblTotal.Text = "";

            cbFormaPago.SelectedIndexChanged += cbFormaPago_SelectedIndexChanged;
        }
        private void ListarVentas()
        {
            dgvVentas.DataSource = _gestorVenta.ListarVentasFormularioVentas(txtBuscar.Text);
            FormatoColumnasDataGrid();

        }
        private void OcultarColumnas()
        {
            if (dgvVentas.Rows.Count > 0)
            {
                dgvVentas.Columns["VentaId"].Visible = false;
                dgvVentas.Columns["IdCliente"].Visible = false;
                dgvVentas.Columns["FechaAnulacion"].Visible = false;
                dgvVentas.Columns["CuotasVencidas"].Visible = false;
            }
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
                txtArticulo.Text = dgvVentas.CurrentRow.Cells["Articulo"].Value?.ToString();
                txtTalle.Text = dgvVentas.CurrentRow.Cells["Talle"].Value?.ToString();
                cbFormaPago.Text = dgvVentas.CurrentRow.Cells["FormaDePago"].Value?.ToString();
                txtPrecio.Text = Convert.ToDecimal(dgvVentas.CurrentRow.Cells["Precio"].Value).ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                txtCuotas.Text = dgvVentas.CurrentRow.Cells["Cuotas"].Value?.ToString();
                txtCantidad.Text = dgvVentas.CurrentRow.Cells["Cantidad"].Value?.ToString();
                lblTotal.Text = Convert.ToDecimal(dgvVentas.CurrentRow.Cells["Total"].Value).ToString("N2", new System.Globalization.CultureInfo("es-AR"));
                dtpFechaCancelacion.Value = DateTime.Parse(dgvVentas.CurrentRow.Cells["FechaDeCancelacion"].Value?.ToString() ?? DateTime.Now.ToString());

                id = dgvVentas.CurrentRow.Cells["VentaId"].Value.ToString();
            }
            esModificacion = true;
        }
        private void ModificarVenta()
        {
            if (cbSeleccionCliente.Text != "" && txtArticulo.Text != "")
            {
                try
                {
                    var venta = new Venta();
                    venta.VentaId = Convert.ToInt32(id);
                    venta.ClientId = idCliente;
                    venta.Articulo = txtArticulo.Text;
                    venta.FormaDePagoId = (int)cbFormaPago.SelectedValue;
                    venta.Talle = string.IsNullOrEmpty(txtTalle.Text) ? (int?)null : Convert.ToInt32(txtTalle.Text);
                    venta.Precio = string.IsNullOrEmpty(txtPrecio.Text) ? (decimal?)null : Convert.ToDecimal(txtPrecio.Text);
                    venta.Cuotas = Convert.ToInt32(txtCuotas.Text);
                    venta.Cantidad = Convert.ToInt32(txtCantidad.Text);
                    venta.Total = Convert.ToDecimal(lblTotal.Text);
                    venta.FechaDeInicio = DateTime.Now;
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
                if (txtArticulo.Text == "")
                {
                    errorProvider1.SetError(txtArticulo, "Campo obligatorio");
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
                var preguntar = MessageBox.Show("¿Realmente desea eliminar el registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (preguntar == DialogResult.Yes)
                {
                    id = dgvVentas.CurrentRow.Cells["VentaId"].Value.ToString();
                    _gestorVenta.EliminarVenta(Convert.ToInt32(id));
                    MessageBox.Show("Operacion Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            dgvVentas.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
            dgvVentas.Columns["FormaDePago"].HeaderText = "Forma de pago";
            dgvVentas.Columns["FechaDeInicio"].HeaderText = "Fecha Venta";
            dgvVentas.Columns["FechaDeCancelacion"].HeaderText = "Cancelación pagos";
            dgvVentas.Columns["Precio"].DefaultCellStyle.Format = "N2";
            dgvVentas.Columns["Total"].DefaultCellStyle.Format = "N2";
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
            DateTime fechaVenta = DateTime.Now;

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
        private decimal CalcularTotal()
        {
            decimal precio = 0;
            int cantidad;
            decimal total = 0;
            if (decimal.TryParse(txtPrecio.Text, out precio) && int.TryParse(txtCantidad.Text, out cantidad))
            {
                total = precio * cantidad;
            }
            return total;
        }
        private void frmVentas_Load(object sender, EventArgs e)
        {
            CargarComboCliente();
            CargarFormaDePagoComboBox();
            ListarVentas();
            OcultarColumnas();
            FormatoColumnasDataGrid();
            btnEditarVenta.Enabled = false;
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if(txtPrecio.Text != "" && txtCantidad.Text != "")
            {
               lblTotal.Text = CalcularTotal().ToString("N2");
            }          
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            if (txtPrecio.Text != "" && txtCantidad.Text != "")
            {
                lblTotal.Text = CalcularTotal().ToString("N2");
            }
        }

        private void MostrarReporteComprobanteDeVenta(int ventaId)
        {
            if (dgvVentas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para imprimir el recibo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            try
            {
                // Crear formulario de reporte
                frmComprobanteDeVenta frmReporte = new frmComprobanteDeVenta(ventaId);
                //frmReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimirVenta_Click(object sender, EventArgs e)
        {
            int ventaId = Convert.ToInt32(dgvVentas.CurrentRow.Cells["VentaId"].Value);
            MostrarReporteComprobanteDeVenta(ventaId);
        }
    }
}
