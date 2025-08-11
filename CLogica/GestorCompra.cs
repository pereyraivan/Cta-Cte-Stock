using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorCompra
    {
        private RepositorioCompra repositorio;

        public GestorCompra()
        {
            repositorio = new RepositorioCompra();
        }

        public string GuardarCompra(int idArticulo, decimal precio, int cantidad, DateTime fechaCompra)
        {
            try
            {
                if (idArticulo <= 0)
                    return "Debe seleccionar un artículo válido";

                if (precio <= 0)
                    return "El precio debe ser mayor a 0";

                if (cantidad <= 0)
                    return "La cantidad debe ser mayor a 0";

                var compra = new Compra
                {
                    IdArticulo = idArticulo,
                    Precio = precio,
                    Cantidad = cantidad,
                    FechaCompra = fechaCompra
                };

                repositorio.Guardar(compra);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"Error al guardar la compra: {ex.Message}";
            }
        }

        public string ModificarCompra(int idCompra, int idArticulo, decimal precio, int cantidad, DateTime fechaCompra)
        {
            try
            {
                if (idCompra <= 0)
                    return "ID de compra inválido";

                if (idArticulo <= 0)
                    return "Debe seleccionar un artículo válido";

                if (precio <= 0)
                    return "El precio debe ser mayor a 0";

                if (cantidad <= 0)
                    return "La cantidad debe ser mayor a 0";

                var compra = new Compra
                {
                    IdCompra = idCompra,
                    IdArticulo = idArticulo,
                    Precio = precio,
                    Cantidad = cantidad,
                    FechaCompra = fechaCompra
                };

                repositorio.Modificar(compra);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"Error al modificar la compra: {ex.Message}";
            }
        }

        public string EliminarCompra(int idCompra)
        {
            try
            {
                if (idCompra <= 0)
                    return "ID de compra inválido";

                repositorio.Eliminar(idCompra);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar la compra: {ex.Message}";
            }
        }

        public List<dynamic> ListarComprasConArticulos()
        {
            try
            {
                return repositorio.ListarComprasConArticulos();
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }

        public List<dynamic> BuscarComprasPorArticulo(string nombreArticulo)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreArticulo))
                    return ListarComprasConArticulos();

                return repositorio.BuscarComprasPorArticulo(nombreArticulo);
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }

        public Compra ObtenerCompraPorId(int idCompra)
        {
            try
            {
                return repositorio.ObtenerPorId(idCompra);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetUltimoError()
        {
            return repositorio.GetUltimoError();
        }
    }
}
