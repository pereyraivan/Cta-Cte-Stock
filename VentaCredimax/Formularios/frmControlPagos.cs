using CLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmControlPagos : Form
    {
        private GestorVenta _gestorVenta = new GestorVenta();
        public frmControlPagos()
        {
            InitializeComponent();
            ListarVentas();
            OcultarColumnas();
            EstiloDataGrid();
        }

        private void frmControlPagos_Load(object sender, EventArgs e)
        {

        }
        private void ListarVentas()
        {
            dgvVentas.DataSource = _gestorVenta.ListarVentas();
            OcultarColumnas();
        }
        private void FiltrarVentasPorCliente()
        {
            dgvVentas.DataSource = _gestorVenta.FiltrarVentasPorCliente(txtBuscar.Text);
            OcultarColumnas();
        }
        private void FiltrarVentasPorArticulo()
        {
            dgvVentas.DataSource = _gestorVenta.FiltrarVentasPorArticulo(txtBuscar.Text);
            OcultarColumnas();
        }
        private void Buscar()
        {
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
        private void EstiloDataGrid()
        {
            if (dgvVentas.Rows.Count > 0)
            {
                dgvVentas.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
                dgvVentas.Columns["FormaDePago"].HeaderText = "Frecuencia de pago";
                dgvVentas.Columns["FechaDeInicio"].HeaderText = "Fecha compra";
                dgvVentas.Columns["FechaDeCancelacion"].HeaderText = "Cancelacion compra";
            }
        }

        private void dgvVentas_DoubleClick(object sender, EventArgs e)
        {
            // Validar que haya una fila seleccionada
            if (dgvVentas.CurrentRow != null)
            {
                // Obtener el ID de la venta desde la fila seleccionada
                int ventaId = Convert.ToInt32(dgvVentas.CurrentRow.Cells["VentaId"].Value);

                // Abrir el formulario frmControlCuotas pasando el ID de la venta
                frmControlCuotas controlCuotas = new frmControlCuotas(ventaId);
                controlCuotas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila válida antes de hacer doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
