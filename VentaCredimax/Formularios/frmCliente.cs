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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VentaCredimax.Formularios
{
    public partial class frmCliente : Form
    {
        private GestorCliente gestorCliente = new GestorCliente();
        private string id = null;
        public frmCliente()
        {
            InitializeComponent();
        }
        private void frmCliente_Load_1(object sender, EventArgs e)
        {
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            Listar();
            OcultarColumnas();
            if (dgvClientes.Rows.Count > 0)
            {
                dgvClientes.SelectedRows[0].Selected = false;
            }
        }
        private void Guardar()
        {
            // Limpiar errores previos
            errorProvider1.Clear();

            bool esValido = true;

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Campo obligatorio");
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                errorProvider1.SetError(txtApellido, "Campo obligatorio");
                esValido = false;
            }         

            // Validar que el DNI sea un número válido
            if (!int.TryParse(txtDni.Text, out int dni))
            {
                errorProvider1.SetError(txtDni, "Debe ingresar un número válido");
                esValido = false;
            }

            if (!esValido)
            {
                return; // Salir si hay errores
            }

            // Validar si el cliente ya existe
            if (gestorCliente.ValidarExistencia(txtNombre.Text, txtApellido.Text, dni).Any())
            {
                MessageBox.Show("El registro ya existe", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear y guardar cliente
            var cliente = new Cliente
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                DNI = dni,
                Direccion = txtDireccion.Text,
                Telefono = txtTelefono.Text,
                FechaAnulacion = null
            };

            gestorCliente.Guardar(cliente);
            MessageBox.Show("Operación Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Actualizar lista y limpiar campos
            Listar();
            Limpiar();
        }
        
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        private void Listar()
        {
            dgvClientes.DataSource = gestorCliente.Listar();
        }
        private void Limpiar()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
        }
        private void Modificar()
        {
            if (txtNombre.Text != "" && txtApellido.Text != "")
            {
                var cliente = new Cliente();
                cliente.ClientId = Convert.ToInt32(id);
                cliente.Nombre = txtNombre.Text;
                cliente.Apellido = txtApellido.Text;
                cliente.DNI = Convert.ToInt32(txtDni.Text);
                cliente.Direccion = txtDireccion.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.FechaAnulacion = null;

                gestorCliente.Modificar(cliente);
                MessageBox.Show("Operación Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorProvider1.Clear();
                btnEditar.Enabled = false;
                btnGuardar.Enabled = true;
            }
            else
            {
                if (txtNombre.Text == "")
                {
                    errorProvider1.SetError(txtNombre, "Campo obligatorio");
                }
                if (txtApellido.Text == "")
                {
                    errorProvider1.SetError(txtApellido, "Campo obligatorio");
                }
            }
        }
        private void dgvClientes_DoubleClick(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                btnEditar.Enabled = true;
                btnGuardar.Enabled = false;
                txtNombre.Text = dgvClientes.CurrentRow.Cells["Nombre"].Value?.ToString();
                txtApellido.Text = dgvClientes.CurrentRow.Cells["Apellido"].Value?.ToString();
                txtDni.Text = dgvClientes.CurrentRow.Cells["DNI"].Value?.ToString();
                txtDireccion.Text = dgvClientes.CurrentRow.Cells["Direccion"].Value?.ToString();
                txtTelefono.Text = dgvClientes.CurrentRow.Cells["Telefono"].Value?.ToString();

                id = dgvClientes.CurrentRow.Cells["ClientId"].Value.ToString();
            }
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Modificar();
            Listar();
            Limpiar();
        }
        private void OcultarColumnas()
        {
            dgvClientes.Columns["ClientId"].Visible = false;
            dgvClientes.Columns["FechaAnulacion"].Visible = false;
        }
        private void Eliminar()
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                var preguntar = MessageBox.Show("¿Realmente desea eliminar el registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (preguntar == DialogResult.Yes)
                {
                    id = dgvClientes.CurrentRow.Cells["ClientId"].Value.ToString();
                    gestorCliente.Eliminar(Convert.ToInt32(id));
                    MessageBox.Show("Operacion Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
            Listar();
            Limpiar();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Eliminar();
        }
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEliminar.Enabled = true;
        }
        private void Buscar()
        {
            if (cbSeleccionBuscar.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un criterio de búsqueda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedValue = cbSeleccionBuscar.SelectedItem.ToString();
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

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }
    }
}
