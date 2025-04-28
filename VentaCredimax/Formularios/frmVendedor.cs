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
    public partial class frmVendedor : Form
    {
        private GestorVendedor gestorVendedor = new GestorVendedor();
        private string id = null;

        public frmVendedor()
        {
            InitializeComponent();
        }

        private void frmVendedor_Load(object sender, EventArgs e)
        {
            Listar();
            OcultarColumnas();  
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void Listar()
        {
            dgvVendedores.DataSource = gestorVendedor.Listar();
        }

        private void Limpiar()
        {
            txtNombreYApellido.Text = "";
            txtTelefono.Text = "";
            id = null;
            OcultarColumnas();
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(txtNombreYApellido.Text))
            {
                MessageBox.Show("El campo Nombre y Apellido es obligatorio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var vendedor = new Vendedor
            {
                NombreYApellido = txtNombreYApellido.Text,
                Telefono = txtTelefono.Text
            };

            gestorVendedor.Guardar(vendedor);
            MessageBox.Show("Vendedor guardado con éxito.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listar();
            Limpiar();
        }

        private void Modificar()
        {
            if (id == null)
            {
                MessageBox.Show("Seleccione un vendedor para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var vendedor = new Vendedor
            {
                VendedorId = Convert.ToInt32(id),
                NombreYApellido = txtNombreYApellido.Text,
                Telefono = txtTelefono.Text
            };

            gestorVendedor.Modificar(vendedor);
            MessageBox.Show("Vendedor modificado con éxito.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listar();
            Limpiar();
        }

        private void Eliminar()
        {
            if (dgvVendedores.SelectedRows.Count > 0)
            {
                var confirmar = MessageBox.Show("¿Está seguro de eliminar este vendedor?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmar == DialogResult.Yes)
                {
                    id = dgvVendedores.CurrentRow.Cells["VendedorId"].Value.ToString();
                    gestorVendedor.Eliminar(Convert.ToInt32(id));
                    MessageBox.Show("Operacion Exitosa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listar();
                    Limpiar();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
        }
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            Guardar();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            Modificar();
        }

        private void dgvVendedores_DoubleClick(object sender, EventArgs e)
        {
            if (dgvVendedores.SelectedRows.Count > 0)
            {
                id = dgvVendedores.CurrentRow.Cells["VendedorId"].Value.ToString();
                txtNombreYApellido.Text = dgvVendedores.CurrentRow.Cells["NombreYApellido"].Value.ToString();
                txtTelefono.Text = dgvVendedores.CurrentRow.Cells["Telefono"].Value?.ToString() ?? "";
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }

        private void dgvVendedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {   
            btnEliminar.Enabled = true;
        }
      
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            dgvVendedores.DataSource = gestorVendedor.Buscar((txtBuscar.Text));
        }
        private void OcultarColumnas()
        {
            dgvVendedores.Columns["VendedorId"].Visible = false;
        }

    }
}

