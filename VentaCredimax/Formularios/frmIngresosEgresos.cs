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
    public partial class frmIngresosEgresos : Form
    {
        private GestorMovimientos _gestorMovimientos;
        private List<MovimientoDTO> _movimientos;

        public frmIngresosEgresos()
        {
            InitializeComponent();
            _gestorMovimientos = new GestorMovimientos();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar las fechas por defecto (último mes)
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;

            // Configurar el DataGridView
            ConfigurarDataGridView();

            // Configurar eventos
            btnAnularCompra.Click += BtnFiltrar_Click;
            dtpFechaDesde.ValueChanged += FechaChanged;
            dtpFechaHasta.ValueChanged += FechaChanged;

            // Configurar los labels
            lblTotal.Text = "Total Ventas: $0.00";
            lblTotalCuotas.Text = "Total Cuotas Pagadas: $0.00";
            lblTotalTrabajos.Text = "Total Trabajos A/A: $0.00";
            lblTotalPagado.Text = "Total Egresos: $0.00";

            // NO cargar datos iniciales automáticamente
            // El usuario debe hacer clic en Filtrar o cambiar las fechas
        }

        private void ConfigurarDataGridView()
        {
            dgvIngresosEgresos.AutoGenerateColumns = false;
            dgvIngresosEgresos.AllowUserToAddRows = false;
            dgvIngresosEgresos.ReadOnly = true;
            dgvIngresosEgresos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Limpiar columnas existentes
            dgvIngresosEgresos.Columns.Clear();

            // Agregar columnas
            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha",
                Name = "colFecha",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tipo",
                HeaderText = "Tipo",
                Name = "colTipo"
            });

            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Concepto",
                HeaderText = "Concepto",
                Name = "colConcepto"
            });

            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                HeaderText = "Descripción",
                Name = "colDescripcion"
            });

            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cliente",
                HeaderText = "Cliente",
                Name = "colCliente"
            });

            dgvIngresosEgresos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Monto",
                HeaderText = "Monto",
                Name = "colMonto",
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Configurar el evento para el formato de filas
            dgvIngresosEgresos.DataBindingComplete += DgvIngresosEgresos_DataBindingComplete;
        }

        private void DgvIngresosEgresos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Aplicar colores diferentes para ingresos y egresos
            foreach (DataGridViewRow row in dgvIngresosEgresos.Rows)
            {
                if (row.DataBoundItem is MovimientoDTO movimiento)
                {
                    if (movimiento.Tipo == "Ingreso")
                    {
                        // Color especial para trabajos de A/A (más claro que el verde normal)
                        if (movimiento.Concepto == "Trabajo A/A")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(193, 255, 193); // Verde muy claro
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 179, 113); // Verde medio mar
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                        }
                    }
                    else if (movimiento.Tipo == "Egreso")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                }
            }
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void FechaChanged(object sender, EventArgs e)
        {
            // Opcional: cargar automáticamente cuando cambian las fechas
            // CargarDatos();
        }

        // Método temporal para testing - agregar al form_load o evento de botón
        private void ProbarConexionBaseDatos()
        {
            try
            {
                string estadisticas = _gestorMovimientos.ObtenerEstadisticasBaseDatos();
                MessageBox.Show($"Estadísticas de la base de datos:\n{estadisticas}", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                DateTime fechaDesde = dtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1);

                // Debug: mostrar las fechas que se están usando
                this.Text = $"Ingresos y Egresos - Desde: {fechaDesde:dd/MM/yyyy} Hasta: {fechaHasta:dd/MM/yyyy}";

                // Validar que fechaDesde no sea mayor que fechaHasta
                if (fechaDesde > fechaHasta)
                {
                    MessageBox.Show("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'", "Error de fechas", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Limpiar datos anteriores
                dgvIngresosEgresos.DataSource = null;
                _movimientos = null;

                // Obtener los movimientos
                _movimientos = _gestorMovimientos.ObtenerMovimientosPorFecha(fechaDesde, fechaHasta);

                // Verificar si hay datos
                if (_movimientos == null || _movimientos.Count == 0)
                {
                    MessageBox.Show($"No se encontraron movimientos entre {fechaDesde:dd/MM/yyyy} y {fechaHasta:dd/MM/yyyy}", 
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Limpiar totales
                    lblTotal.Text = "Total Ventas: $0.00";
                    lblTotalCuotas.Text = "Total Cuotas Pagadas: $0.00";
                    lblTotalTrabajos.Text = "Total Trabajos A/A: $0.00";
                    lblTotalPagado.Text = "Total Egresos: $0.00";
                    return;
                }

                // Bindear al DataGridView
                dgvIngresosEgresos.DataSource = _movimientos;

                // Obtener resumen
                var resumen = _gestorMovimientos.ObtenerResumenPorFecha(fechaDesde, fechaHasta);

                // Actualizar labels
                ActualizarTotales(resumen);

                // Mostrar información en el título
                this.Text = $"Ingresos y Egresos - {_movimientos.Count} movimientos encontrados";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}\n\nDetalles: {ex.InnerException?.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotales(ResumenMovimientosDTO resumen)
        {
            var fechaDesde = dtpFechaDesde.Value.Date;
            var fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1);
            var totalTrabajosAA = _gestorMovimientos.ObtenerTotalTrabajosAA(fechaDesde, fechaHasta);

            lblTotal.Text = $"Total Ventas: ${resumen.TotalVentas:N2}";
            lblTotal.ForeColor = Color.FromArgb(34, 139, 34); // Verde

            lblTotalCuotas.Text = $"Total Cuotas Pagadas: ${resumen.TotalCuotas:N2}";
            lblTotalCuotas.ForeColor = Color.FromArgb(34, 139, 34); // Verde

            lblTotalTrabajos.Text = $"Total Trabajos A/A: ${totalTrabajosAA:N2}";
            lblTotalTrabajos.ForeColor = Color.FromArgb(60, 179, 113); // Verde medio mar

            lblTotalPagado.Text = $"Total Egresos: ${resumen.TotalEgresos:N2}";
            lblTotalPagado.ForeColor = Color.FromArgb(220, 20, 60); // Rojo
        }
    }
}
