using CEntidades;
using CEntidades.DTOs;
using CLogica;
using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmMenu : Form
    {
        private GestorVenta _gestorVenta = new GestorVenta();
        private GestorReportes _gestorReportes = new GestorReportes();
        public frmMenu()
        {
            InitializeComponent();           
        }
        private void frmMenu_Load(object sender, EventArgs e)
        {
            
            cbOrdenarPor.SelectedIndex = 0;
            ListarVentas();
            OcultarColumnas();
            EstiloDataGrid();
            FormatearFilas();   
        }

        private void btnGestionCliente_Click(object sender, EventArgs e)
        {
            frmCliente frmCliente = new frmCliente();
            frmCliente.ShowDialog();
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            pSubMenuInformes.Visible = true;
        }

        private void btnInformeCuotas_Click(object sender, EventArgs e)
        {
            // Obtener el IdCliente de la columna "IdCliente"
            int idCliente = Convert.ToInt32(dgvVentasMenu?.CurrentRow?.Cells["IdCliente"]?.Value);
            if (idCliente > 0)
            {
                pSubMenuInformes.Visible = false;
                ImprimirHistorialVentasPorCliente(idCliente);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una venta del cliente para generar su historial de reporte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnInformeVentas_Click(object sender, EventArgs e)
        {
            pSubMenuInformes.Visible = false;
            frmVentasPorFecha frmReporteVentasPorFecha = new frmVentasPorFecha();
            frmReporteVentasPorFecha.ShowDialog();
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
            dgvVentasMenu.DataSource = _gestorVenta.ListarVentasMenu(criterioSeleccionado);
            OcultarColumnas();
            FormatearFilas();
            EstiloDataGrid();
            
        }

        private void FiltrarVentasPorCliente()
        {
            dgvVentasMenu.DataSource = _gestorVenta.FiltrarVentasPorCliente(txtBuscar.Text);
            OcultarColumnas();
        }

        private void FiltrarVentasPorArticulo()
        {
            dgvVentasMenu.DataSource = _gestorVenta.FiltrarVentasPorArticulo(txtBuscar.Text);
            OcultarColumnas();
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

        private void OcultarColumnas()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                dgvVentasMenu.Columns["VentaId"].Visible = false;
                dgvVentasMenu.Columns["IdCliente"].Visible = false;
                dgvVentasMenu.Columns["FechaAnulacion"].Visible = false;
                dgvVentasMenu.Columns["CuotasVencidas"].Visible = false;
            }
        }

        private void EstiloDataGrid()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                dgvVentasMenu.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
                dgvVentasMenu.Columns["FormaDePago"].HeaderText = "Frecuencia de pago";
                dgvVentasMenu.Columns["FechaDeInicio"].HeaderText = "Fecha compra";
                dgvVentasMenu.Columns["FechaDeCancelacion"].HeaderText = "Cancelacion compra";
            }
        }

        private void dgvVentasMenu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validar que haya una fila seleccionada
            if (dgvVentasMenu.CurrentRow != null)
            {
                // Obtener el ID de la venta desde la fila seleccionada
                int ventaId = Convert.ToInt32(dgvVentasMenu.CurrentRow.Cells["VentaId"].Value);

                // Abrir el formulario frmControlCuotas pasando el ID de la venta
                frmControlCuotas controlCuotas = new frmControlCuotas(ventaId);
                controlCuotas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione una fila válida antes de hacer doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            frmVentas venta = new frmVentas();
            venta.FormClosed += (s, args) => ListarVentas(); // Refresca la lista al cerrar el formulario
            venta.ShowDialog();
        }

        private void btnGestionPagos_Click(object sender, EventArgs e)
        {
            frmControlPagos controlPagos = new frmControlPagos();
            controlPagos.ShowDialog();
        }

        private void FormatearFilas()
        {
            if (dgvVentasMenu.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvVentasMenu.Rows)
                {
                    var venta = row.DataBoundItem as VentaDTO;
                    if (venta != null && venta.CuotasVencidas)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
        }

        private void cbOrdenarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListarVentas();
        }

        private void ImprimirHistorialVentasPorCliente(int clientId)
        {
            // Verificar si hay una fila seleccionada en el DataGridView
            if (dgvVentasMenu.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione una venta del cliente para generar su historial de reporte.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            List<sp_GetVentasByClientId_Result> listaVentasPorCliente = _gestorReportes.DatosVentasPorCliente(clientId);

            if (listaVentasPorCliente.Any())
            {
                // Cargar la plantilla HTML
                string textoHtml = Properties.Resources.VentasPorCliente.ToString();

                textoHtml = textoHtml.Replace("@nombre", listaVentasPorCliente.FirstOrDefault().NombreCliente);
                textoHtml = textoHtml.Replace("@apellido", listaVentasPorCliente.FirstOrDefault().ApellidoCliente);
                textoHtml = textoHtml.Replace("@dni", listaVentasPorCliente.FirstOrDefault().DNICliente.ToString());
                textoHtml = textoHtml.Replace("@direccion", listaVentasPorCliente.FirstOrDefault().DireccionCliente);
                textoHtml = textoHtml.Replace("@telefono", listaVentasPorCliente.FirstOrDefault().TelefonoCliente);

                // Generar las filas de la tabla agrupando por VentaId
                string filasTabla = string.Join("", listaVentasPorCliente
                    .GroupBy(v => v.VentaId) // Agrupar por VentaId
                    .SelectMany(grupo => grupo.Select((venta, index) => $@"
                    <tr>
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Articulo}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{string.Format("{0:#,##0.00}", venta.Precio).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>" : "")}
                        {(index == 0 ? $"<td rowspan='{grupo.Count()}'>{venta.Cuotas}</td>" : "")}                   
                        <td>{venta.FechaDeVenta.ToString("dd/MM/yyyy")}</td>
                        <td>{venta.NumeroDeCuota}</td>
                        <td>{string.Format("{0:#,##0.00}", venta.MontoCuota).Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>
                        <td>{(venta.FechaQuePagoCuota.HasValue ? venta.FechaQuePagoCuota.Value.ToString("dd/MM/yyyy") : "")}</td>
                    </tr>")));

                // Reemplazar en la plantilla HTML
                textoHtml = textoHtml.Replace("@filasTabla", filasTabla);

                textoHtml = textoHtml.Replace("@talle", listaVentasPorCliente.FirstOrDefault().Talle.ToString());         
                textoHtml = textoHtml.Replace("@fecha-cancelacion-cuota", listaVentasPorCliente.FirstOrDefault()?.FechaCancelacionCuotas?.ToString("dd/MM/yyyy") ?? "");
                textoHtml = textoHtml.Replace("@fecha-programada-cuota", listaVentasPorCliente.FirstOrDefault()?.FechaProgramadaDeCuota.ToString("dd/MM/yyyy"));
                textoHtml = textoHtml.Replace("@fecha-venta", listaVentasPorCliente.FirstOrDefault()?.FechaDeVenta.ToString("dd/MM/yyyy"));
                textoHtml = textoHtml.Replace("@frecuencia-pago", listaVentasPorCliente.FirstOrDefault().Nombre.ToString());

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = $"ventas-por-cliente_{listaVentasPorCliente.FirstOrDefault().NombreCliente + listaVentasPorCliente.FirstOrDefault().ApellidoCliente}-{DateTime.Now.ToString("dd_MM_yyyy")}.pdf";
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

        private void btnSeleccionarVenta_Click(object sender, EventArgs e)
        {
            ListarVentas();
        }

    }
}
