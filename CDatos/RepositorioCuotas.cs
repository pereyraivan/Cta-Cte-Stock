using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDatos
{
    public class RepositorioCuotas
    {
        public List<CuotaDTO> ObtenerCuotasPorVenta(int ventaId)
        {
            using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
            {
                var cuotas = (from c in db.Cuota
                              join mp in db.MetodoDePago on c.IdMetodoDePago equals mp.IdMetodoPago into mpGroup
                              from mp in mpGroup.DefaultIfEmpty()
                              where c.VentaId == ventaId
                              select new
                              {
                                  c.CuotaId,
                                  c.VentaId,
                                  c.NumeroDeCuota,
                                  c.MontoCuota,
                                  c.FechaProgramada,
                                  c.FechaPago,
                                  c.Estado,
                                  c.IdMetodoDePago,
                                  MetodoDePago = mp != null ? mp.Descripcion : "No definido"
                              }).ToList();

                return cuotas.Select(c => new CuotaDTO
                {
                    CuotaId = c.CuotaId,
                    VentaID = c.VentaId,
                    NumeroDeCuota = c.NumeroDeCuota,
                    MontoCuota = c.MontoCuota,
                    FechaProgramada = c.FechaProgramada.ToString("dd/MM/yyyy"),
                    FechaPago = c.FechaPago.HasValue ? c.FechaPago.Value.ToString("dd/MM/yyyy") : "No pagada",
                    Estado = (c.Estado ?? false) ? "Pagada" : "Pendiente",
                    IdMetodoDePago = c.IdMetodoDePago,
                    MetodoDePago = c.MetodoDePago
                }).ToList();
            }
        }
        public bool RegistrarPago(int cuotaId)
        {
            return RegistrarPago(cuotaId, null);
        }

        public bool RegistrarPago(int cuotaId, int? idMetodoPago)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    Cuota cuota = db.Cuota.FirstOrDefault(c => c.CuotaId == cuotaId);
                    if (cuota != null)
                    {
                        cuota.Estado = true; // Marcar como pagada
                        cuota.FechaPago = DateTime.Now; // Registrar la fecha de pago
                        cuota.IdMetodoDePago = idMetodoPago; // Registrar el método de pago

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
            catch
            {
                return false;         
            }
        }

        public decimal ObtenerTotalCuotasPagadasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var totalCuotasPagadas = db.Cuota
                        .Where(c => c.FechaPago.HasValue && 
                                   c.FechaPago.Value >= fechaDesde && 
                                   c.FechaPago.Value <= fechaHasta &&
                                   c.Estado == true)
                        .Sum(c => (decimal?)c.MontoCuota) ?? 0;

                    return totalCuotasPagadas;
                }
            }
            catch
            {
                return 0;
            }
        }

        public List<MetodoDePago> ObtenerMetodosDePago()
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    return db.MetodoDePago.ToList();
                }
            }
            catch
            {
                return new List<MetodoDePago>();
            }
        }
    }
}
