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
    }
}
