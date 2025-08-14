using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDatos
{
    public class RepositorioMovimientos
    {
        public List<MovimientoDTO> ObtenerMovimientosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<MovimientoDTO> movimientos = new List<MovimientoDTO>();

            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Debug: verificar conexión
                    if (db == null)
                        throw new Exception("No se pudo establecer conexión con la base de datos");

                    // Obtener todas las ventas realizadas en el rango de fechas
                    var ventasData = (from v in db.Venta
                                     join c in db.Cliente on v.ClientId equals c.ClientId
                                     join a in db.Articulo on v.IdArticulo equals a.ArticuloId
                                     where v.FechaDeInicio >= fechaDesde &&
                                           v.FechaDeInicio <= fechaHasta
                                     select new
                                     {
                                         Fecha = v.FechaDeInicio,
                                         TotalVenta = v.Total,
                                         Descripcion = a.Descripcion,
                                         ClienteApellido = c.Apellido,
                                         ClienteNombre = c.Nombre,
                                         VentaId = v.VentaId
                                     }).ToList();

                    var ingresosVentas = ventasData.Select(v => new MovimientoDTO
                    {
                        Fecha = v.Fecha,
                        Tipo = "Ingreso",
                        Concepto = "Venta Realizada",
                        Descripcion = "Venta - " + v.Descripcion,
                        Monto = v.TotalVenta ?? 0,
                        Cliente = v.ClienteApellido + " " + v.ClienteNombre,
                        VentaId = v.VentaId,
                        CuotaId = null
                    }).ToList();

                    // Obtener ingresos por cuotas pagadas (consulta más flexible)
                    var cuotasData = (from cu in db.Cuota
                                     join v in db.Venta on cu.VentaId equals v.VentaId
                                     join c in db.Cliente on v.ClientId equals c.ClientId
                                     join a in db.Articulo on v.IdArticulo equals a.ArticuloId
                                     where cu.FechaPago.HasValue &&
                                           cu.FechaPago.Value >= fechaDesde &&
                                           cu.FechaPago.Value <= fechaHasta
                                     select new
                                     {
                                         Fecha = cu.FechaPago.Value,
                                         MontoCuota = cu.MontoCuota,
                                         NumeroDeCuota = cu.NumeroDeCuota,
                                         Descripcion = a.Descripcion,
                                         ClienteApellido = c.Apellido,
                                         ClienteNombre = c.Nombre,
                                         VentaId = v.VentaId,
                                         CuotaId = cu.CuotaId
                                     }).ToList();

                    var ingresosCuotas = cuotasData.Select(cu => new MovimientoDTO
                    {
                        Fecha = cu.Fecha,
                        Tipo = "Ingreso",
                        Concepto = "Pago de Cuota",
                        Descripcion = "Cuota " + cu.NumeroDeCuota.ToString() + " - " + cu.Descripcion,
                        Monto = cu.MontoCuota,
                        Cliente = cu.ClienteApellido + " " + cu.ClienteNombre,
                        VentaId = cu.VentaId,
                        CuotaId = cu.CuotaId
                    }).ToList();

                    // Obtener egresos por compras
                    var comprasData = (from comp in db.Compra
                                      join a in db.Articulo on comp.IdArticulo equals a.ArticuloId
                                      where comp.FechaCompra.HasValue &&
                                            comp.FechaCompra.Value >= fechaDesde &&
                                            comp.FechaCompra.Value <= fechaHasta
                                      select new
                                      {
                                          Fecha = comp.FechaCompra.Value,
                                          Precio = comp.Precio,
                                          Cantidad = comp.Cantidad,
                                          Descripcion = a.Descripcion,
                                          CompraId = comp.IdCompra
                                      }).ToList();

                    var egresosCompras = comprasData.Select(comp => new MovimientoDTO
                    {
                        Fecha = comp.Fecha,
                        Tipo = "Egreso",
                        Concepto = "Compra",
                        Descripcion = "Compra - " + comp.Descripcion + " (Cant: " + comp.Cantidad + ")",
                        Monto = comp.Precio * comp.Cantidad,
                        Cliente = "Proveedor",
                        VentaId = null,
                        CuotaId = null
                    }).ToList();

                    // Obtener ingresos por trabajos de aire acondicionado
                    var trabajosData = (from t in db.TrabajoAireAcondicionado
                                       where t.FechaTrabajo.HasValue &&
                                             t.FechaTrabajo.Value >= fechaDesde &&
                                             t.FechaTrabajo.Value <= fechaHasta
                                       select new
                                       {
                                           Fecha = t.FechaTrabajo.Value,
                                           Precio = t.Precio ?? 0,
                                           Descripcion = t.DescripcionTrabajo,
                                           Cliente = t.Cliente,
                                           TrabajoId = t.IdTrabajo
                                       }).ToList();

                    var ingresosTrabajos = trabajosData.Select(t => new MovimientoDTO
                    {
                        Fecha = t.Fecha,
                        Tipo = "Ingreso",
                        Concepto = "Trabajo A/A",
                        Descripcion = "Trabajo - " + t.Descripcion,
                        Monto = t.Precio,
                        Cliente = t.Cliente ?? "Cliente no especificado",
                        VentaId = null,
                        CuotaId = null
                    }).ToList();

                    // Combinar todos los movimientos
                    movimientos.AddRange(ingresosVentas);
                    movimientos.AddRange(ingresosCuotas);
                    movimientos.AddRange(ingresosTrabajos);
                    movimientos.AddRange(egresosCompras);

                    // Ordenar por fecha descendente
                    movimientos = movimientos.OrderByDescending(m => m.Fecha).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log del error si es necesario
                throw new Exception($"Error al obtener movimientos: {ex.Message}");
            }

            return movimientos;
        }

        // Método de prueba para verificar datos en la base
        public string ObtenerEstadisticasBaseDatos()
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var totalVentas = db.Venta.Where(v => v.FechaAnulacion == null).Count();
                    var ventasConAnticipo = db.Venta.Where(v => v.FechaAnulacion == null && v.Anticipo.HasValue && v.Anticipo > 0).Count();
                    var totalCuotas = db.Cuota.Count();
                    var cuotasPagadas = db.Cuota.Where(c => c.FechaPago.HasValue).Count();
                    
                    return $"Ventas totales: {totalVentas}, Ventas con anticipo: {ventasConAnticipo}, Cuotas totales: {totalCuotas}, Cuotas pagadas: {cuotasPagadas}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public decimal ObtenerTotalIngresosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Total de todas las ventas realizadas en el rango de fechas
                    var totalVentas = db.Venta
                        .Where(v => v.FechaDeInicio >= fechaDesde &&
                                   v.FechaDeInicio <= fechaHasta)
                        .Sum(v => v.Total) ?? 0;

                    return totalVentas;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerTotalTrabajosAA(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Total de trabajos de aire acondicionado en el rango de fechas
                    var totalTrabajos = db.TrabajoAireAcondicionado
                        .Where(t => t.FechaTrabajo.HasValue &&
                                   t.FechaTrabajo.Value >= fechaDesde &&
                                   t.FechaTrabajo.Value <= fechaHasta)
                        .Sum(t => t.Precio) ?? 0;

                    return totalTrabajos;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerTotalCuotasPagadas(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Total de cuotas pagadas en el rango de fechas
                    var totalCuotas = db.Cuota
                        .Where(c => c.FechaPago.HasValue &&
                                   c.FechaPago.Value >= fechaDesde &&
                                   c.FechaPago.Value <= fechaHasta)
                        .Sum(c => (decimal?)c.MontoCuota) ?? 0;

                    return totalCuotas;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal ObtenerTotalEgresosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Total de compras (egresos) en el rango de fechas
                    var totalCompras = db.Compra
                        .Where(c => c.FechaCompra.HasValue &&
                                   c.FechaCompra.Value >= fechaDesde &&
                                   c.FechaCompra.Value <= fechaHasta)
                        .Sum(c => (decimal?)(c.Precio * c.Cantidad)) ?? 0;

                    return totalCompras;
                }
            }
            catch
            {
                return 0;
            }
        }

        public ResumenMovimientosDTO ObtenerResumenPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Total de todas las ventas realizadas en el rango de fechas
                    var totalVentas = db.Venta
                        .Where(v => v.FechaDeInicio >= fechaDesde &&
                                   v.FechaDeInicio <= fechaHasta)
                        .Sum(v => v.Total) ?? 0;

                    // Total de cuotas pagadas
                    var totalCuotas = db.Cuota
                        .Where(c => c.FechaPago.HasValue &&
                                   c.FechaPago.Value >= fechaDesde &&
                                   c.FechaPago.Value <= fechaHasta)
                        .Sum(c => (decimal?)c.MontoCuota) ?? 0;

                    // Total de trabajos de aire acondicionado
                    var totalTrabajos = db.TrabajoAireAcondicionado
                        .Where(t => t.FechaTrabajo.HasValue &&
                                   t.FechaTrabajo.Value >= fechaDesde &&
                                   t.FechaTrabajo.Value <= fechaHasta)
                        .Sum(t => t.Precio) ?? 0;

                    // Total de egresos (compras)
                    var totalEgresos = db.Compra
                        .Where(c => c.FechaCompra.HasValue &&
                                   c.FechaCompra.Value >= fechaDesde &&
                                   c.FechaCompra.Value <= fechaHasta)
                        .Sum(c => (decimal?)(c.Precio * c.Cantidad)) ?? 0;

                    return new ResumenMovimientosDTO
                    {
                        TotalVentas = totalVentas,
                        TotalCuotas = totalCuotas,
                        TotalIngresos = totalVentas + totalCuotas + totalTrabajos,
                        TotalEgresos = totalEgresos,
                        Diferencia = (totalVentas + totalCuotas + totalTrabajos) - totalEgresos
                    };
                }
            }
            catch
            {
                return new ResumenMovimientosDTO();
            }
        }
    }
}
