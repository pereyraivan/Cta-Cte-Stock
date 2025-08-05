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
            // Generar código automáticamente si está vacío
            if (string.IsNullOrEmpty(articulo.Codigo))
            {
                articulo.Codigo = ObtenerProximoCodigo();
            }

            // Validaciones de negocio
            if (string.IsNullOrEmpty(articulo.Codigo))
                throw new Exception("El código es requerido");

            if (string.IsNullOrEmpty(articulo.Descripcion))
                throw new Exception("La descripción es requerida");

            if (articulo.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a 0");

            // Verificar que el código no exista (incluyendo anulados, por restricción de BD)
            if (_repositorioArticulo.ExisteCodigoCompleto(articulo.Codigo))
                throw new Exception($"Ya existe un artículo con el código '{articulo.Codigo}' (puede estar anulado)");

            // Validar duplicados por descripción, marca, medida y tipo conector
            if (ExisteArticuloDuplicado(articulo.Descripcion, articulo.IdMarca, articulo.IdMedida, articulo.IdTipoConector))
                throw new Exception("Ya existe un artículo con la misma combinación de descripción, marca, medida y tipo de conector");

            _repositorioArticulo.Guardar(articulo);
        }

        public void Modificar(Articulo articulo)
        {
            // Generar código automáticamente si está vacío
            if (string.IsNullOrEmpty(articulo.Codigo))
            {
                articulo.Codigo = ObtenerProximoCodigo();
            }

            // Validaciones de negocio
            if (string.IsNullOrEmpty(articulo.Codigo))
                throw new Exception("El código es requerido");

            if (string.IsNullOrEmpty(articulo.Descripcion))
                throw new Exception("La descripción es requerida");

            if (articulo.PrecioVenta <= 0)
                throw new Exception("El precio de venta debe ser mayor a 0");

            // Verificar que el código no exista en otros artículos (incluyendo anulados)
            if (_repositorioArticulo.ExisteCodigoCompleto(articulo.Codigo, articulo.ArticuloId))
                throw new Exception($"Ya existe otro artículo con el código '{articulo.Codigo}' (puede estar anulado)");

            // Validar duplicados por descripción, marca, medida y tipo conector
            if (ExisteArticuloDuplicado(articulo.Descripcion, articulo.IdMarca, articulo.IdMedida, articulo.IdTipoConector, articulo.ArticuloId))
                throw new Exception("Ya existe un artículo con la misma combinación de descripción, marca, medida y tipo de conector");

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

        public List<dynamic> ListarConDetalles()
        {
            return _repositorioArticulo.ListarConDetalles();
        }

        public List<dynamic> BuscarConDetalles(string textoBusqueda)
        {
            return _repositorioArticulo.BuscarConDetalles(textoBusqueda);
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

        public string ObtenerProximoCodigo()
        {
            return _repositorioArticulo.ObtenerProximoCodigo();
        }

        public bool ExisteArticuloDuplicado(string descripcion, int? idMarca, int? idMedida, int? idTipoConector, int? articuloIdExcluir = null)
        {
            var articulos = _repositorioArticulo.Listar();
            
            var duplicado = articulos.FirstOrDefault(a =>
                a.Descripcion != null &&
                a.Descripcion.Trim().Equals(descripcion.Trim(), StringComparison.OrdinalIgnoreCase) &&
                a.IdMarca == idMarca &&
                a.IdMedida == idMedida &&
                a.IdTipoConector == idTipoConector &&
                a.FechaAnulacion == null &&
                (articuloIdExcluir == null || a.ArticuloId != articuloIdExcluir.Value)
            );

            return duplicado != null;
        }
    }
}
