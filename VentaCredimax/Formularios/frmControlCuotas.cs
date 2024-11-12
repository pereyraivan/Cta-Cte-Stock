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
    public partial class frmControlCuotas : Form
    {
        private int VentaId;
        private GestorCuotas _gestorCuotas = new GestorCuotas();
        public frmControlCuotas(int ventaId)
        {
            InitializeComponent();
            VentaId = ventaId;
            CargarCuotas();
        }

        private void frmControlCuotas_Load(object sender, EventArgs e)
        {

        }
        private void CargarCuotas()
        {
            try
            {
              
                List<Cuota> listaCuotas = _gestorCuotas.ObtenerCuotasPorVenta(VentaId);

                // Asignar la lista de cuotas al DataGridView
                dgvCuotas.DataSource = listaCuotas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cuotas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
