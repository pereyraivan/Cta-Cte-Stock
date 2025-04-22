using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioVenta
    {
        private string respuesta = "";
        public void RegistrarVenta(Venta venta)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    db.Venta.Add(venta);
                    db.SaveChanges();

                    if(venta.Cuotas > 0)
                    {
                        decimal montoPorCuota = (decimal)(venta.Precio / venta.Cuotas);
                        List<Cuota> cuotas = new List<Cuota>();
                        DateTime fechaVencimiento = (DateTime)venta.FechaDeInicio;

                        // Determinar el incremento de la fecha basado en el FormaDePagoId
                        int incrementoDias = 0;
                        switch (venta.FormaDePagoId)
                        {
                            case 1: // Mensual
                                incrementoDias = 30; 
                                break;
                            case 2: // Quincenal
                                incrementoDias = 15;
                                break;
                            case 3: // Semanal
                                incrementoDias = 7;
                                break;
                            default:
                                incrementoDias = 0; 
                                break;
                        }

                        for (int i = 1; i <= venta.Cuotas; i++)
                        {
                            // Calcular la fecha de vencimiento según la frecuencia
                            if (venta.FormaDePagoId == 1) // Mensual
                                fechaVencimiento = venta.FechaDeInicio.AddMonths(i);
                            else
                                fechaVencimiento = venta.FechaDeInicio.AddDays(incrementoDias * i);

                            Cuota nuevaCuota = new Cuota
                            {
                                VentaId = venta.VentaId,
                                MontoCuota = montoPorCuota,
                                NumeroDeCuota = i,
                                FechaProgramada = fechaVencimiento,
                                Estado = false
                            };

                            cuotas.Add(nuevaCuota);
                        }

                        // Agregar las cuotas a la base de datos
                        db.Cuota.AddRange(cuotas);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }
        public List<VentaDTO> ListarVentas(string criterioOrden, bool mostrarTodas)
        {
            List<VentaDTO> ventas = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    ventas = (from v in db.Venta
                              join c in db.Cliente on v.ClientId equals c.ClientId
                              join fp in db.FormaDePago on v.FormaDePagoId equals fp.FormaDePagoId
                              where mostrarTodas || v.FechaAnulacion == null
                              orderby v.FechaDeInicio descending
                              select new VentaDTO
                              {
                                  VentaId = v.VentaId,
                                  IdCliente = c.ClientId,
                                  NombreCliente = c.Apellido+ " " + c.Nombre, // Nombre del cliente
                                  Articulo = v.Articulo,
                                  Talle = v.Talle,
                                  FormaDePago = fp.Nombre,  // Nombre de la forma de pago
                                  Precio = v.Precio,
                                  Cuotas = v.Cuotas,
                                  FechaDeInicio = v.FechaDeInicio,
                                  FechaDeCancelacion = v.FechaDeCancelacion,
                                  Cantidad = v.Cantidad,
                                  Total = v.Total,
                                  FechaAnulacion = v.FechaAnulacion,
                                  CuotasVencidas = db.Cuota.Any(cuota => cuota.VentaId == v.VentaId && cuota.FechaProgramada < DateTime.Now && cuota.FechaPago == null)
                              }).ToList();

                    switch (criterioOrden.Trim())
                    {
                        case "Cuotas Vencidas":
                            ventas = ventas
                                .OrderByDescending(v => v.CuotasVencidas)
                                .ThenByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;

                        case "Fecha":
                            ventas = ventas
                                .OrderByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;

                        default:
                            ventas = ventas
                                .OrderByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                respuesta = ex.Message;
            }
            return ventas;
        }
        public List<VentaDTO> ListarVentasMenu(string criterioOrden)
        {
            List<VentaDTO> ventas = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    ventas = (from v in db.Venta
                              join c in db.Cliente on v.ClientId equals c.ClientId
                              join fp in db.FormaDePago on v.FormaDePagoId equals fp.FormaDePagoId
                              where v.FechaAnulacion == null
                              select new VentaDTO
                              {
                                  VentaId = v.VentaId,
                                  IdCliente = c.ClientId,
                                  NombreCliente = c.Apellido + " " + c.Nombre,
                                  Articulo = v.Articulo,
                                  Talle = v.Talle,
                                  FormaDePago = fp.Nombre,
                                  Precio = v.Precio,
                                  Cuotas = v.Cuotas,
                                  FechaDeInicio = v.FechaDeInicio,
                                  FechaDeCancelacion = v.FechaDeCancelacion,
                                  FechaAnulacion = v.FechaAnulacion,
                                  Cantidad = v.Cantidad,
                                  Total = v.Total,
                                  CuotasVencidas = db.Cuota.Any(cuota => cuota.VentaId == v.VentaId && cuota.FechaProgramada < DateTime.Now && cuota.FechaPago == null)
                              })
                              .ToList();
                    switch (criterioOrden.Trim())
                    {
                        case "Cuotas Vencidas":
                            ventas = ventas
                                .OrderByDescending(v => v.CuotasVencidas)
                                .ThenByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;

                        case "Fecha":
                            ventas = ventas
                                .OrderByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;

                        default:
                            ventas = ventas
                                .OrderByDescending(v => v.FechaDeInicio)
                                .ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                respuesta = ex.Message;
            }
            return ventas;
        }
        public List<VentaDTO> FiltrarVentasPorCliente(string nombreApellidoCliente)
        {
            List<VentaDTO> ventas = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    ventas = (from v in db.Venta
                              join c in db.Cliente on v.ClientId equals c.ClientId
                              join fp in db.FormaDePago on v.FormaDePagoId equals fp.FormaDePagoId
                              where (c.Apellido + " " + c.Nombre).Contains(nombreApellidoCliente) // Filtrar por nombre y apellido
                              orderby v.FechaDeInicio descending
                              select new VentaDTO
                              {
                                  VentaId = v.VentaId,
                                  IdCliente = c.ClientId,
                                  NombreCliente = c.Apellido + " " + c.Nombre,
                                  Articulo = v.Articulo,
                                  Talle = v.Talle,
                                  FormaDePago = fp.Nombre,
                                  Precio = v.Precio,
                                  Cuotas = v.Cuotas,
                                  FechaDeInicio = v.FechaDeInicio,
                                  FechaDeCancelacion = v.FechaDeCancelacion,
                                  Cantidad = v.Cantidad,
                                  Total = v.Total,
                                  FechaAnulacion = v.FechaAnulacion
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                respuesta = ex.Message;
            }
            return ventas;
        }
        public List<VentaDTO> FiltrarVentasPorArticulo(string nombreArticulo)
        {
            List<VentaDTO> ventas = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    ventas = (from v in db.Venta
                              join c in db.Cliente on v.ClientId equals c.ClientId
                              join fp in db.FormaDePago on v.FormaDePagoId equals fp.FormaDePagoId
                              where v.Articulo.Contains(nombreArticulo) // Filtrar por nombre del artículo
                              orderby v.FechaDeInicio descending
                              select new VentaDTO
                              {
                                  VentaId = v.VentaId,
                                  IdCliente = c.ClientId,
                                  NombreCliente = c.Apellido + " " + c.Nombre,
                                  Articulo = v.Articulo,
                                  Talle = v.Talle,
                                  FormaDePago = fp.Nombre,
                                  Precio = v.Precio,
                                  Cuotas = v.Cuotas,
                                  FechaDeInicio = v.FechaDeInicio,
                                  FechaDeCancelacion = v.FechaDeCancelacion,
                                  Cantidad = v.Cantidad,
                                  Total = v.Total,
                                  FechaAnulacion = v.FechaAnulacion
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Manejar el error
                respuesta = ex.Message;
            }
            return ventas;
        }
        public void ModificarVenta(Venta venta)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    var editarVenta = db.Venta.FirstOrDefault(x => x.VentaId == venta.VentaId);
                    // Comparar los valores relevantes
                    bool editarTablaCuotas = editarVenta.Cuotas != venta.Cuotas
                                             || editarVenta.Precio != venta.Precio
                                             || editarVenta.Total != venta.Total
                                             || editarVenta.FormaDePagoId != venta.FormaDePagoId;
                    // Eliminar cuotas existentes solo si no hay cuotas pagadas
                    bool tieneCuotaPagada = db.Cuota.Any(c => c.VentaId == venta.VentaId && c.Estado == true);

                    if (tieneCuotaPagada)
                    {
                        throw new InvalidOperationException("No se puede modificar la venta porque tiene una o varias cuotas pagadas.");
                    }

                    if (editarTablaCuotas)
                    {
                       
                        // Eliminar cuotas existentes
                        var cuotasExistentes = db.Cuota.Where(c => c.VentaId == venta.VentaId);
                        db.Cuota.RemoveRange(cuotasExistentes);

                        // Recalcular y agregar nuevas cuotas
                        decimal montoPorCuota = (decimal)(venta.Total / venta.Cuotas);
                        List<Cuota> nuevasCuotas = new List<Cuota>();
                        DateTime fechaVencimiento = (DateTime)venta.FechaDeInicio;

                        int incrementoDias = 0;
                        switch (venta.FormaDePagoId)
                        {
                            case 1: // Mensual
                                incrementoDias = 30;
                                break;
                            case 2: // Quincenal
                                incrementoDias = 15;
                                break;
                            case 3: // Semanal
                                incrementoDias = 7;
                                break;
                            default:
                                incrementoDias = 0;
                                break;
                        }

                        for (int i = 1; i <= venta.Cuotas; i++)
                        {
                            // Calcular la fecha de vencimiento según la frecuencia
                            if (venta.FormaDePagoId == 1) // Mensual
                                fechaVencimiento = venta.FechaDeInicio.AddMonths(i);
                            else
                                fechaVencimiento = venta.FechaDeInicio.AddDays(incrementoDias * i);

                            Cuota nuevaCuota = new Cuota
                            {
                                VentaId = venta.VentaId,
                                MontoCuota = montoPorCuota,
                                NumeroDeCuota = i,
                                FechaProgramada = fechaVencimiento,
                                Estado = false
                            };
                            nuevasCuotas.Add(nuevaCuota);
                        }
                        // Agregar las cuotas a la base de datos
                        db.Cuota.AddRange(nuevasCuotas);
                        db.SaveChanges();

                    }

                    //var editarVenta = db.Venta.FirstOrDefault(x => x.VentaId == venta.VentaId);
                    editarVenta.ClientId = venta.ClientId;
                    editarVenta.Articulo = venta.Articulo;
                    editarVenta.Talle = venta.Talle;
                    editarVenta.FormaDePagoId = venta.FormaDePagoId;
                    editarVenta.FechaDeInicio = venta.FechaDeInicio;
                    editarVenta.FechaDeCancelacion = venta.FechaDeCancelacion;
                    editarVenta.Precio = venta.Precio;
                    editarVenta.Cuotas = venta.Cuotas;
                    editarVenta.Cantidad = venta.Cantidad;
                    editarVenta.Total = venta.Total;
             
                    db.SaveChanges();
                }
            }
            catch(InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }
        public void EliminarVenta(int id)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    EliminarCuotasPorIdVenta(id);
                    var venta = db.Venta.FirstOrDefault(x => x.VentaId == id);
                    venta.FechaAnulacion = DateTime.Today;
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {

                respuesta = e.Message;
            }
        }
        public void EliminarCuotasPorIdVenta(int ventaId)
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                try
                {
                    var cuotas = db.Cuota.Where(c => c.VentaId == ventaId).ToList();
                    foreach (var c in cuotas)
                    {
                        c.Estado = true;
                    }
                    db.SaveChanges() ;
                }
                catch(Exception e)
                {

                }
               
            }         
        }
        public bool EsDemo()
        {
            
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                   
                   return db.Configuracion.Select(x => x.isDemo).FirstOrDefault();
                  
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return false;
            }
        }
    }
}
