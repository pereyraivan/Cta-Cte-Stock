using CEntidades;
using CDatos;
using System.Collections.Generic;

namespace CLogica
{
    public class GestorVendedor
    {
        private RepositorioVendedor _repositorioVendedor = new RepositorioVendedor();

        public void Guardar(Vendedor vendedor)
        {
            _repositorioVendedor.Guardar(vendedor);
        }

        public void Modificar(Vendedor vendedor)
        {
            _repositorioVendedor.Modificar(vendedor);
        }

        public void Eliminar(int id)
        {
            _repositorioVendedor.Eliminar(id);
        }

        public List<Vendedor> Listar()
        {
            return _repositorioVendedor.Listar();
        }
        public List<Vendedor> Buscar(string nombre)
        {
            return _repositorioVendedor.Buscar(nombre);
        }
    }
}
