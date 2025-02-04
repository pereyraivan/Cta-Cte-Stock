using CEntidades;
using CEntidades.DTOs;
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
        }

        private void frmControlPagos_Load(object sender, EventArgs e)
        {
            cbOrdenarPor.SelectedIndex = 0;
            ListarVentas();
            OcultarColumnas();
            EstiloDataGrid();
            PintarFilas();
        }
        private void ListarVentas()
        {
            string criterioSeleccionado = "";
            if (cbOrdenarPor.Items.Count > 0)
            {
                criterioSeleccionado = cbOrdenarPor.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un criterio de ordenación.");
            }
            dgvVentas.DataSource = _gestorVenta.ListarVentas(criterioSeleccionado);
            OcultarColumnas();
            EstiloDataGrid();
            PintarFilas();
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
            if(cbBuscarPor.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione un tipo de busqueda.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
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
                controlCuotas.FormClosed += (s, args) => ListarVentas();
                controlCuotas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila válida antes de hacer doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSeleccionarVenta_Click(object sender, EventArgs e)
        {
            // Validar que haya una fila seleccionada
            if (dgvVentas.CurrentRow != null)
            {
                // Obtener el ID de la venta desde la fila seleccionada
                int ventaId = Convert.ToInt32(dgvVentas.CurrentRow.Cells["VentaId"].Value);

                // Abrir el formulario frmControlCuotas pasando el ID de la venta
                frmControlCuotas controlCuotas = new frmControlCuotas(ventaId);
                controlCuotas.FormClosed += (s, args) => ListarVentas();
                controlCuotas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila válida antes de hacer doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void PintarFilas()
        {
            if (dgvVentas.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    var venta = row.DataBoundItem as VentaDTO;
                    if (venta != null && venta.CuotasVencidas)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                    else if (venta != null && venta.FechaAnulacion != null)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
            }
        }
        private void cbOrdenarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListarVentas();
        }
    }
}
