using CEntidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioArticulo
    {
        private string respuesta = "";

        public string GetUltimoError()
        {
            return respuesta;
        }

        public void Guardar(Articulo articulo)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    articulo.FechaAlta = DateTime.Now;
                    db.Articulo.Add(articulo);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al guardar el artículo: {e.Message}", e);
            }
        }

        public void Modificar(Articulo articulo)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var articuloExistente = db.Articulo.FirstOrDefault(x => x.ArticuloId == articulo.ArticuloId);
                    if (articuloExistente != null)
                    {
                        articuloExistente.Codigo = articulo.Codigo;
                        articuloExistente.Descripcion = articulo.Descripcion;
                        articuloExistente.PrecioCompra = articulo.PrecioCompra;
                        articuloExistente.PrecioVenta = articulo.PrecioVenta;
                        articuloExistente.Stock = articulo.Stock;
                        articuloExistente.StockMinimo = articulo.StockMinimo;
                        articuloExistente.IdMarca = articulo.IdMarca;
                        articuloExistente.IdMedida = articulo.IdMedida;
                        articuloExistente.IdTipoConector = articulo.IdTipoConector;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("El artículo no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al modificar el artículo: {e.Message}", e);
            }
        }

        public void ActualizarStock(int articuloId, int cantidad, string tipoMovimiento)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var articulo = db.Articulo.FirstOrDefault(x => x.ArticuloId == articuloId);
                    if (articulo != null)
                    {
                        int stockAnterior = articulo.Stock;
                        
                        if (tipoMovimiento == "Entrada")
                            articulo.Stock += cantidad;
                        else if (tipoMovimiento == "Salida")
                            articulo.Stock -= cantidad;

                        // Registrar el movimiento
                        var movimiento = new MovimientoStock
                        {
                            ArticuloId = articuloId,
                            Fecha = DateTime.Now,
                            StockAnterior = stockAnterior,
                            Cantidad = cantidad,
                            StockNuevo = articulo.Stock,
                            Observaciones = $"Movimiento: {tipoMovimiento}"
                        };
                        db.MovimientoStock.Add(movimiento);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al actualizar el stock: {e.Message}", e);
            }
        }

        public List<Articulo> Listar()
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Deshabilitar lazy loading para evitar problemas con ObjectContext dispuesto
                    db.Configuration.LazyLoadingEnabled = false;
                    
                    // Obtener artículos con consulta simple
                    articulos = db.Articulo
                        .Where(x => x.FechaAnulacion == null)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulos;
        }

        public List<dynamic> ListarConDetalles()
        {
            List<dynamic> articulosConDetalles = new List<dynamic>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // LINQ con joins directos - mucho más simple
                    var query = from a in db.Articulo
                               join m in db.Marca on a.IdMarca equals m.IdMarca into marcaJoin
                               from marca in marcaJoin.DefaultIfEmpty()
                               join med in db.Medida on a.IdMedida equals med.IdMedida into medidaJoin
                               from medida in medidaJoin.DefaultIfEmpty()
                               join tc in db.TipoConector on a.IdTipoConector equals tc.IdTipoConector into tipoJoin
                               from tipoConector in tipoJoin.DefaultIfEmpty()
                               where a.FechaAnulacion == null
                               select new
                               {
                                   a.ArticuloId,
                                   a.Codigo,
                                   a.Descripcion,
                                   Marca = marca != null ? marca.NombreMarca : "Sin marca",
                                   Medida = medida != null ? medida.NombreMedida : "Sin medida",
                                   TipoConector = tipoConector != null ? tipoConector.NombreTipoConector : "Sin tipo",
                                   a.PrecioCompra,
                                   a.PrecioVenta,
                                   a.Stock,
                                   a.StockMinimo,
                                   a.FechaAlta
                               };

                    articulosConDetalles = query.ToList<dynamic>();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulosConDetalles;
        }

        public List<Articulo> BuscarPorCodigo(string codigo)
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Deshabilitar lazy loading para evitar problemas con ObjectContext dispuesto
                    db.Configuration.LazyLoadingEnabled = false;
                    
                    articulos = db.Articulo
                        .Where(x => x.Codigo.Contains(codigo) && x.FechaAnulacion == null)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulos;
        }
        public List<Articulo> BuscarPorNombre(string nombreArticulo)
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Deshabilitar lazy loading para evitar problemas con ObjectContext dispuesto
                    db.Configuration.LazyLoadingEnabled = false;
                    
                    articulos = db.Articulo
                        .Where(x => x.Descripcion.Contains(nombreArticulo) && x.FechaAnulacion == null)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulos;
        }
        public void Eliminar(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var articulo = db.Articulo.FirstOrDefault(x => x.ArticuloId == id);
                    if (articulo != null)
                    {
                        articulo.FechaAnulacion = DateTime.Today;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("El artículo no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al eliminar el artículo: {e.Message}", e);
            }
        }

        public bool ExisteCodigo(string codigo, int? articuloIdExcluir = null)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var query = db.Articulo.Where(x => x.Codigo == codigo && x.FechaAnulacion == null);
                    
                    if (articuloIdExcluir.HasValue)
                    {
                        query = query.Where(x => x.ArticuloId != articuloIdExcluir.Value);
                    }
                    
                    return query.Any();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return false;
            }
        }

        public bool ExisteCodigoCompleto(string codigo, int? articuloIdExcluir = null)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var query = db.Articulo.Where(x => x.Codigo == codigo);
                    
                    if (articuloIdExcluir.HasValue)
                    {
                        query = query.Where(x => x.ArticuloId != articuloIdExcluir.Value);
                    }
                    
                    return query.Any();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return false;
            }
        }

        public List<dynamic> BuscarConDetalles(string textoBusqueda)
        {
            List<dynamic> articulosConDetalles = new List<dynamic>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // LINQ con joins directos para búsqueda
                    var query = from a in db.Articulo
                               join m in db.Marca on a.IdMarca equals m.IdMarca into marcaJoin
                               from marca in marcaJoin.DefaultIfEmpty()
                               join med in db.Medida on a.IdMedida equals med.IdMedida into medidaJoin
                               from medida in medidaJoin.DefaultIfEmpty()
                               join tc in db.TipoConector on a.IdTipoConector equals tc.IdTipoConector into tipoJoin
                               from tipoConector in tipoJoin.DefaultIfEmpty()
                               where a.FechaAnulacion == null && 
                                     (a.Codigo.Contains(textoBusqueda) || a.Descripcion.Contains(textoBusqueda))
                               select new
                               {
                                   a.ArticuloId,
                                   a.Codigo,
                                   a.Descripcion,
                                   Marca = marca != null ? marca.NombreMarca : "Sin marca",
                                   Medida = medida != null ? medida.NombreMedida : "Sin medida",
                                   TipoConector = tipoConector != null ? tipoConector.NombreTipoConector : "Sin tipo",
                                   a.PrecioCompra,
                                   a.PrecioVenta,
                                   a.Stock,
                                   a.StockMinimo,
                                   a.FechaAlta
                               };

                    articulosConDetalles = query.ToList<dynamic>();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulosConDetalles;
        }

        public string ObtenerProximoCodigo()
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    // Buscar el último código numérico
                    var ultimoCodigo = db.Articulo
                        .Where(x => x.Codigo != null)
                        .AsEnumerable() // Ejecutar en memoria para usar funciones de .NET
                        .Where(x => x.Codigo.All(char.IsDigit)) // Solo códigos numéricos
                        .Select(x => int.TryParse(x.Codigo, out int codigo) ? codigo : 0)
                        .DefaultIfEmpty(0)
                        .Max();

                    // Generar el próximo código
                    int proximoCodigo = ultimoCodigo + 1;
                    return proximoCodigo.ToString(); // Sin formato, comienza desde 1
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return "1"; // Código por defecto en caso de error
            }
        }

    }
}
