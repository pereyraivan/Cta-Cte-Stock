using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorMedida
    {
        private RepositorioMedida _repositorioMedida = new RepositorioMedida();

        public List<Medida> ListarMedidas()
        {
            return _repositorioMedida.ListarMedidas();
        }

        public void RegistrarMedida(Medida medida)
        {
            _repositorioMedida.RegistrarMedida(medida);
        }

        public void ModificarMedida(Medida medida)
        {
            _repositorioMedida.ModificarMedida(medida);
        }

        public void EliminarMedida(int id)
        {
            _repositorioMedida.EliminarMedida(id);
        }

        public string ObtenerRespuesta()
        {
            return _repositorioMedida.ObtenerRespuesta();
        }
    }
}
