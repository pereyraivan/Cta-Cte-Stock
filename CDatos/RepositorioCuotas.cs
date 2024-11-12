using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioCuotas
    {
        public List<CuotaDTO> ObtenerCuotasPorVenta(int ventaId)
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                return (from c in db.Cuota
                        where c.VentaId == ventaId
                        select new CuotaDTO
                        {
                            CuotaId = c.CuotaId,
                            VentaID = c.VentaId,
                            NumeroDeCuota = c.NumeroDeCuota,
                            MontoCuota = c.MontoCuota,
                            FechaProgramada = c.FechaProgramada.ToString("dd/MM/yyyy"),
                            FechaPago = c.FechaPago.HasValue ? c.FechaPago.Value.ToString("dd/MM/yyyy") : "No pagada",
                            Estado = c.Estado ? "Pagada" : "Pendiente"
                        }).ToList();
            }
        }
    }
}
