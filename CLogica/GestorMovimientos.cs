using CDatos;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;

namespace CLogica
{
    public class GestorMovimientos
    {
        private RepositorioMovimientos _repositorio = new RepositorioMovimientos();

        public List<MovimientoDTO> ObtenerMovimientosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerMovimientosPorFecha(fechaDesde, fechaHasta);
        }

        public decimal ObtenerTotalIngresosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerTotalIngresosPorFecha(fechaDesde, fechaHasta);
        }

        public decimal ObtenerTotalCuotasPagadas(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerTotalCuotasPagadas(fechaDesde, fechaHasta);
        }

        public decimal ObtenerTotalTrabajosAA(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerTotalTrabajosAA(fechaDesde, fechaHasta);
        }

        public decimal ObtenerTotalEgresosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerTotalEgresosPorFecha(fechaDesde, fechaHasta);
        }

        public ResumenMovimientosDTO ObtenerResumenPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorio.ObtenerResumenPorFecha(fechaDesde, fechaHasta);
        }

        // Método de prueba
        public string ObtenerEstadisticasBaseDatos()
        {
            return _repositorio.ObtenerEstadisticasBaseDatos();
        }
    }
}
