using CEntidades.DTOs;
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
using VentaCredimax.Formularios;

namespace VentaCredimax
{
    public partial class frmSeleccionarCliente : Form
    {
        // Propiedad pública para almacenar el cliente seleccionado
        public ClienteDTO ClienteSeleccionado { get; private set; }

        GestorCliente gestorCliente = new GestorCliente();
        private frmVentas formularioVenta;
        public frmSeleccionarCliente(frmVentas frmPadre)
        {
            InitializeComponent();
            this.formularioVenta = frmPadre;
        }

        private void frmSeleccionarCliente_Load(object sender, EventArgs e)
        {
            cbBuscarPor.SelectedIndex = 0;
            ListarCliente();
            OcultarColumnas();
        }
        private void ListarCliente()
        {
            dgvClientes.DataSource = gestorCliente.Listar();
        }
        private void OcultarColumnas()
        {
            dgvClientes.Columns["ClientId"].Visible = false;
            dgvClientes.Columns["FechaAnulacion"].Visible = false;
        }
        private void FiltrarPorApellido()
        {
            dgvClientes.DataSource = gestorCliente.FiltrarPorApellido(txtBuscar.Text);
        }
        private void FiltrarPorNombre()
        {
            dgvClientes.DataSource = gestorCliente.FiltrarPorNombre(txtBuscar.Text);
        }
        private void FiltrarPorDNi()
        {
            dgvClientes.DataSource = gestorCliente.FiltrarPorDocumento((txtBuscar.Text));
        }
        private void Buscar()
        {
            string selectedValue = cbBuscarPor.SelectedItem.ToString();
            switch (selectedValue)
            {
                case "Apellido":
                    FiltrarPorApellido();
                    break;
                case "Nombre":
                    FiltrarPorNombre();
                    break;
                case "DNI":
                    FiltrarPorDNi();
                    break;
            }
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }
        private void SeleccionarCliente()
        {          
            // Verifica si se ha seleccionado una fila válida
            if (dgvClientes.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvClientes.SelectedRows[0];

                string nombreCliente = row.Cells["Nombre"].Value.ToString();
                string apellidoCliente = row.Cells["Apellido"].Value.ToString();
                int IdClienteSeleccionado = Convert.ToInt32(row.Cells["ClientId"].Value);
                int dni = Convert.ToInt32(row.Cells["DNI"].Value);

                formularioVenta.ClienteSeleccionado(nombreCliente, apellidoCliente, IdClienteSeleccionado,dni);
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un cliente.", "Selección de cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSeleccionarCliente_Click(object sender, EventArgs e)
        {
            SeleccionarCliente();
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            var frmCliente = new frmCliente();
            frmCliente.FormClosed += new FormClosedEventHandler(ActualizarClientes);
            frmCliente.ShowDialog();
        }

        private void ActualizarClientes(object sender, FormClosedEventArgs e)
        {
            // Este método se llama cuando el formulario ABM se cierra
            ListarCliente(); // Método que recarga el DataGridView con la lista actualizada de clientes
            
            // Notificar al formulario padre que se actualizaron los clientes
            if (formularioVenta != null)
            {
                formularioVenta.CargarComboCliente();
            }
        }

        private void dgvClientes_DoubleClick(object sender, EventArgs e)
        {
            SeleccionarCliente();
        }
    }
}
