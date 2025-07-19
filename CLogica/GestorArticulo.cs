using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorArticulo
    {
        private RepositorioArticulo _repositorioArticulo = new RepositorioArticulo();

        public void Guardar(Articulo articulo)
        {
            // Validaciones de negocio
            if (string.IsNullOrEmpty(articulo.Codigo))
                throw new Exception("El código es requerido");

            if (string.IsNullOrEmpty(articulo.Descripcion))
                throw new Exception("La descripción es requerida");

            if (articulo.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a 0");

            _repositorioArticulo.Guardar(articulo);
        }

        public void Modificar(Articulo articulo)
        {
            // Validaciones de negocio
            if (string.IsNullOrEmpty(articulo.Codigo))
                throw new Exception("El código es requerido");

            if (string.IsNullOrEmpty(articulo.Descripcion))
                throw new Exception("La descripción es requerida");

            if (articulo.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a 0");

            _repositorioArticulo.Modificar(articulo);
        }

        public void ActualizarStock(int articuloId, int cantidad, string tipoMovimiento)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            if (tipoMovimiento != "Entrada" && tipoMovimiento != "Salida")
                throw new Exception("Tipo de movimiento inválido");

            _repositorioArticulo.ActualizarStock(articuloId, cantidad, tipoMovimiento);
        }

        public List<Articulo> Listar()
        {
            return _repositorioArticulo.Listar();
        }

        public List<Articulo> BuscarPorCodigo(string codigo)
        {
            return _repositorioArticulo.BuscarPorCodigo(codigo);
        }
        public List<Articulo> BuscarPorNombre(string descripcion)
        {
            return _repositorioArticulo.BuscarPorNombre(descripcion);
        }

        public bool ValidarStock(int articuloId, int cantidadRequerida)
        {
            var articulo = _repositorioArticulo.Listar().FirstOrDefault(x => x.ArticuloId == articuloId);
            if (articulo == null)
                return false;

            return articulo.Stock >= cantidadRequerida;
        }
        public void Eliminar(int id)
        {
            _repositorioArticulo.Eliminar(id);
        }
    }
}
