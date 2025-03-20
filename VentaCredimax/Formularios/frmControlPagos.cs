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
        private GestorReportes _gestorReportes = new GestorReportes();
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
            bool mostrarTodas = cbTodasVentas.Checked;
            string criterioSeleccionado = "";
            if (cbOrdenarPor.Items.Count > 0)
            {
                criterioSeleccionado = cbOrdenarPor.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un criterio de ordenación.");
            }
            dgvVentas.DataSource = _gestorVenta.ListarVentas(criterioSeleccionado, mostrarTodas);
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
            if (cbBuscarPor.SelectedIndex == -1)
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

        private void cbTodasVentas_CheckedChanged(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void btnImpDetalleDeVenta_Click(object sender, EventArgs e)
        {

            // Obtener el IdCliente de la columna "IdCliente"
            int IdVenta = Convert.ToInt32(dgvVentas?.CurrentRow?.Cells["VentaId"]?.Value);
            if (IdVenta > 0)
            {
              
                ImprimirDetalleDeVenta(IdVenta);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una venta para ver su detalle.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void ImprimirDetalleDeVenta(int IdVenta)
        {
            // Verificar si hay una fila seleccionada en el DataGridView
            if (dgvVentas.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione una venta para ver su detalle.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            List<sp_DetalleDeVenta_Result> listaDetalleDeVenta = _gestorReportes.DatosDetalleDeVenta(IdVenta);

            if (listaDetalleDeVenta.Any())
            {
                // Cargar la plantilla HTML
                string textoHtml = Properties.Resources.DetalleDeVenta.ToString();

                textoHtml = textoHtml.Replace("@nombre", listaDetalleDeVenta.FirstOrDefault().NombreCliente);
                textoHtml = textoHtml.Replace("@apellido", listaDetalleDeVenta.FirstOrDefault().ApellidoCliente);
                textoHtml = textoHtml.Replace("@dni", listaDetalleDeVenta.FirstOrDefault().DNICliente.ToString());
                textoHtml = textoHtml.Replace("@direccion", listaDetalleDeVenta.FirstOrDefault().DireccionCliente);
                textoHtml = textoHtml.Replace("@telefono", listaDetalleDeVenta.FirstOrDefault().TelefonoCliente);
                textoHtml = textoHtml.Replace("@fecha-hoy", DateTime.Now.ToString("dd/MM/yyyy"));

                // Generar las filas de la tabla agrupando por VentaId
                string filasTabla = string.Join("", listaDetalleDeVenta
                    .GroupBy(v => v.VentaId) // Agrupar por VentaId
                    .SelectMany(grupo => grupo.Select((venta, index) => $@"
                    <tr>
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Articulo}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{string.Format("{0:#,##0.00}", venta.Precio).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Cuotas}</td>" : "")}  
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.FechaDeVenta.ToString("dd/MM/yyyy")}</td>": "")}
                        <td>{venta.NumeroDeCuota}</td>
                        <td>{string.Format("{0:#,##0.00}", venta.MontoCuota).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>
                        <td>{(venta.FechaProgramadaDeCuota.ToString("dd/MM/yyyy"))}</td>
                        <td>{(venta.FechaQuePagoCuota.HasValue ? venta.FechaQuePagoCuota.Value.ToString("dd/MM/yyyy") : "Pendiente")}</td>
                    </tr>")));

                // Reemplazar en la plantilla HTML
                textoHtml = textoHtml.Replace("@filasTabla", filasTabla);
                textoHtml = textoHtml.Replace("@fecha-programada-cuota", listaDetalleDeVenta.FirstOrDefault()?.FechaProgramadaDeCuota.ToString("dd/MM/yyyy"));
                

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = $"detalle-de-venta_{listaDetalleDeVenta.FirstOrDefault().NombreCliente + listaDetalleDeVenta.FirstOrDefault().ApellidoCliente}-{DateTime.Now.ToString("dd_MM_yyyy")}.pdf";
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
    }
}
