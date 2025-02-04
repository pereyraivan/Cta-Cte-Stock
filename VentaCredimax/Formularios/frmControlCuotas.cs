using CEntidades;
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
            //dgvCuotas.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvCuotas_CellFormatting);         
        }
        private void frmControlCuotas_Load(object sender, EventArgs e)
        {
            CargarCuotas();
            EstiloDataGridView();
            PintarFilaPorEstado();           
        }
        private void CargarCuotas()
        {
            List<CuotaDTO> cuotas = _gestorCuotas.ObtenerCuotasPorVenta(VentaId);
            dgvCuotas.DataSource = cuotas;
        }
        private void RegistrarPago()
        {
            if (dgvCuotas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para registrar el pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el número de la cuota seleccionada
            int numeroCuotaSeleccionada = Convert.ToInt32(dgvCuotas.CurrentRow.Cells["NumeroDeCuota"].Value);
            int ventaId = Convert.ToInt32(dgvCuotas.CurrentRow.Cells["VentaId"].Value);
            string estadoCuota = dgvCuotas.CurrentRow.Cells["Estado"].Value.ToString();
            List<CuotaDTO> cuotas = _gestorCuotas.ObtenerCuotasPorVenta(ventaId);
            
            bool hayCuotasAnterioresPendientes = cuotas
            .Where(c => c.NumeroDeCuota < numeroCuotaSeleccionada)
            .Any(c => c.Estado != "Pagada"); // Estado = false significa pendiente
            if(estadoCuota == "Pagada")
            {
                MessageBox.Show("La cuota ya se encuentra pagada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (hayCuotasAnterioresPendientes)
            {
                MessageBox.Show("Tiene cuotas anteriores pendientes de pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el ID de la cuota seleccionada
            int idCuota = Convert.ToInt32(dgvCuotas.CurrentRow.Cells["CuotaId"].Value);

            // Registrar el pago a través del gestor
            bool resultado = _gestorCuotas.RegistrarPago(idCuota);

            if (resultado)
            {
                MessageBox.Show("Pago registrado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCuotas(); // Recargar cuotas
            }
            else
            {
                MessageBox.Show("Hubo un problema al registrar el pago. Inténtelo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistrarPago();
            PintarFilaPorEstado();
        }
        private void EstiloDataGridView()
        {
            if (dgvCuotas.Rows.Count > 0)
            {
                dgvCuotas.Columns["CuotaId"].Visible = false;
                dgvCuotas.Columns["VentaId"].Visible = false;
                dgvCuotas.Columns["NumeroDeCuota"].HeaderText = "Nro de Cuota";
                dgvCuotas.Columns["MontoCuota"].HeaderText = "Monto Cuota";
                dgvCuotas.Columns["FechaProgramada"].HeaderText = "Vencimiento";
                dgvCuotas.Columns["FechaPago"].HeaderText = "Fecha de Pago";
               
            }
        }

        private void PintarFilaPorEstado()
        {
            if (dgvCuotas.Rows.Count > 0)
            {
                // Aplicar estilos de color a las filas
                foreach (DataGridViewRow row in dgvCuotas.Rows)
                {
                    string estado = row.Cells["Estado"].Value.ToString(); // Verifica si está pagada
                    DateTime fechaVencimiento = Convert.ToDateTime(row.Cells["FechaProgramada"].Value);

                    if (estado == "Pagada")
                    {
                        // Pagada: Color verde
                        row.DefaultCellStyle.BackColor = Color.LightGreen;

                    }
                    else if (estado == "Pendiente")
                    {
                        if (fechaVencimiento < DateTime.Now)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void dgvCuotas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvCuotas.ClearSelection();
            dgvCuotas.CurrentCell = null;
        }

        private void btnImprimirComprobante_Click(object sender, EventArgs e)
        {
            if (dgvCuotas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para registrar el pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int ventaId = Convert.ToInt32(dgvCuotas.CurrentRow.Cells["VentaId"].Value);
                int numeroCuota = Convert.ToInt32(dgvCuotas.CurrentRow.Cells["NumeroDeCuota"].Value);
                MostrarReporte(ventaId, numeroCuota);
            }         
        }
        private void MostrarReporte(int ventaId, int numeroCuota)
        {
            if (dgvCuotas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una cuota para imprimir el recibo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Obtener datos de la cuota seleccionada
         
            string estado = dgvCuotas.CurrentRow.Cells["Estado"].Value?.ToString();

            if (estado == "Pendiente")
            {
                MessageBox.Show("No se puede imprimir el recibo porque la cuota no está pagada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Crear formulario de reporte
                frmReportReciboDePago frmReporte = new frmReportReciboDePago(ventaId, numeroCuota);
                //frmReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}


