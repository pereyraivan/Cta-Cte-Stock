using CDatos;
using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorCuotas
    {
        private RepositorioCuotas _repositorioCuotas = new RepositorioCuotas();

        public List<CuotaDTO> ObtenerCuotasPorVenta(int ventaId)
        {
            return _repositorioCuotas.ObtenerCuotasPorVenta(ventaId);
        }
        public bool RegistrarPago(int cuotaId)
        {
            return _repositorioCuotas.RegistrarPago(cuotaId);
        }

        public bool RegistrarPago(int cuotaId, int idMetodoPago)
        {
            return _repositorioCuotas.RegistrarPago(cuotaId, idMetodoPago);
        }

        public decimal ObtenerTotalCuotasPagadasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorioCuotas.ObtenerTotalCuotasPagadasPorFecha(fechaDesde, fechaHasta);
        }

        public List<MetodoDePago> ObtenerMetodosDePago()
        {
            return _repositorioCuotas.ObtenerMetodosDePago();
        }
    }
}
