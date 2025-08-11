using CEntidades;
using CEntidades.DTOs;
using CLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmVerVentasPorFecha : Form
    {
        private GestorReportes _gestorReportes = new GestorReportes();
        private GestorCuotas _gestorCuotas = new GestorCuotas();

        public frmVerVentasPorFecha()
        {
            InitializeComponent();
            this.Load += frmVerVentasPorFecha_Load;
        }

        private void frmVerVentasPorFecha_Load(object sender, EventArgs e)
        {
            // Configurar fechas por defecto (día actual)
            dtpFechaHasta.Value = DateTime.Now;
            dtpFechaDesde.Value = DateTime.Now.Date;
            
            // Configurar controles
            ConfigurarFormulario();
            
            // Cargar datos iniciales
            FiltrarVentasPorFecha();
        }

        private void ConfigurarFormulario()
        {
            // Configurar DataGridView
            dgvVentasPorFecha.AutoGenerateColumns = true;
            dgvVentasPorFecha.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentasPorFecha.AllowUserToAddRows = false;
            dgvVentasPorFecha.AllowUserToDeleteRows = false;
            dgvVentasPorFecha.ReadOnly = true;
            dgvVentasPorFecha.BackgroundColor = Color.White;
            dgvVentasPorFecha.GridColor = Color.LightGray;
            dgvVentasPorFecha.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            
            // Configurar DateTimePickers
            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            
            // Configurar eventos
            btnAnularCompra.Click += BtnFiltrar_Click;
            dtpFechaDesde.ValueChanged += DateTimePicker_ValueChanged;
            dtpFechaHasta.ValueChanged += DateTimePicker_ValueChanged;
        }

        private void FiltrarVentasPorFecha()
        {
            try
            {
                DateTime fechaDesde = dtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1); // Incluir todo el día

                // Validar que la fecha desde sea menor o igual a fecha hasta
                if (fechaDesde > fechaHasta.Date)
                {
                    MessageBox.Show("La fecha 'Desde' debe ser menor o igual a la fecha 'Hasta'.", "Error de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear DataTable para mostrar datos unificados
                var tablaUnificada = CrearTablaUnificada(fechaDesde, fechaHasta.Date);

                // Mostrar en el DataGridView
                dgvVentasPorFecha.DataSource = tablaUnificada;
                
                // Aplicar formato unificado
                FormatearColumnasUnificadas();
                
                // Calcular totales separados
                CalcularTotalesSeparados(fechaDesde, fechaHasta.Date);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcularTotales(List<sp_ReporteVentas_Result> ventas)
        {
            try
            {
                decimal totalVentas = 0;

                foreach (var venta in ventas)
                {
                    // Sumar el total de ventas
                    if (venta.Total.HasValue)
                        totalVentas += venta.Total.Value;
                }

                // Calcular el total de cuotas pagadas en el rango de fechas
                DateTime fechaDesde = dtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1);
                decimal totalCuotasPagadas = _gestorCuotas.ObtenerTotalCuotasPagadasPorFecha(fechaDesde, fechaHasta);

                // Formatear totales en pesos argentinos
                var cultura = new CultureInfo("es-AR");
                lblTotal.Text = $"Total Ventas: {totalVentas.ToString("C", cultura)}";
                lblTotalPagado.Text = $"Total cuotas pagadas: {totalCuotasPagadas.ToString("C", cultura)}";
            }
            catch (Exception ex)
            {
                lblTotal.Text = "Error al calcular total de ventas";
                lblTotalPagado.Text = "Error al calcular total de pagos";
                MessageBox.Show($"Error al calcular totales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcularTotalesConTrabajos(List<sp_ReporteVentas_Result> ventas, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                decimal totalVentas = 0;

                foreach (var venta in ventas)
                {
                    // Sumar el total de ventas
                    if (venta.Total.HasValue)
                        totalVentas += venta.Total.Value;
                }

                // Obtener trabajos de aire acondicionado en el mismo período
                var gestorAire = new GestorAireAcondicionado();
                var trabajos = gestorAire.ListarTrabajosPorFecha(fechaDesde, fechaHasta);
                decimal totalTrabajos = 0;
                
                foreach (var trabajo in trabajos)
                {
                    if (trabajo.Precio.HasValue)
                        totalTrabajos += trabajo.Precio.Value;
                }

                // Calcular el total de cuotas pagadas en el rango de fechas
                decimal totalCuotasPagadas = _gestorCuotas.ObtenerTotalCuotasPagadasPorFecha(fechaDesde, fechaHasta);

                // Formatear totales en pesos argentinos
                var cultura = new CultureInfo("es-AR");
                decimal totalGeneral = totalVentas + totalTrabajos;
                
                lblTotal.Text = $"Total Ingresos: {totalGeneral.ToString("C", cultura)} " +
                               $"(Ventas: {totalVentas.ToString("C", cultura)}, " +
                               $"Trabajos A/A: {totalTrabajos.ToString("C", cultura)} [{trabajos.Count}])";
                lblTotalPagado.Text = $"Total cuotas pagadas: {totalCuotasPagadas.ToString("C", cultura)}";
            }
            catch (Exception ex)
            {
                lblTotal.Text = "Error al calcular totales";
                lblTotalPagado.Text = "Error al calcular total de pagos";
                MessageBox.Show($"Error al calcular totales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearColumnas()
        {
            if (dgvVentasPorFecha.Columns.Count > 0)
            {
                // Configurar anchos y formato de columnas
                foreach (DataGridViewColumn column in dgvVentasPorFecha.Columns)
                {
                    switch (column.Name.ToLower())
                    {
                        case "id":
                        case "ventaid":
                        case "idarticulo":
                            column.Visible = false; // Ocultar columnas de ID
                            break;
                        case "fecha":
                        case "fechaventa":
                        case "fechadeventa":
                            column.HeaderText = "Fecha";
                            column.DefaultCellStyle.Format = "dd/MM/yyyy";
                            column.Width = 100;
                            break;
                        case "cliente":
                        case "nombrecliente":
                        case "apellidoynombre":
                            column.HeaderText = "Cliente";
                            column.Width = 200;
                            break;
                        case "articulo":
                        case "nombrearticulo":
                        case "descripcion":
                            column.HeaderText = "Artículo";
                            column.Width = 200;
                            break;
                        case "cantidad":
                            column.HeaderText = "Cant.";
                            column.Width = 60;
                            break;
                        case "precio":
                        case "precioventa":
                        case "preciounitario":
                            column.HeaderText = "Precio Unit.";
                            column.DefaultCellStyle.Format = "C";
                            column.DefaultCellStyle.FormatProvider = new CultureInfo("es-AR");
                            column.Width = 120;
                            break;
                        case "total":
                        case "totalventa":
                        case "subtotal":
                            column.HeaderText = "Total";
                            column.DefaultCellStyle.Format = "C";
                            column.DefaultCellStyle.FormatProvider = new CultureInfo("es-AR");
                            column.Width = 120;
                            break;
                        case "formapago":
                        case "modalidadpago":
                            column.HeaderText = "Forma Pago";
                            column.Width = 120;
                            break;
                        case "vendedor":
                        case "nombrevendedor":
                            column.HeaderText = "Vendedor";
                            column.Width = 150;
                            break;
                    }
                }
                
                // Configurar ancho automático después de configurar columnas
                dgvVentasPorFecha.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                
                // Estilo del encabezado
                dgvVentasPorFecha.EnableHeadersVisualStyles = false;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
                dgvVentasPorFecha.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F);
            }
        }

        private void FormatearColumnasUnificadas()
        {
            if (dgvVentasPorFecha.Columns.Count > 0)
            {
                // Configurar columnas específicas para el reporte unificado
                foreach (DataGridViewColumn column in dgvVentasPorFecha.Columns)
                {
                    switch (column.Name.ToLower())
                    {
                        case "id":
                        case "dni":
                        case "codigoarticulo":
                            column.Visible = false; // Ocultar columnas auxiliares
                            break;
                        case "tipo":
                            column.HeaderText = "Tipo";
                            column.Width = 80;
                            break;
                        case "fecha":
                            column.HeaderText = "Fecha";
                            column.DefaultCellStyle.Format = "dd/MM/yyyy";
                            column.Width = 100;
                            break;
                        case "cliente":
                            column.HeaderText = "Cliente";
                            column.Width = 150;
                            break;
                        case "descripcion":
                            column.HeaderText = "Descripción";
                            column.Width = 200;
                            break;
                        case "cantidad":
                            column.HeaderText = "Cant.";
                            column.Width = 60;
                            break;
                        case "precio":
                            column.HeaderText = "Precio Unit.";
                            column.DefaultCellStyle.Format = "C";
                            column.DefaultCellStyle.FormatProvider = new CultureInfo("es-AR");
                            column.Width = 120;
                            break;
                        case "total":
                            column.HeaderText = "Total";
                            column.DefaultCellStyle.Format = "C";
                            column.DefaultCellStyle.FormatProvider = new CultureInfo("es-AR");
                            column.Width = 120;
                            break;
                        case "formapago":
                            column.HeaderText = "Forma Pago";
                            column.Width = 100;
                            break;
                        case "estado":
                            column.HeaderText = "Estado";
                            column.Width = 100;
                            break;
                        case "cuotas":
                            column.HeaderText = "Cuotas";
                            column.Width = 60;
                            break;
                    }
                }
                
                // Configurar ancho automático después de configurar columnas
                dgvVentasPorFecha.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                
                // Estilo del encabezado
                dgvVentasPorFecha.EnableHeadersVisualStyles = false;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkGreen;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dgvVentasPorFecha.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
                dgvVentasPorFecha.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F);
            }
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            FiltrarVentasPorFecha();
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Auto-filtrar cuando cambia la fecha (opcional)
            if (dtpFechaDesde.Value <= dtpFechaHasta.Value)
            {
                FiltrarVentasPorFecha();
            }
        }

        private DataTable CrearTablaUnificada(DateTime fechaDesde, DateTime fechaHasta)
        {
            var tabla = new DataTable();
            
            // Definir columnas
            tabla.Columns.Add("Tipo", typeof(string));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Cliente", typeof(string));
            tabla.Columns.Add("Descripcion", typeof(string));
            tabla.Columns.Add("Cantidad", typeof(int));
            tabla.Columns.Add("PrecioUnitario", typeof(decimal));
            tabla.Columns.Add("Total", typeof(decimal));
            tabla.Columns.Add("FormaPago", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));

            // Obtener ventas
            var ventas = _gestorReportes.DatosVentasPorFecha(fechaDesde, fechaHasta);
            foreach (var venta in ventas)
            {
                var row = tabla.NewRow();
                row["Tipo"] = "Venta";
                row["Fecha"] = venta.FechaDeVenta;
                row["Cliente"] = $"{venta.Nombre} {venta.Apellido}";
                row["Descripcion"] = venta.Articulo;
                row["Cantidad"] = venta.Cantidad ?? 1;
                row["PrecioUnitario"] = venta.Precio ?? 0;
                row["Total"] = venta.Total ?? 0;
                row["FormaPago"] = venta.FormaDePago;
                row["Estado"] = venta.Estado;
                tabla.Rows.Add(row);
            }

            // Obtener trabajos de aire acondicionado
            var gestorAire = new GestorAireAcondicionado();
            var trabajos = gestorAire.ListarTrabajosPorFecha(fechaDesde, fechaHasta);
            foreach (var trabajo in trabajos)
            {
                var row = tabla.NewRow();
                row["Tipo"] = "Trabajo A/A";
                row["Fecha"] = trabajo.FechaTrabajo ?? DateTime.Now;
                row["Cliente"] = trabajo.Cliente ?? "N/A";
                row["Descripcion"] = trabajo.DescripcionTrabajo;
                row["Cantidad"] = 1;
                row["PrecioUnitario"] = trabajo.Precio ?? 0;
                row["Total"] = trabajo.Precio ?? 0;
                row["FormaPago"] = "Contado";
                row["Estado"] = "Completado";
                tabla.Rows.Add(row);
            }

            // Ordenar por fecha descendente
            var view = tabla.DefaultView;
            view.Sort = "Fecha DESC";
            return view.ToTable();
        }

        private void CalcularTotalesSeparados(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                // Calcular total de ventas
                var ventas = _gestorReportes.DatosVentasPorFecha(fechaDesde, fechaHasta);
                decimal totalVentas = ventas.Sum(v => v.Total ?? 0);

                // Calcular total de trabajos
                var gestorAire = new GestorAireAcondicionado();
                var trabajos = gestorAire.ListarTrabajosPorFecha(fechaDesde, fechaHasta);
                decimal totalTrabajos = trabajos.Sum(t => t.Precio ?? 0);

                // Calcular total de cuotas pagadas
                decimal totalCuotasPagadas = _gestorCuotas.ObtenerTotalCuotasPagadasPorFecha(fechaDesde, fechaHasta);

                // Formatear en pesos argentinos
                var cultura = new CultureInfo("es-AR");
                
                // Mostrar totales en labels separados
                lblTotal.Text = $"Total Ventas: {totalVentas.ToString("C", cultura)} [{ventas.Count}]";
                lblTotalTrabajos.Text = $"Total Trabajos A/A: {totalTrabajos.ToString("C", cultura)} [{trabajos.Count}]";
                lblTotalPagado.Text = $"Total Cuotas Pagadas: {totalCuotasPagadas.ToString("C", cultura)}";
            }
            catch (Exception ex)
            {
                lblTotal.Text = "Error al calcular total de ventas";
                lblTotalTrabajos.Text = "Error al calcular total de trabajos";
                lblTotalPagado.Text = "Error al calcular total de pagos";
                MessageBox.Show($"Error al calcular totales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
