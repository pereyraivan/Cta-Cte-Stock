using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLogica
{
    public class GestorReportes
    {
        private RepositorioReportes _repositorioReporte = new RepositorioReportes();
        private GestorAireAcondicionado _gestorAireAcondicionado = new GestorAireAcondicionado();

        public List<ReciboDePago_Result> DatosReciboDePago(int IdVenta, int numeroCuota)
        {
            return _repositorioReporte.DatosReciboDePago(IdVenta, numeroCuota);
        }
        public List<ComprobanteDePago_Result> DatosComprobanteDeVenta(int IdVenta)
        {
            return _repositorioReporte.DatosComprobanteDeVenta(IdVenta);
        }
        public List<sp_GetVentasByClientId_Result> DatosVentasPorCliente(int IdCliente)
        {
            return _repositorioReporte.DatosVentasPorCliente(IdCliente);
        }
        public List<sp_ReporteVentas_Result> DatosVentasPorFecha(DateTime fechaDesde,DateTime fechaHasta)
        {
            return _repositorioReporte.DatosVentasPorFecha(fechaDesde,fechaHasta);
        }
        public List<sp_DetalleDeVenta_Result> DatosDetalleDeVenta(int IdVenta)
        {
            return _repositorioReporte.DatosDetalleDeVenta(IdVenta);
        }

        /// <summary>
        /// Calcula el total de ingresos (ventas + trabajos) por fecha
        /// </summary>
        public decimal CalcularTotalIngresosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                decimal totalVentas = 0;
                decimal totalTrabajos = 0;

                // Sumar ventas
                var ventas = DatosVentasPorFecha(fechaDesde, fechaHasta);
                totalVentas = ventas.Sum(v => v.Total ?? 0);

                // Sumar trabajos de aire acondicionado
                var trabajos = _gestorAireAcondicionado.ListarTrabajosPorFecha(fechaDesde, fechaHasta);
                totalTrabajos = trabajos.Sum(t => t.Precio ?? 0);

                return totalVentas + totalTrabajos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular total de ingresos", ex);
            }
        }
    }
}
