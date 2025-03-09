using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorReportes
    {
        private RepositorioReportes _repositorioReporte = new RepositorioReportes();
      
        public List<ReciboDePago_Result> DatosReciboDePago(int IdVenta, int numeroCuota)
        {
            return _repositorioReporte.DatosReciboDePago(IdVenta, numeroCuota);
        }
        public List<ComprobanteDePago_Result> DatosComprobanteDeVenta(int IdVenta)
        {
            return _repositorioReporte.DatosComprobanteDeVenta(IdVenta);
        }
    }
}
