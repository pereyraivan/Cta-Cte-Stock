using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioPago
    {
        public string RegistrarPago(Pago pago)
        {
            string respuesta = "";
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    db.Pago.Add(pago);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return respuesta;
        }

        public Cuota ObtenerCuotaPorId(int cuotaId)
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                return db.Cuota.Find(cuotaId);
            }
        }

        public void ActualizarCuota(Cuota cuota)
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                db.Entry(cuota).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Cuota> ObtenerCuotasPorVenta(int ventaId)
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                return db.Cuota.Where(c => c.VentaId == ventaId).ToList();
            }
        }
    }
}
