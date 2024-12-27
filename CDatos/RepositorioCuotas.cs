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
                var cuotas = (from c in db.Cuota
                              where c.VentaId == ventaId
                              select new
                              {
                                  c.CuotaId,
                                  c.VentaId,
                                  c.NumeroDeCuota,
                                  c.MontoCuota,
                                  c.FechaProgramada,
                                  c.FechaPago,
                                  c.Estado
                              }).ToList();

                return cuotas.Select(c => new CuotaDTO
                {
                    CuotaId = c.CuotaId,
                    VentaID = c.VentaId,
                    NumeroDeCuota = c.NumeroDeCuota,
                    MontoCuota = c.MontoCuota,
                    FechaProgramada = c.FechaProgramada.ToString("dd/MM/yyyy"),
                    FechaPago = c.FechaPago.HasValue ? c.FechaPago.Value.ToString("dd/MM/yyyy") : "No pagada",
                    Estado = (c.Estado ?? false) ? "Pagada" : "Pendiente"
                }).ToList();
            }
        }
        public bool RegistrarPago(int cuotaId)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    Cuota cuota = db.Cuota.FirstOrDefault(c => c.CuotaId == cuotaId);
                    if (cuota != null)
                    {
                        cuota.Estado = true; // Marcar como pagada
                        cuota.FechaPago = DateTime.Now; // Registrar la fecha de pago

                        db.SaveChanges();

                        int ventaId = cuota.VentaId;
                        bool todasCuotasPagadas = db.Cuota
                            .Where(c => c.VentaId == ventaId)
                            .All(c => c.Estado == true); // Todas las cuotas tienen Estado = true (pagadas)

                        if (todasCuotasPagadas)
                        {
                            // Buscar la venta asociada y establecer la fecha de anulación
                            Venta venta = db.Venta.FirstOrDefault(v => v.VentaId == ventaId);
                            if (venta != null)
                            {
                                venta.FechaAnulacion = cuota.FechaPago; // Fecha de anulación con la fecha del último pago
                                db.SaveChanges();
                            }
                        }
                    }

                   
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
