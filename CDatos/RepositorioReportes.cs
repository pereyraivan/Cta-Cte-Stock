using CEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioReportes
    {
   //     public List<ComprobanteDePago_Result> ObtenerDatosReporteComprobanteVenta(int IdVenta)
   //     {
			//try
			//{
   //             using (VentasCredimaxEntities db = new VentasCredimaxEntities())
   //             {
   //                 return true;
   //             }

   //         }
			//catch (Exception)
			//{

			//	throw;
			//}
   //     }
        public List<ReciboDePago_Result> DatosReciboDePago(int IdVenta, int numeroCuota)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    return db.Database.SqlQuery<ReciboDePago_Result>(
                        "EXEC ReciboDePago @IdVenta,  @NumeroDeCuota",
                        new SqlParameter("@IdVenta", IdVenta),
                        new SqlParameter("@NumeroDeCuota", numeroCuota)
                        ).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del recibo de pago", ex);
            }
        }
        public List<ComprobanteDePago_Result> DatosComprobanteDeVenta(int IdVenta)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    return db.Database.SqlQuery<ComprobanteDePago_Result>(
                        "EXEC ComprobanteDePago @IdVenta",
                        new SqlParameter("@IdVenta", IdVenta)
                        ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del recibo de pago", ex);
            }
        }
    }
}
