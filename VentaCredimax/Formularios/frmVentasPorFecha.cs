using CEntidades;
using CLogica;
using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmVentasPorFecha : Form
    {
        GestorReportes _gestorReportes = new GestorReportes();
        public frmVentasPorFecha()
        {
            InitializeComponent();
        }

        private void frmReporteVentasPorFecha_Load(object sender, EventArgs e)
        {
            
        }

        private void btnImprimirComprobante_Click(object sender, EventArgs e)
        {
            if (dtpDesde.Value == null || dtpHasta.Value == null)
            {
                MessageBox.Show("Debe ingresar las fechas de inicio y fin para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DateTime fechaDesde = dtpDesde.Value;
                DateTime fechaHasta = dtpHasta.Value;
                if(fechaDesde > fechaHasta)
                {
                    MessageBox.Show("Debe ingresar las fechas de inicio y fin para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    imprimirReporteVentasPdf(fechaDesde, fechaHasta);
                }          
            }
        }
        private void imprimirReporteVentasPdf(DateTime fechaDesde , DateTime fechaHasta)
        {
            List<sp_ReporteVentas_Result> listaVentasPorFecha = _gestorReportes.DatosVentasPorFecha(fechaDesde, fechaHasta);

            if (listaVentasPorFecha.Any())
            {
                // Cargar la plantilla HTML
                string textoHtml = Properties.Resources.VentasPorFecha.ToString();

                textoHtml = textoHtml.Replace("@fecha-desde", fechaDesde.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                textoHtml = textoHtml.Replace("@fecha-hasta", fechaHasta.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                // Generar las filas de la tabla agrupando por VentaId
                string filasTabla = string.Join("", listaVentasPorFecha.Select(venta => $@"
                <tr>
                    <td>{venta.FechaDeVanta:dd/MM/yyyy}</td>
                    <td>{venta.Nombre} {venta.Apellido}</td>
                    <td>{venta.DNI}</td>
                    <td>{venta.Articulo}</td>
                    <td>{venta.Precio?.ToString("#,##0.00").Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>
                    <td>{venta.Talle}</td>
                    <td>{venta.Cantidad}</td>
                    <td>{venta.FormaDePago}</td>
                    <td>{venta.Cuotas}</td>                
                    <td>{venta.Total?.ToString("#,##0.00").Replace(",", "X").Replace(".", ",").Replace("X", ".")}</td>
                    <td>{venta.Estado}</td>
                </tr>"));

                // Reemplazar en la plantilla HTML
                textoHtml = textoHtml.Replace("@filasTabla", filasTabla);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = $"ventas-por-fecha-{fechaDesde:dd_MM_yyyy}-{fechaHasta:dd_MM_yyyy}.pdf";
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
