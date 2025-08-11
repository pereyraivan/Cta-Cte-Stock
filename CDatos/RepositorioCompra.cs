using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioCompra
    {
        private string respuesta = "";

        public string GetUltimoError()
        {
            return respuesta;
        }

        public void Guardar(Compra compra)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    db.Compra.Add(compra);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al guardar la compra: {e.Message}", e);
            }
        }

        public void Modificar(Compra compra)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var compraExistente = db.Compra.FirstOrDefault(x => x.IdCompra == compra.IdCompra);
                    if (compraExistente != null)
                    {
                        compraExistente.IdArticulo = compra.IdArticulo;
                        compraExistente.Precio = compra.Precio;
                        compraExistente.Cantidad = compra.Cantidad;
                        compraExistente.FechaCompra = compra.FechaCompra;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("La compra no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al modificar la compra: {e.Message}", e);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var compra = db.Compra.FirstOrDefault(x => x.IdCompra == id);
                    if (compra != null)
                    {
                        db.Compra.Remove(compra);
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("La compra no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al eliminar la compra: {e.Message}", e);
            }
        }

        public List<Compra> Listar()
        {
            List<Compra> compras = new List<Compra>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    compras = db.Compra.OrderByDescending(x => x.FechaCompra).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return compras;
        }

        public Compra ObtenerPorId(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    return db.Compra.FirstOrDefault(x => x.IdCompra == id);
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return null;
            }
        }

        // Método para obtener compras con información del artículo
        public List<dynamic> ListarComprasConArticulos()
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var comprasConArticulos = (from c in db.Compra
                                             join a in db.Articulo on c.IdArticulo equals a.ArticuloId
                                             orderby c.FechaCompra descending
                                             select new
                                             {
                                                 IdCompra = c.IdCompra,
                                                 IdArticulo = c.IdArticulo,
                                                 NombreArticulo = a.Descripcion,
                                                 CodigoArticulo = a.Codigo,
                                                 Precio = c.Precio,
                                                 Cantidad = c.Cantidad,
                                                 Total = c.Precio * c.Cantidad,
                                                 FechaCompra = c.FechaCompra
                                             }).ToList();

                    return comprasConArticulos.Cast<dynamic>().ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return new List<dynamic>();
            }
        }

        // Método para buscar compras por nombre de artículo
        public List<dynamic> BuscarComprasPorArticulo(string nombreArticulo)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var comprasEncontradas = (from c in db.Compra
                                            join a in db.Articulo on c.IdArticulo equals a.ArticuloId
                                            where a.Descripcion.Contains(nombreArticulo)
                                            orderby c.FechaCompra descending
                                            select new
                                            {
                                                IdCompra = c.IdCompra,
                                                IdArticulo = c.IdArticulo,
                                                NombreArticulo = a.Descripcion,
                                                CodigoArticulo = a.Codigo,
                                                Precio = c.Precio,
                                                Cantidad = c.Cantidad,
                                                Total = c.Precio * c.Cantidad,
                                                FechaCompra = c.FechaCompra
                                            }).ToList();

                    return comprasEncontradas.Cast<dynamic>().ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return new List<dynamic>();
            }
        }
    }
}
