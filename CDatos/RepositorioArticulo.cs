using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioArticulo
    {
        private string respuesta = "";

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
                        articuloExistente.StockMinimo = articulo.StockMinimo;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
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
                        if (tipoMovimiento == "Entrada")
                            articulo.Stock += cantidad;
                        else if (tipoMovimiento == "Salida")
                            articulo.Stock -= cantidad;

                        // Registrar el movimiento
                        var movimiento = new MovimientoStock
                        {
                            ArticuloId = articuloId,
                            Fecha = DateTime.Now,
                            TipoMovimiento = tipoMovimiento,
                            Cantidad = cantidad
                        };
                        db.MovimientoStock.Add(movimiento);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }

        public List<Articulo> Listar()
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    articulos = db.Articulo.Where(x => x.FechaAnulacion == null).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return articulos;
        }

        public List<Articulo> BuscarPorCodigo(string codigo)
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    articulos = db.Articulo.Where(x => x.Codigo.Contains(codigo) && x.FechaAnulacion == null).ToList();
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
                    articulos = db.Articulo.Where(x => x.Descripcion.Contains(nombreArticulo) && x.FechaAnulacion == null).ToList();
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
                    articulo.FechaAnulacion = DateTime.Today;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }

    }
}
