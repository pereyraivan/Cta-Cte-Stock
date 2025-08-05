using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorMarca
    {
        private RepositorioMarca _repositorioMarca = new RepositorioMarca();

        public List<Marca> ListarMarcas()
        {
            return _repositorioMarca.ListarMarcas();
        }

        public void RegistrarMarca(Marca marca)
        {
            _repositorioMarca.RegistrarMarca(marca);
        }

        public void ModificarMarca(Marca marca)
        {
            _repositorioMarca.ModificarMarca(marca);
        }

        public void EliminarMarca(int id)
        {
            _repositorioMarca.EliminarMarca(id);
        }

        public string ObtenerRespuesta()
        {
            return _repositorioMarca.ObtenerRespuesta();
        }
    }
}
